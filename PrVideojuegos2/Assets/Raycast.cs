using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public bool verRayo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if(verRayo)
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.blue);

            }
            // print("Objeto en frente " + hit.collider.gameObject.tag+ " a distancia: " + hit.distance);


        }
        else
        {
            if(verRayo) {
                Debug.DrawRay(transform.position, transform.forward * 1000, Color.black);
            }
        }
            //print("No hay objeto en frente");
            //

    }
}
