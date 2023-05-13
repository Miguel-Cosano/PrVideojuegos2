using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocheBehaviour : MonoBehaviour
{

    public enum Estado { Cuestas, Salto, Hachas, Plataforma, Zigzag };
    Estado estado;

    public Transform checkpoint1;
    public Transform checkpoint2;
    public Transform checkpoint3;
    public Transform checkpoint4;
    public Transform checkpoint5;

    public Raycast raycastScript;
    public float velocidad = 1f;

    // Start is called before the first frame update
    void Start()
    {
        estado = Estado.Plataforma;
        raycastScript = GetComponent<Raycast>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case Estado.Cuestas:
                //Funcion para las cuestas
                break;
            case Estado.Salto:
                //Funcion para los saltos
                break;
            case Estado.Hachas:
                //Funcion para las hachas
                break;
            case Estado.Plataforma:
                //Funcion para la plataforma
                //if(raycastScript)
                //{
                transform.position = new Vector3(transform.position.x - velocidad * Time.deltaTime, transform.position.y, transform.position.z);
                //}
                break;
            case Estado.Zigzag:
                //Funcion para el zigzag
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            // Ir al checkpoint correspondiente
            switch (estado)
            {
                case Estado.Cuestas:
                    transform.position = checkpoint3.position;
                    //transform.rotation = checkpoint3.rotation;
                    break;
                case Estado.Salto:
                    transform.position = checkpoint5.position;
                    //Checkpoint salto
                    break;
                case Estado.Hachas:
                    transform.position = checkpoint4.position;
                    //Checkpoint hacha
                    break;
                case Estado.Plataforma:
                    transform.position = checkpoint1.position;
                    //transform.rotation = Quaternion.Euler(0, 0, 0);
                    //transform.rotation = checkpoint1.rotation;
                    break;
                case Estado.Zigzag:
                    transform.position = checkpoint2.position;
                    //transform.rotation = checkpoint2.rotation;
                    break;
            }
        }
    }
}
