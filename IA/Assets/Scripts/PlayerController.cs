using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f; 
    [SerializeField] Transform camara;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara = camara.right;

        adelanteCamara.y = 0f;
        derechaCamara.y = 0f;

        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        Vector3 direccionPlano = (derechaCamara * h + adelanteCamara * v);

        if (direccionPlano.sqrMagnitude > 0.0001f)
        {
            direccionPlano.Normalize();
        }

        Vector3 desplazamientoXZ = direccionPlano * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + desplazamientoXZ);
    }
}