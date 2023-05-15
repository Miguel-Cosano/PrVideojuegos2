using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPrueba : MonoBehaviour
{
    PathCreator creator;
    // Start is called before the first frame update
    void Start()
    {
        creator = GetComponent<PathCreator>();
        print(creator.bezierPath.GetPoint(0));
        print(creator.bezierPath.GetPoint(3));
        Vector3 dir1 = creator.bezierPath.GetPoint(3) - creator.bezierPath.GetPoint(1);
        dir1.Normalize();
        print(Vector3.Angle(dir1, transform.right));
    }

    // Update is called once per framesd
    void Update()
    {
        
    }
}
