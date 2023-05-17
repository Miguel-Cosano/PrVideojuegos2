using java.lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBehaviour : MonoBehaviour
{
    public float velocity;
    public bool volteado;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0f, 0f, velocity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReStart()
    {
        /*if(volteado) transform.rotation = new Quaternion(0f, 180f, -90f, 1);
        else transform.rotation = new Quaternion(0f, 0f, -90f, 1);*/
        transform.rotation = new Quaternion(0f, 0f, -90f, 1);
        transform.position = new Vector3(0f, 0f, velocity);
    }
}
