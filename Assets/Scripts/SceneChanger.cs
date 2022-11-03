using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject sceneButtonsUI;

    public void changeToFirst()
    {
        int currentIndex=SceneManager.GetActiveScene().buildIndex;
        if(currentIndex!=0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void changeToSecond()
    {
        int currentIndex=SceneManager.GetActiveScene().buildIndex;
        if(currentIndex!=1)
        {
            SceneManager.LoadScene(1);
        }
    }

}
