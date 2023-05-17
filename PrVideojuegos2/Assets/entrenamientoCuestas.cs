using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using weka.classifiers.trees;
using weka.classifiers.evaluation;
using weka.core;
using java.io;
using java.lang;
using java.util;
using weka.classifiers.functions;
using weka.classifiers;
using static CocheBehaviour;
using PathCreation;
using Unity.VisualScripting;
using weka.core.converters;
using com.sun.xml.@internal.bind.v2.runtime.unmarshaller;
using System.Numerics;

public class entrenamientoCuestas : MonoBehaviour
{
    MultilayerPerceptron saberPredecirAceleracion;
    weka.core.Instances casosEntrenamiento;
    public GameObject coche;
    GameObject InstanciaCoche;
    string ESTADO = "Sin conocimiento";
    string acciones;
    public GameObject[] cuestas;
    public Transform[] inicioCuestas;
    public Transform[] finCuestas;
    public Transform[] objetivoCuestas;
    public Transform[] PuntosSpawn;

    private bool metaAlcanzada;
    private int incrementoAceleracion;



    void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 600, 20), "Estado: " + ESTADO);
        GUI.Label(new Rect(10, 20, 600, 20), acciones);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 6f;
        Application.targetFrameRate = 30;

        incrementoAceleracion = 5;
        metaAlcanzada = false;
        StartCoroutine("Entrenamiento");

    }

    IEnumerator Entrenamiento()
    {
        casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/ExperienciasCuesta.arff"));
        int aceleracion = 10;
        for (int indexCuesta = 0; indexCuesta < cuestas.Length ; indexCuesta++) 
        {
            int numPruebas = 0;
            
            while (!metaAlcanzada && numPruebas <= 20)
            {
                GameObject cuestaProbada = cuestas[indexCuesta];
                UnityEngine.Vector3 inicioCuesta = cuestaProbada.GetComponent<PathCreator>().bezierPath.GetPoint(0);
                UnityEngine.Vector3 finCuesta = cuestaProbada.GetComponent<PathCreator>().bezierPath.GetPoint(3);

                InstanciaCoche = Instantiate(coche) as GameObject;
                
                InstanciaCoche.transform.position = PuntosSpawn[indexCuesta].position;
                InstanciaCoche.transform.LookAt(inicioCuestas[indexCuesta].position);


                Rigidbody rb = InstanciaCoche.GetComponent<Rigidbody>();

                InstanciaCoche.GetComponent<CocheBehaviourEntrenamiento>().aceleracion = aceleracion;

               
                
                yield return new WaitUntil(() => (rb.velocity.x < -1f //Si agota la fuerza
                               || UnityEngine.Vector3.Distance(InstanciaCoche.transform.position, objetivoCuestas[indexCuesta].position) <=1) //Si alcanza el final
                               ||InstanciaCoche.transform.position.y < -1f); //Si se cae

                
                
                

                if (UnityEngine.Vector3.Distance(InstanciaCoche.transform.position, objetivoCuestas[indexCuesta].position) <= 1){
                    metaAlcanzada = true; //Para que deje de hacer pruebas con esa cuesta
                    Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
                    //Vector3 dir1 = creator.bezierPath.GetPoint(3) - creator.bezierPath.GetPoint(1);
                    //dir1.Normalize();
                    //print(Vector3.Angle(dir1, transform.right));
                    casoAaprender.setValue(0, aceleracion); //Almacenamos la aceleracion aplicada
                    UnityEngine.Vector3 dir1 = finCuestas[indexCuesta].position - inicioCuestas[indexCuesta].position;
                    dir1.Normalize();
                    float angulo = UnityEngine.Vector3.Angle(dir1, transform.right);
                    casoAaprender.setValue(1, angulo); //Almacenamos el angulo de la cuesta

                    casosEntrenamiento.add(casoAaprender);
                    print("Con una aceleracion de " + aceleracion + " en una cuesta de " + angulo );

                }


                Destroy(InstanciaCoche);

                aceleracion += incrementoAceleracion;


                if (aceleracion == 500)
                {
                    incrementoAceleracion = 50;
                }

            }
            incrementoAceleracion = 5;
            metaAlcanzada = false;



        }
        saberPredecirAceleracion = new MultilayerPerceptron();
        casosEntrenamiento.setClassIndex(0);
        saberPredecirAceleracion.buildClassifier(casosEntrenamiento);

        Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());  //Crea un registro de experiencia durante el juego
        casoPrueba.setDataset(casosEntrenamiento);
        casoPrueba.setValue(1, 20);


        print("Para un cuesta con angulo 20 aplicaria una aceleracion igua a " + (float)saberPredecirAceleracion.classifyInstance(casoPrueba));

        File salida = new File("Assets/Finales_Cuesta.arff");
        if(!salida.exists())
        {
            System.IO.File.Create(salida.getAbsoluteFile().toString()).Dispose();
        }
        ArffSaver saver = new ArffSaver();
        saver.setInstances(casosEntrenamiento);
        saver.setFile(salida);
        saver.writeBatch();

    }

    // Update is called once per frame
    void Update()
    {
        
     

    }

   






}
