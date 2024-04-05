using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerVCA : MonoBehaviour
{

    private FMOD.Studio.VCA controllerVCA;

    public string nameVCA;

    private Slider slider;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        string path = "vca:/" + nameVCA;
        controllerVCA = FMODUnity.RuntimeManager.GetVCA(path);
        slider = GetComponent<Slider>();
        controllerVCA.getVolume(out value);
        slider.value = value;
    }

   

    public void SetVolume(float volume)
    {
        controllerVCA.setVolume(volume);
    }
}
