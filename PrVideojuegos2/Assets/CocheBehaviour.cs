using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weka.classifiers.trees;
using weka.classifiers.evaluation;
using weka.core;
using java.io;
using java.lang;
using java.util;
using weka.classifiers.functions;
using weka.classifiers;
using weka.core.converters;

public class CocheBehaviour : MonoBehaviour
{

    public enum Estado { Cuestas, Salto, Hachas, Plataforma, Zigzag, Completado };
    Estado estado;

    M5P saberPredecirAceleracion;
    weka.core.Instances casosEntrenamiento;

    public Transform checkpoint1;
    public Transform checkpoint2;
    public Transform checkpoint3;
    public Transform checkpoint4;
    public Transform checkpoint5;
    public Transform checkpoint6;
    public Transform metaPlataforma;
    public Transform metaZigZag;
    public Transform metaCuestas;
    public Transform metaHachas;
    public Transform metaSalto;

    public WheelCollider delanteDerecha;
    public WheelCollider delanteIzquierda;
    public WheelCollider detrasDerecha;
    public WheelCollider detrasIzquierda;

    public Transform delanteDerechaTransform;
    public Transform delanteIzquierdaTransform;
    public Transform detrasDerechaTransform;
    public Transform detrasIzquierdaTransform;

    public Transform inicioCuestaObjetivo;
    public Transform finCuestaObjetivo;



    public float aceleracion = 500f;
    public float fuerzaDeRotura = 300f;


    private float acelaracionActual = 0f;
    private float fuerzaDeRoturaActual = 0f;


    private Rigidbody rigidBody;

    public Transform[] raycast;
    RaycastHit[] hits = new RaycastHit[3];
    public bool usarModelo;

    private int indexCuesta;

    float Brakes;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        indexCuesta = 0;

        estado = Estado.Hachas;
        rigidBody = GetComponent<Rigidbody>();

        Brakes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Aplicamos fuerza sobre el coche


        acelaracionActual = aceleracion;

        //Establecemos el giro de las ruedas en funcion de la fuerza
        delanteDerecha.motorTorque = acelaracionActual;
        delanteIzquierda.motorTorque = acelaracionActual;

        //Establecemos la fuerza que es necesaria aplicar para que empiece a moverse
        delanteDerecha.brakeTorque = 0;
        delanteIzquierda.brakeTorque = 0;


