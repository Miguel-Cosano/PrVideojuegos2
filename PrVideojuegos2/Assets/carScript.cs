using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CocheBehaviour;

public class carScript : MonoBehaviour
{
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
        rigidBody = GetComponent<Rigidbody>();
  
    }

    // Update is called once per frame
    void Update()
    {
     
        rigidBody.AddRelativeForce(aceleracion * Vector3.forward);
        //updateRuedas()
        acelaracionActual = aceleracion;

        //Establecemos el giro de las ruedas en funcion de la fuerza
        delanteDerecha.motorTorque = acelaracionActual;
        delanteIzquierda.motorTorque = acelaracionActual;

        //Establecemos la fuerza que es necesaria aplicar para que empiece a moverse
        delanteDerecha.brakeTorque = fuerzaDeRoturaActual;
        delanteIzquierda.brakeTorque = fuerzaDeRoturaActual;


        updateRuedas();
        observarCarretera();
    }

    void observarCarretera()
    {
        
        for (int cont = 0; cont < raycast.Length; cont++)
        {
            if (Physics.Raycast(raycast[cont].position, raycast[cont].forward, out hits[cont], 30))
            {
                
                    if (hits[cont].collider.gameObject.CompareTag("Suelo"))
                    {
                        if(cont == 1) // Se acerca una curva hacia la derecha 
                        {
                            transform.Rotate(0, 30 * Time.deltaTime, 0);
                        }else if(cont == 2) //Se acerca una curva hacia la izquierda
                        {
                            transform.Rotate(0, -30 * Time.deltaTime, 0);
                        }
                    }
                    
                
                
            }
        }
        
    }

    void updateRuedas() 
    {
        if(acelaracionActual != 0f) //En caso de que no este parado
        {
            delanteDerechaTransform.Rotate(0, 0, 500 * Time.deltaTime);
            delanteIzquierdaTransform.Rotate(0,0,500 * Time.deltaTime);
            detrasDerechaTransform.Rotate(0, 0, 500 * Time.deltaTime);
            detrasIzquierdaTransform.Rotate(0,0,500 * Time.deltaTime);
        }

    }
  


  
}
