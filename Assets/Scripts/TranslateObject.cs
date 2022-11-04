using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    public Transform objectToRotateAround;
    private Vector3 old;
    public Material lineMaterial;

    void Start()
    {
        /*//Rotate 30 degrees by x
        transform.RotateAround(objectToRotateAround.transform.position, Vector3.right, 30);
        //Rotate 30 degrees by y
        transform.RotateAround(objectToRotateAround.transform.position, Vector3.up, 30);
        //Rotate 60 degrees by z
        transform.RotateAround(objectToRotateAround.transform.position, Vector3.forward, 60);
        //Translate +10 by x
        old=transform.position;
        transform.Translate(Vector3.right*10, Space.World);
        //Translate +5 by y
        transform.Translate(Vector3.up*5, Space.World);
        //Translate +3 by z
        transform.Translate(Vector3.forward*3, Space.World);*/

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
        m=scale*m;
        m.SetRow(3, new Vector4(0,0,0,1));

        transform.position=m.MultiplyPoint3x4(transform.position);

        old=transform.position;
        transform.position+=new Vector3(10, 5, 3);

        drawLine(old, transform.position, Color.black);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void drawLine(Vector3 start, Vector3 end, Color color)
    {
        if(lineMaterial!=null)
        {
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.SetColors(color, color);
            lr.SetWidth(0.1f, 0.1f);
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }
    }

}