        updateRuedas(); //Metodo para hacer que las ruedas giren
        //observarCarretera();//Metodo para que el coche gire si detecta que la carretera gira a izquierda o a derecha
        switch (estado)
        {
            case Estado.Cuestas:
                pruebaCuesta();
                break;
            case Estado.Salto:
                pruebaSalto();
                break;
            case Estado.Hachas:
                pruebaHachas();
                break;
            case Estado.Plataforma:
                pruebaPlataforma();
                break;
            case Estado.Zigzag:
                pruebaZigzag();
                break;
            case Estado.Completado:
                completado();
                break;
        }

    }

    void pruebaCuesta()
    {
        if (usarModelo)
        {
            casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/Finales_Cuesta.arff"));
            saberPredecirAceleracion = new M5P();
            casosEntrenamiento.setClassIndex(0);
            saberPredecirAceleracion.buildClassifier(casosEntrenamiento);

            //Calculo angulo y longitud de la cuesta
            UnityEngine.Vector3 dir1 = finCuestaObjetivo.position - inicioCuestaObjetivo.position;
            dir1.Normalize();
            float angulo = UnityEngine.Vector3.Angle(dir1, Vector3.left);

            Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());
            casoPrueba.setValue(1, angulo);

            aceleracion = (float)saberPredecirAceleracion.classifyInstance(casoPrueba);
            usarModelo = false;

            print("Para esta cuesta de " + angulo + "º grados voy a aplicar una fuerza de " + aceleracion);
        }


        if (Vector3.Distance(finCuestaObjetivo.position, transform.position) >= 1)
        {
            rigidBody.AddRelativeForce(aceleracion * Vector3.forward, ForceMode.Impulse);
        }

        if ((Vector3.Distance(metaCuestas.position, transform.position) <= 2.5))
        {
            rigidBody.AddRelativeForce(aceleracion * Vector3.back, ForceMode.Impulse);
            print("Nivel de las cuestas superado");
            estado = Estado.Hachas;
            transform.position = checkpoint4.position;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    void pruebaPlataforma()
    {
        for (int cont = 0; cont < raycast.Length; cont++)
        {
            if (Physics.Raycast(raycast[cont].position, raycast[cont].forward, out hits[cont], 30))
            {

                if (hits[cont].collider.gameObject.CompareTag("Carretera") || hits[cont].collider.gameObject.CompareTag("Plataforma"))
                {
                    if (cont == 0) 
                    {
                        aceleracion = 25;
                    } 
                }
                else
                {
                    if (cont == 0)
                    {
                        Brakes = 300;
                        delanteDerecha.brakeTorque = Brakes;
                        delanteIzquierda.brakeTorque = Brakes;
                    }
                }
            }
        }

        if((Vector3.Distance(metaPlataforma.position, transform.position) <= 0.5))
        {
            print("Nivel de la plataforma superado");
            estado = Estado.Zigzag;
            transform.position = checkpoint2.position;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    void pruebaZigzag()
    {
        
        aceleracion = 14;
        for (int cont = 0; cont < raycast.Length; cont++)
        {
            if (Physics.Raycast(raycast[cont].position, raycast[cont].forward, out hits[cont], 30))
            {

                if (hits[cont].collider.gameObject.CompareTag("Suelo"))
                {
                    if (cont == 1) // Se acerca una curva hacia la derecha 
                    {
                        girarDerecha();
                    }
                    else if (cont == 2) //Se acerca una curva hacia la izquierda
                    {
                        girarIzquierda();
                    }
                }
            }
        }
        if ((Vector3.Distance(metaZigZag.position, transform.position) <= 0.5))
        {
            print("Nivel del zigzag superado");
            estado = Estado.Cuestas;
            transform.position = checkpoint3.position;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    void girarDerecha()
    {
        if(hits[1].collider.gameObject.CompareTag("Suelo"))
        {
            transform.Rotate(0, 80 * Time.deltaTime, 0);
        }
    }

    void girarIzquierda()
    {
        if (hits[2].collider.gameObject.CompareTag("Suelo"))
        {
            transform.Rotate(0, -80 * Time.deltaTime, 0);
        }
    }

    void pruebaHachas()
    {
        aceleracion = 25;
        if ((Vector3.Distance(metaHachas.position, transform.position) <= 0.5))
        {
            print("Nivel de hachas superado");
            estado = Estado.Salto;
            transform.position = checkpoint5.position;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    void pruebaSalto()
    {
        aceleracion = 53;
        if (Vector3.Distance(metaSalto.position, transform.position) >= 1)
        {
            rigidBody.AddRelativeForce(aceleracion * Vector3.forward, ForceMode.Impulse);
        }
        if ((Vector3.Distance(metaSalto.position, transform.position) <= 0.5))
        {
            print("Nivel del salto superado. Juego completado");
            estado = Estado.Completado;
            transform.position = checkpoint6.position;
        }
    }

    void completado()
    {
        Brakes = 300;
        delanteDerecha.brakeTorque = Brakes;
        delanteIzquierda.brakeTorque = Brakes;
        transform.position = checkpoint6.position;
        transform.Rotate(0f, 0f, 0f);
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

    private void OnTriggerEnter()
    {
        // Ir al checkpoint correspondiente
        switch (estado)
        {
            case Estado.Cuestas:
                transform.position = checkpoint3.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //transform.rotation = checkpoint3.rotation;
                break;
            case Estado.Salto:
                transform.position = checkpoint5.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //Checkpoint salto
                break;
            case Estado.Hachas:
                transform.position = checkpoint4.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //Checkpoint hacha
                break;
            case Estado.Plataforma:
                transform.position = checkpoint1.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //transform.rotation = checkpoint1.rotation;
                break;
            case Estado.Zigzag:
                transform.position = checkpoint2.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                //transform.rotation = checkpoint2.rotation;
                break;
        }
    }
}
