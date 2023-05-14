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

public class entrenamientoCuestas : MonoBehaviour
{
    weka.classifiers.trees.M5P saberPredecirFuerzaX;
    weka.core.Instances casosEntrenamiento;
    public GameObject coche;
    GameObject InstanciaCoche;
    string ESTADO = "Sin conocimiento";
    string acciones;

    private bool metaAlcanzada;



    void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 600, 20), "Estado: " + ESTADO);
        GUI.Label(new Rect(10, 20, 600, 20), acciones);
    }

    // Start is called before the first frame update
    void Start()
    {
        metaAlcanzada = false;
        StartCoroutine("Entrenamiento");

    }

    IEnumerator Entrenamiento()
    {
        casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/ExperienciasCuesta.arff"));

        for(float aceleracion = 5; aceleracion <= 1000; aceleracion = aceleracion+10)
        {
            InstanciaCoche = Instantiate(coche) as GameObject;
            InstanciaCoche.GetComponent<CocheBehaviour>().aceleracion = aceleracion;
            float tiempoInicio = Time.deltaTime;
            yield return new WaitUntil(() => (metaAlcanzada || Time.deltaTime - tiempoInicio >= 10)); //Espera hasta que alcanza la meta o pasan 10 seg


            Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
            acciones = "Generando experiencia con aceleracion= "+aceleracion +

        }
    }

    // Update is called once per frame
    void Update()
    {
        
     

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Meta"))
        {
            metaAlcanzada = true;
        }
    }






}
