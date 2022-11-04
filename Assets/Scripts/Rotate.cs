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
        Matrix4x4 scale = Matrix4x4.identity;
        scale[0,0]=1.5f;
        scale[0,1]=0;
        scale[0,2]=0;
        scale[0,3]=0;
        scale[1,0]=0;
        scale[1,1]=1.5f;
        scale[1,2]=0;
        scale[1,3]=0;
        scale[2,0]=0;
        scale[2,1]=0;
        scale[2,2]=1.5f;
        scale[2,3]=0;
        scale[3,0]=0;
        scale[3,1]=0;
        scale[3,2]=0;
        scale[3,3]=1;

        Matrix4x4 m = Matrix4x4.zero;
        m.SetRow(0, new Vector4(0.4330127f, -0.7500000f,  0.5000000f, 0));
        m.SetRow(1, new Vector4(0.8750000f,  0.2165063f, -0.4330127f, 0));
        m.SetRow(2, new Vector4(0.2165063f,  0.6250000f,  0.7500000f, 0));
        m.SetRow(3, new Vector4(0,0,0,1));
        m=scale*m;

        transform.position=m.MultiplyPoint3x4(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
