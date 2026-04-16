using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10;

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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        rb.linearVelocity = dir * speed;
    }


}
