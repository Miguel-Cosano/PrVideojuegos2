using com.sun.corba.se.pept.broker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocheBehaviourHachas : MonoBehaviour
{
   public enum Estado { Salto, Hachas, Completado };
    Estado estado;

    public WheelCollider delanteDerecha;
    public WheelCollider delanteIzquierda;
    public WheelCollider detrasDerecha;
    public WheelCollider detrasIzquierda;

    public Transform delanteDerechaTransform;
    public Transform delanteIzquierdaTransform;
    public Transform detrasDerechaTransform;
    public Transform detrasIzquierdaTransform;

    public Transform puntoInicio;
    public Transform MetaHachas;
    public Transform MetaSalto;
    public Transform completado;

    private Rigidbody rigidBody;

    public float aceleracion = 0;
    public float fuerzaDeRotura = 300f;


    private float acelaracionActual = 0;
    private float fuerzaDeRoturaActual = 0f;

    private float impulso;
    public GameObject[] hachas = new GameObject[4];
    public GameObject bola;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        rigidBody = GetComponent<Rigidbody>();
        estado = Estado.Hachas;
        aceleracion = 10;
        impulso = 30;
    }

    // Update is called once per frame
    void Update()
    {

        acelaracionActual = aceleracion;

        //Establecemos el giro de las ruedas en funcion de la fuerza
        delanteDerecha.motorTorque = acelaracionActual;
        delanteIzquierda.motorTorque = acelaracionActual;

        //Establecemos la fuerza que es necesaria aplicar para que empiece a moverse
        delanteDerecha.brakeTorque = 0;
        delanteIzquierda.brakeTorque = 0;

        updateRuedas(); //Metodo para hacer que las ruedas giren

        switch (estado)
        {
            case Estado.Hachas:
                pruebaHachas();
                break;
            case Estado.Salto:
                pruebaSalto();
                break;
            case Estado.Completado:
                entrenamientoCompletado();
                break;
        }
    }

    void pruebaHachas()
    {
        if ((Vector3.Distance(MetaHachas.position, transform.position) <= 0.5))
        {
            print("Nivel de hachas superado");
            estado = Estado.Salto;
            transform.position = MetaHachas.position;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    void pruebaSalto()
    {
        if (Vector3.Distance(MetaSalto.position, transform.position) >= 1)
        {
            rigidBody.AddRelativeForce(impulso * Vector3.forward, ForceMode.Impulse);
        }
        if ((Vector3.Distance(MetaSalto.position, transform.position) <= 0.5))
        {
            print("Nivel del salto superado. Entrenamiento completado con aceleracion " + aceleracion + " y impulso " + impulso);
            estado = Estado.Completado;
            transform.position = completado.position;
        }
    }

    void entrenamientoCompletado()
    {
        transform.position = completado.position;
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
        switch (estado)
        {
            case Estado.Hachas:
                transform.position = puntoInicio.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                aceleracion += 5;
                print(aceleracion);
                foreach (GameObject hacha in hachas)
                {
                    hacha.GetComponent<AxeBehaviour>().ReStart();
                }
                break;
            case Estado.Salto:
                transform.position = MetaHachas.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                if(impulso >= 50)
                {
                    impulso += 3;
                }else
                {
                    impulso += 10;
                }
                print(impulso);
                bola.GetComponent<BolaBehaviour>().ReStart();
                break;
        }
    }
}
