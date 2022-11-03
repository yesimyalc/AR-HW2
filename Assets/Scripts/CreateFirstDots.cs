using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class CreateFirstDots : MonoBehaviour
{
    // Start is called before the first frame update
    private int dotCount;
    public Material dotMaterial;
    void Start()
    {
        int start=0;
        String[] spearator = {" "};
        foreach (string line in System.IO.File.ReadLines("file1.txt"))
        {  
             if(start==0)
             {
                dotCount=Int32.Parse(line);
                start++;
             }
             else{
                 String[] strlist = line.Split(spearator, 3, StringSplitOptions.RemoveEmptyEntries);

                 GameObject newDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                 newDot.transform.parent=gameObject.transform;
                 newDot.transform.localScale=new Vector3(0.5f, 0.5f, 0.5f);
                 MeshRenderer renderer = newDot.GetComponent<MeshRenderer>();
                 renderer.material=dotMaterial;
                 if(strlist[0].Contains("."))
                    newDot.transform.position=new Vector3(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat));
                 else
                    newDot.transform.position=new Vector3(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]));

             }
        }

    }

}
