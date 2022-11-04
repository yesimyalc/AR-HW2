using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using UnityEngine.UI;

public class CreateFirstDots : MonoBehaviour
{
    // Start is called before the first frame update
    private int dotCount;
    private int dotCount11;
    private UnityEngine.Vector4[] firstDots;
    private UnityEngine.Vector4[] scaledDots;
    private bool isFirst=true;
    private bool isScaled=false;

    public Material dotMaterial;
    public GameObject alignWOScalingButton;
    public GameObject alignWScalingButton;
    public Button resetButton;

    void Start()
    {
        int start=0;
        String[] spearator = {" "};
        foreach (string line in System.IO.File.ReadLines("file1.txt"))
        {  
             if(start==0)
             {
                dotCount=Int32.Parse(line);
                firstDots=new UnityEngine.Vector4[dotCount];
                start++;
             }
             else{
                 String[] strlist = line.Split(spearator, 3, StringSplitOptions.RemoveEmptyEntries);

                 GameObject newDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                 newDot.transform.parent=gameObject.transform;
                 newDot.transform.localScale=new Vector3(0.5f, 0.5f, 0.5f);
                 MeshRenderer renderer = newDot.GetComponent<MeshRenderer>();
                 newDot.tag="FirstDot";
                 renderer.material=dotMaterial;

                 if(strlist[0].Contains(".") || strlist[1].Contains(".") || strlist[2].Contains("."))
                 {
                    firstDots[start-1]=new Vector3(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat));
                    newDot.transform.position=new Vector3(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat));
                 }
                 else
                 {
                    firstDots[start-1]=new Vector3(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]));
                    newDot.transform.position=new Vector3(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]));
                 }
                 start++;
             }
        }

        start=0;

        foreach (string line in System.IO.File.ReadLines("file11.txt"))
        {  
             if(start==0)
             {
                dotCount11=Int32.Parse(line);
                scaledDots=new UnityEngine.Vector4[dotCount11];
                start++;
             }
             else{
                 String[] strlist = line.Split(spearator, 3, StringSplitOptions.RemoveEmptyEntries);

                 if(strlist[0].Contains(".") || strlist[1].Contains(".") || strlist[2].Contains("."))
                    scaledDots[start-1]=new Vector3(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat));
                 else 
                    scaledDots[start-1]=new Vector3(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]));

                 start++;
             }
        }

        alignWOScalingButton.SetActive(true);
        alignWScalingButton.SetActive(false);

    }

    public void switchToScaledDots()
    {
        if(isScaled==true)
            return;

        alignWScalingButton.SetActive(true);
        alignWOScalingButton.SetActive(false);

        var firstDots = GameObject.FindGameObjectsWithTag ("FirstDot"); 
        foreach (var firstDot in firstDots)
            Destroy(firstDot);

        for(int i=0; i<dotCount11; ++i)
        {
            GameObject newDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newDot.transform.parent=gameObject.transform;
            newDot.transform.localScale=new Vector3(0.5f, 0.5f, 0.5f);
            MeshRenderer renderer = newDot.GetComponent<MeshRenderer>();
            newDot.tag="ScaledDot";
            renderer.material=dotMaterial;
            newDot.transform.position=scaledDots[i];
        }
        isScaled=true;
        isFirst=false;

        resetButton.onClick.Invoke();
    }

    public void switchToFirstDots()
    {
        if(isFirst==true)
            return;

        alignWOScalingButton.SetActive(true);
        alignWScalingButton.SetActive(false);

        var scaledDots = GameObject.FindGameObjectsWithTag ("ScaledDot"); 
        foreach (var scaledDot in scaledDots)
            Destroy(scaledDot);

        for(int i=0; i<dotCount; ++i)
        {
            GameObject newDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newDot.transform.parent=gameObject.transform;
            newDot.transform.localScale=new Vector3(0.5f, 0.5f, 0.5f);
            MeshRenderer renderer = newDot.GetComponent<MeshRenderer>();
            newDot.tag="FirstDot";
            renderer.material=dotMaterial;
            newDot.transform.position=firstDots[i];
        }
        isFirst=true;
        isScaled=false;

        resetButton.onClick.Invoke();
    }

}
