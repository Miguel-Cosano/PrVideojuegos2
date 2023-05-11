using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBehaviour : MonoBehaviour
{
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0f, 0f, velocity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
