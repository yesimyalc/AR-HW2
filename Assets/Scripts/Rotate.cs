using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform objectToRotateAround;
    void Start()
    {
        /*//Rotate 30 degrees by x
        transform.RotateAround(objectToRotateAround.transform.position, Vector3.right, 30);
        //Rotate 30 degrees by y
        transform.RotateAround(objectToRotateAround.transform.position, Vector3.up, 30);
        //Rotate 60 degrees by z
        transform.RotateAround(objectToRotateAround.transform.position, Vector3.forward, 60);
        //Translate +10 by x*/

        Vector3 translation=new Vector3(0, 0, 0);
        Vector3 scale = new Vector3(1, 1, 1);

        Matrix4x4 m = Matrix4x4.zero;
        m.SetRow(0, new Vector4(0.4330127f, -0.7500000f,  0.5000000f, 0));
        m.SetRow(1, new Vector4(0.8750000f,  0.2165063f, -0.4330127f, 0));
        m.SetRow(2, new Vector4(0.2165063f,  0.6250000f,  0.7500000f, 0));
        m.SetRow(3, new Vector4(0,0,0,1));

        transform.position=m.MultiplyPoint3x4(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
