using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPaused;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        isPaused = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
