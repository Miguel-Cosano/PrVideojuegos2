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
    
    // Añadir velocidad y array de los raycasts    
  
    


    public WheelCollider delanteDerecha;
    public WheelCollider delanteIzquierda;
    public WheelCollider detrasDerecha;
    public WheelCollider detrasIzquierda;

    public Transform delanteDerechaTransform;
    public Transform delanteIzquierdaTransform;
    public Transform detrasDerechaTransform;
    public Transform detrasIzquierdaTransform;



    public float aceleracion = 500f;
    public float fuerzaDeRotura = 300f;


    private float acelaracionActual = 0f;
    private float fuerzaDeRoturaActual = 0f;

    private Rigidbody rigidBody;

    public Transform[] raycast;
    RaycastHit[] hits = new RaycastHit[3];

    // Start is called before the first frame update
    void Start()
    {

        estado = Estado.Cuestas;
        rigidBody = GetComponent<Rigidbody>();
  

    }

    // Update is called once per frame
    void Update()
    {
        //Aplicamos fuerza sobre el coche
        rigidBody.AddRelativeForce(aceleracion * Vector3.forward);

        acelaracionActual = aceleracion;
        
        //Establecemos el giro de las ruedas en funcion de la fuerza
        delanteDerecha.motorTorque = acelaracionActual;
        delanteIzquierda.motorTorque = acelaracionActual;

        //Establecemos la fuerza que es necesaria aplicar para que empiece a moverse
        delanteDerecha.brakeTorque = fuerzaDeRoturaActual;
        delanteIzquierda.brakeTorque = fuerzaDeRoturaActual;


        updateRuedas(); //Metodo para hacer que las ruedas giren
        observarCarretera();//Metodo para que el coche gire si detecta que la carretera gira a izquierda o a derecha
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
                /* Aquí si se identifica que el raycast central apunta a carretera o plataforma, avanza
                if(raycastScript)
                {
                    transform.position = new Vector3(transform.position.x - velocidad * Time.deltaTime, transform.position.y, transform.position.z);
                }
                */
                break;
            case Estado.Zigzag:
                //Funcion para el zigzag
                break;
        }
        
    }

    void observarCarretera()
    {

        for (int cont = 0; cont < raycast.Length; cont++)
        {
            if (Physics.Raycast(raycast[cont].position, raycast[cont].forward, out hits[cont], 30))
            {

                if (hits[cont].collider.gameObject.CompareTag("Suelo"))
                {
                    if (cont == 1) // Se acerca una curva hacia la derecha 
                    {
                        transform.Rotate(0, 30 * Time.deltaTime, 0);
                    }
                    else if (cont == 2) //Se acerca una curva hacia la izquierda
                    {
                        transform.Rotate(0, -30 * Time.deltaTime, 0);
                    }
                }



            }
        }

    }

    void updateRuedas()
    {
        if (acelaracionActual != 0f) //En caso de que no este parado
        {
            delanteDerechaTransform.Rotate(0, 0, 500 * Time.deltaTime);
            delanteIzquierdaTransform.Rotate(0, 0, 500 * Time.deltaTime);
            detrasDerechaTransform.Rotate(0, 0, 500 * Time.deltaTime);
            detrasIzquierdaTransform.Rotate(0, 0, 500 * Time.deltaTime);
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
