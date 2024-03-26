using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherManager : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float RainIntensity;
    //[SerializeField, Range(0f, 1f)] float SnowIntensity;
    [SerializeField, Range(0f, 1f)] float FogIntensity;
    //[SerializeField, Range(0f, 1f)] float HailIntensity;
    [SerializeField] VisualEffect RainVFX;
    //[SerializeField] VisualEffect SnowVFX;
    //[SerializeField] VisualEffect HailVFX;
    float PreviousRainIntensity;
    //float PreviousSnowIntensity;
    //float PreviousHailIntensity;

    // Start is called before the first frame update
    void Start()
    {
        RainVFX.SetFloat("Intensity", RainIntensity);
        //SnowVFX.SetFloat("Intensity", SnowIntensity);
    }

    // Update is called once per frame
    void Update()
    {
        if (RainIntensity != PreviousRainIntensity)
        {
            PreviousRainIntensity = RainIntensity;
            RainVFX.SetFloat("Intensity", RainIntensity);
        }
        /*if (SnowIntensity != PreviousSnowIntensity)
        {
            PreviousSnowIntensity = SnowIntensity;
            SnowVFX.SetFloat("Intensity", SnowIntensity);
        }
        if (HailIntensity != PreviousHailIntensity)
        {
            PreviousHailIntensity = HailIntensity;
            HailVFX.SetFloat("Intensity", HailIntensity);
        }*/
    }
}
