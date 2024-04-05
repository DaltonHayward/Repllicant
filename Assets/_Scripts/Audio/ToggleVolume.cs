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
        controllerVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + nameVCA);
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float volume)
    {
        controllerVCA.setVolume(volume);
    }
}
