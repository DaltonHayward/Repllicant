using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVolume : MonoBehaviour
{

    private FMOD.Studio.VCA controllerVCA;

    public string nameVCA;

    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        controllerVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        //vca:/Master
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(bool value)
    {
        float volume = 1f;
        if (value == true)
        {
            volume = 0f;
        }
        if (value == false)
        {
            volume = 1f;
        }

        controllerVCA.setVolume(volume);
    }
}
