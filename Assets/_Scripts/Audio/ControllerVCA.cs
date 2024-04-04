using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerVCA : MonoBehaviour
{

    private FMOD.Studio.VCA controllerVCA;

    public string nameVCA;

    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        controllerVCA = FMODUnity.RuntimeManager.GetVCA("vca:/" + nameVCA);
        slider = GetComponent<Slider>();
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
