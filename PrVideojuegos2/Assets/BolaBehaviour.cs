using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0f, 0f, 90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReStart()
    {
        transform.rotation = new Quaternion(0f, 0f, 0f, 1);
        transform.position = new Vector3(0f, 0f, 90f);
    }
}
