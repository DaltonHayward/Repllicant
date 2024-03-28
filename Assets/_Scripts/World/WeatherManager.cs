using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;

public class WeatherManager : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float RainIntensity;
    //[SerializeField, Range(0f, 1f)] float SnowIntensity;
    [SerializeField, Range(0f, 1f)] float FogIntensity;
    //[SerializeField, Range(0f, 1f)] float HailIntensity;

    [SerializeField] float MinFogAttenuationDistance = 10f;
    [SerializeField] float MaxFogAttenuationDistance = 50f;
    [SerializeField] VisualEffect RainVFX;
    //[SerializeField] VisualEffect SnowVFX;
    //[SerializeField] VisualEffect HailVFX;
    [SerializeField] Volume FogVolume;
    float PreviousRainIntensity;
    //float PreviousSnowIntensity;
    //float PreviousHailIntensity;
    float PreviousFogIntensity;
    //Fog CachedFogComponent;

    // Start is called before the first frame update
    void Start()
    {
        RainVFX.SetFloat("Intensity", RainIntensity);
        //SnowVFX.SetFloat("Intensity", SnowIntensity);
/*
        FogVolume.weight = FogIntensity;

        //FogVolume.profile.TryGet<Fog>(out CachedFogComponent);

        if (CachedFogComponent != null)
        {
            CachedFogComponent.meanFreePath.Override(Mathf.Lerp(MaxFogAttenuationDistance,
                                                               MinFogAttenuationDistance,
                                                               FogIntensity));
        }*/
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
        }
         if (FogIntensity != PreviousFogIntensity)
        {
            PreviousFogIntensity = FogIntensity;
            FogVolume.weight = FogIntensity;

            if (CachedFogComponent != null)
                CachedFogComponent.meanFreePath.value = Mathf.Lerp(MaxFogAttenuationDistance,
                                                                   MinFogAttenuationDistance,
                                                                   FogIntensity);
        }*/
    }
}
