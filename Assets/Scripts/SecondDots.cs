using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using Accord.Math.Decompositions;

public class SecondDots : MonoBehaviour
{
    public Material dotMaterial;
    public Material dotMaterialTransparent;
    public Material dotMaterialTransparent2;
    public Material lineMaterial;

    private UnityEngine.Vector3[] defaultCoordinates;
    private UnityEngine.Vector4[] newCoordinates;

    private int dotCount1;
    private int dotCount2;
    private UnityEngine.Vector4[] set1;
    private UnityEngine.Vector4[] set2;

    bool alignmentDone=false;

    void Start()
    {
        //Creating the dots
        int start=0;
        String[] spearator = {" "};
        foreach (string line in System.IO.File.ReadLines("file2.txt"))
        {  
             if(start==0)
             {
                dotCount2=Int32.Parse(line);
                start++;
                set2=new UnityEngine.Vector4[dotCount2];
             }
             else{
                 String[] strlist = line.Split(spearator, 3, StringSplitOptions.RemoveEmptyEntries);

                 GameObject newDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                 newDot.transform.parent=gameObject.transform;
                 newDot.transform.position=newDot.transform.parent.position;
                 newDot.transform.localScale=new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
                 MeshRenderer renderer = newDot.GetComponent<MeshRenderer>();
                 renderer.material=dotMaterial;
                 if(strlist[0].Contains(".") || strlist[1].Contains(".") || strlist[2].Contains("."))
                 {
                    newDot.transform.position+=new UnityEngine.Vector3(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat));
                    set2[start-1]=new UnityEngine.Vector4(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat), 1);
                 }
                 else
                 {
                    newDot.transform.position+=new UnityEngine.Vector3(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]));
                    set2[start-1]=new UnityEngine.Vector4(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]), 1);
                 }

                 start++;
             }
        }
        start=0;

        foreach (string line in System.IO.File.ReadLines("file1.txt"))
        {
            if(start==0)
            {
                dotCount1=Int32.Parse(line);
                start++;
                set1=new UnityEngine.Vector4[dotCount2];
            }
            else{
                String[] strlist = line.Split(spearator, 3, StringSplitOptions.RemoveEmptyEntries);
                if(strlist[0].Contains(".") || strlist[1].Contains(".") || strlist[2].Contains("."))
                    set1[start-1]=new UnityEngine.Vector4(float.Parse(strlist[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(strlist[2], CultureInfo.InvariantCulture.NumberFormat), 1);
                else
                    set1[start-1]=new UnityEngine.Vector4(float.Parse(strlist[0]), float.Parse(strlist[1]), float.Parse(strlist[2]), 1);

                start++;
            }
        }

        //Saving the initial coordinates of the second set
        defaultCoordinates= new UnityEngine.Vector3[gameObject.transform.childCount];
        newCoordinates= new UnityEngine.Vector4[gameObject.transform.childCount];
        for(int i=0; i<gameObject.transform.childCount; ++i)
            defaultCoordinates[i]=gameObject.gameObject.transform.GetChild(i).transform.position;

    }

    void setToFirstSetCoordinates()
    {
        gameObject.transform.position=new UnityEngine.Vector3(0,0,0);
    }

    public void resetToDefaultCoordinates()
    {
        gameObject.transform.position=new UnityEngine.Vector3(20,0,0);
        alignmentDone=false;
        for(int i=0; i<gameObject.transform.childCount; ++i)
            gameObject.transform.GetChild(i).gameObject.transform.position=defaultCoordinates[i];

        var clones = GameObject.FindGameObjectsWithTag ("Clone"); 
        foreach (var clone in clones)
            Destroy(clone);
    }

    private void doAlignment(Matrix4x4 transformation)
    {
        if(alignmentDone==false)
        {
            setToFirstSetCoordinates();
            for(int i=0; i<dotCount2; ++i)
            {
                //Simulating first state in new space
                GameObject cloneDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cloneDot.transform.position=gameObject.transform.GetChild(i).gameObject.transform.position;
                cloneDot.transform.localScale=new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
                MeshRenderer renderer = cloneDot.GetComponent<MeshRenderer>();
                renderer.material=dotMaterialTransparent;
                cloneDot.tag="Clone";

                //Do rotation
                Vector3 newPosition=newCoordinates[i];;
                newPosition+=new Vector3(-transformation.GetColumn(3).x, -transformation.GetColumn(3).y, -transformation.GetColumn(3).z);
                GameObject cloneDot2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cloneDot2.transform.position=newPosition;
                cloneDot2.transform.localScale=new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
                MeshRenderer renderer2 = cloneDot2.GetComponent<MeshRenderer>();
                renderer2.material=dotMaterialTransparent2;
                cloneDot2.tag="Clone";

                //Draw Line
                drawLine(cloneDot2.transform.position, newCoordinates[i], Color.black);

                //Do translation
                gameObject.transform.GetChild(i).gameObject.transform.position=newCoordinates[i];
            }
            alignmentDone=true;
        }

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
            myLine.tag="Clone";
        }
    }

    public void alignWithoutScaling()
    {
        int inlierTreshold=dotCount2/2;
        double[,] set2Pairs=new double[3,3];
        double[,] set1Pairs=new double[3,3];

        //Randomly choose 3 points from file2
        //Choose 3 corresponding points from file 1 assuming they have exact transforms
        for(int i=0; i<dotCount2; ++i)
        {
            for(int j=0; j<dotCount2; ++j)
            {
                if(j==i)
                    j++;
                if(j>=dotCount2)
                    break;

                for(int k=0; k<dotCount2; ++k)
                {
                    if(k==i || k==j)
                        k++;
                    if(k==i || k==j)
                        k++;
                    if(k>=dotCount2)
                        break;

                    for(int m=0; m<dotCount1; ++m)
                    {
                        for(int l=0; l<dotCount1; ++l)
                        {   
                            if(l==m)
                                l++;
                            if(l>=dotCount1)
                                break;

                            for(int z=0; z<dotCount1; ++z)
                            {
                                if(z==m || z==l)
                                    z++;
                                if(z==m || z==l)
                                    z++;
                                if(z>=dotCount1)
                                    break;

                                set2Pairs[0,0]=(double)set2[i].x;
                                set2Pairs[0,1]=(double)set2[i].y;
                                set2Pairs[0,2]=(double)set2[i].z;
                                set2Pairs[1,0]=(double)set2[j].x;
                                set2Pairs[1,1]=(double)set2[j].y;
                                set2Pairs[1,2]=(double)set2[j].z;
                                set2Pairs[2,0]=(double)set2[k].x;
                                set2Pairs[2,1]=(double)set2[k].y;
                                set2Pairs[2,2]=(double)set2[k].z;

                                set1Pairs[0,0]=(double)set1[m].x;
                                set1Pairs[0,1]=(double)set1[m].y;
                                set1Pairs[0,2]=(double)set1[m].z;
                                set1Pairs[1,0]=(double)set1[l].x;
                                set1Pairs[1,1]=(double)set1[l].y;
                                set1Pairs[1,2]=(double)set1[l].z;
                                set1Pairs[2,0]=(double)set1[z].x;
                                set1Pairs[2,1]=(double)set1[z].y;
                                set1Pairs[2,2]=(double)set1[z].z;
                                
                                //Calculate their transformation
                                UnityEngine.Matrix4x4 estimatedTransform=calcTransform(set2Pairs, set1Pairs);

                                //For the calculated transformation, count the inliers
                                int inlierCount=findInliersCount(estimatedTransform);

                                //If the inlier count is bigger than treshold, transform is found. If not try again for another point match
                                if(inlierCount>=inlierTreshold)
                                {
                                    doAlignment(estimatedTransform);
                                    return;
                                }
                            }

                        }

                    }

                }

            }

        }
    }

    private UnityEngine.Matrix4x4 calcTransform(double[,] secondSet, double[,] firstSet)
    {
       //Pnew=T*Pold
       //Find T which is a 4x4 matrix given 3 pairs of points by solving the system of linear equations

       //translate points to their centroids
       double[,] centroid1=MeanAlongXD(secondSet);
       double[,] centroid2=MeanAlongXD(firstSet);
       double[,] AA=SubstractInequalMatrixD(secondSet, centroid1);
       double[,] BB=SubstractInequalMatrixD(firstSet, centroid2);

       //rotation matrix
       double[,] H=MultiplyMatrixD(TransposeD(AA), BB);
       SingularValueDecomposition svd= new SingularValueDecomposition(H, true, true);
       double[,] R=MultiplyMatrixD(svd.RightSingularVectors, TransposeD(svd.LeftSingularVectors));

       //Special Reflection Case
       if(DeterminantD(R)<0)
       {
            svd.RightSingularVectors[0, 2]*=-1;
            svd.RightSingularVectors[1, 2]*=-1;
            svd.RightSingularVectors[2, 2]*=-1;
            R=MultiplyMatrixD(svd.RightSingularVectors, TransposeD(svd.LeftSingularVectors));
       }

       //translation
       double[,] t=SubstractMatrixD(centroid2, MultiplyMatrixD(R, centroid1));
       
       //homogeneous transformation
       UnityEngine.Matrix4x4 T=UnityEngine.Matrix4x4.identity;
       T.SetRow(0, new UnityEngine.Vector4((float)R[0,0], (float)R[0,1], (float)R[0,2], (float)t[0,0]));
       T.SetRow(1, new UnityEngine.Vector4((float)R[1,0], (float)R[1,1], (float)R[1,2], (float)t[1,0]));
       T.SetRow(2, new UnityEngine.Vector4((float)R[2,0], (float)R[2,1], (float)R[2,2], (float)t[2,0]));
       T.SetRow(3, new UnityEngine.Vector4(0,0,0,1));

       return T;
    }

    private int findInliersCount(UnityEngine.Matrix4x4 transformVector)
    {
        int foundInlierCount=0;
        float distanceTreshold=0.1f;

        for(int i=0; i<dotCount2; ++i)
        {
            newCoordinates[i]=transformVector*set2[i];
            
            for(int j=0; j<dotCount1; ++j)
            {
                if(distanceOfVectors(newCoordinates[i], set1[j])<=distanceTreshold)
                {
                    foundInlierCount++;
                    break;
                }
            }
        }

        return foundInlierCount;
    }

    private float distanceOfVectors(UnityEngine.Vector4 vector1, UnityEngine.Vector4 vector2)
    { 
        return Math.Abs((vector1-vector2).magnitude);
    }

    private double DeterminantD(double[,] arr1)
    {
        double det=0;
        for(int i=0; i<3; i++)
        {
            det = det + (arr1[0,i]*(arr1[1,(i+1)%3]*arr1[2,(i+2)%3] - arr1[1,(i+2)%3]*arr1[2,(i+1)%3]));
        }

        return det;
    }

    private double[,] MultiplyMatrixD(double[,] A, double[,] B)
    {
        int rA = A.GetLength(0);
        int cA = A.GetLength(1);
        int rB = B.GetLength(0);
        int cB = B.GetLength(1);

        double[,] kHasil = new  double[rA, cB];

        double temp = 0;

        for (int i = 0; i < rA; i++)
        {
            for (int j = 0; j < cB; j++)
            {
                temp = 0;
                for (int k = 0; k < cA; k++)
                {
                    temp += A[i, k] * B[k, j];
                }
                kHasil[i, j] = temp;
            }
        }

        return kHasil;
    }

    private double[,] SubstractMatrixD(double[,] A, double[,] B)
    {
        double[,] result=new double[3,3];

        int rA = A.GetLength(0);
        int cA = A.GetLength(1);
        int rB = B.GetLength(0);
        int cB = B.GetLength(1);

        if(rA==rB && cA==cB)
        {
            for(int i=0; i<rA; i++)
                for(int j=0; j<cA; ++j)
                    result[i,j]=A[i,j]-B[i,j];
        }

        return result;
    }

    private double[,] SubstractInequalMatrixD(double[,] A, double[,] B)
    {
        double[,] result=new double[3,3];

        for(int i=0; i<3; ++i)
            for(int j=0; j<3; ++j)
                result[i,j]=A[i,j]-B[j,0];

        return result;
    }

    private double[,] MeanAlongXD(double[,] A)
    {
        double[,] result=new double[3,1];

        for(int i=0; i<3; ++i)
            result[i,0]=(A[0,i]+A[1,i]+A[2,i])/3;

        return result;
    }

    public double[,] TransposeD(double[,] matrix)
    {
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        double[,] result = new double[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    public void printArrayD(double[,] array)
    {
        int w =  array.GetLength(0);
        int h =  array.GetLength(1);

        for(int i=0; i<w; ++i)
            for(int j=0; j<h; ++j)
                UnityEngine.Debug.Log(array[i,j]);
    }
}
