using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] Transform camara; 
     Rigidbody rb;
    private Vector3 velocidadVertical;
    //[SerializeField] private float gravedad = 5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        Movement();
        //AgregarGravedad();
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara = camara.right;

        adelanteCamara.y = 0f;
        derechaCamara.y = 0f; // ⚠️ acá tenías un bug antes (usabas x)

        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        Vector3 direccionPlano = (derechaCamara * h + adelanteCamara * v);

        if (direccionPlano.sqrMagnitude > 0.0001f)
        {
            direccionPlano.Normalize();
        }

        Vector3 desplazamientoXZ = direccionPlano * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + desplazamientoXZ);
    }

    //private void AgregarGravedad()
    //{
    //    velocidadVertical.y -= gravedad * Time.deltaTime;

    //    characterController.Move(velocidadVertical *Time.deltaTime);

    //    if(characterController.isGrounded && velocidadVertical.y <0)
    //    {
    //        velocidadVertical.y = -2f;
    //    }

    //}


}
