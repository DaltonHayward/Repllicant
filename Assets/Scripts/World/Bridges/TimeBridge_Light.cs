using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bridge class to control direction light object (day/night cycle visual element)
/// </summary>
// 
public class TimeBridge_Light : TimeBridge_Base
{
    [SerializeField] Light LinkedLight;
    [SerializeField] float SunriseTime = 6f;
    [SerializeField] float SunsetTime = 20f;

    [SerializeField] Gradient LightColour_Day;
    [SerializeField] Gradient LightColour_Night;
    [SerializeField] AnimationCurve LightIntensity_Day;
    [SerializeField] AnimationCurve LightIntensity_Night;

    float DayLightLength;
    float NightLength;

   void Start()
   {
        DayLightLength = SunsetTime - SunriseTime;
        NightLength = SunriseTime + (TimeManager.Instance.DayLength - SunsetTime);
   }
    public override void OnTick(float CurrentTime)
    { 
        float progress = -1f;
        bool isNight = true;

        if(CurrentTime < SunriseTime) // before dawn
        {
            progress = (CurrentTime + (TimeManager.Instance.DayLength - SunsetTime)) / NightLength;
        }
        else if (CurrentTime < SunsetTime) // during the day
        {   
            progress = (CurrentTime - SunriseTime) / DayLightLength;
            isNight = false;
        }
        else // after sunset
        {
            progress = (CurrentTime - SunsetTime) / NightLength ;
        }
        LinkedLight.intensity = isNight ? LightIntensity_Night.Evaluate(progress) : LightIntensity_Day.Evaluate(progress);
        LinkedLight.color = isNight ? LightColour_Night.Evaluate(progress) : LightColour_Day.Evaluate(progress);

        LinkedLight.transform.rotation = Quaternion.Euler(progress * 180f, 0f, 0f);
    }

}
