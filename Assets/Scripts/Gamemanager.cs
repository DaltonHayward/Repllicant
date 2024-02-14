using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gamemanager : MonoBehaviour
{

    public static Gamemanager instance;
    private void Start()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    public  static void jumpScene(string name)
    {
        SceneManager.LoadScene(name);
    }

 
}
