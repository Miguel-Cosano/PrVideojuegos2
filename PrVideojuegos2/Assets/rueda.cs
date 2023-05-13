using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rueda : MonoBehaviour
{
    public int fuerza;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //fuerza = transform.GetComponent<carScript>().fuerzaAplicada;

        float turn = Input.GetAxis("Horizontal");
        rb.AddRelativeTorque(Vector3.forward * fuerza);
    }
}
