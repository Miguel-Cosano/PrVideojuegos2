using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carScript : MonoBehaviour
{
    public Transform DelanteraDerecha;
    public Transform DelanteraIzquierda;
    public Transform TraseraDerecha;
    public Transform TraseraIzquierda;

    public int fuerzaAplicada;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
      //  DelanteraDerecha.Rotate(0, 0, 500 * Time.deltaTime);
      //  DelanteraIzquierda.Rotate(0, 0, 500 * Time.deltaTime);
      //  TraseraDerecha.Rotate(0, 0, 500 * Time.deltaTime);
      //  TraseraIzquierda.Rotate(0, 0, 500 * Time.deltaTime);
    }
}
