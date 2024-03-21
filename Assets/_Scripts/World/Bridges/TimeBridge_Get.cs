using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBridge_Get : TimeBridge_Base
{
    public string timeString;

    public int hours;

    public int minutes;

    float SunriseTime = 6f;
    float SunsetTime = 20f;
    float DayLightLength;
    float NightLength;

    float _CurrentTime;

    void Start()
   {
        DayLightLength = SunsetTime - SunriseTime;
        NightLength = SunriseTime + (TimeManager.Instance.DayLength - SunsetTime);
   }
    public override void OnTick(float CurrentTime)
    {
        hours = Mathf.FloorToInt(CurrentTime);

        float remainder = (CurrentTime - hours) * 60f; // remainder in minutes
        minutes = Mathf.FloorToInt(remainder);
        
        remainder = (remainder - minutes) * 60f; //remainder in seconds

        timeString = hours.ToString() + ":" + minutes.ToString("00") + ":" + remainder.ToString("00.0");
        _CurrentTime = CurrentTime;
    }

    // get time as string
    public string TimeToString()
    {
        return timeString;
    }

    // get time as float
    public float TimeToFloat()
    {
        return _CurrentTime;
    }

    // get hours as int
    public int Hours()
    {
        return hours;
    }

    // get minutes as int
    public int Minutes()
    {
        return minutes;
    }

    // get bool if before dawn
    public bool BeforeDawn()
    {
        return _CurrentTime < SunriseTime; // before dawn
        
    }

    // get bool if during day
    public bool DuringDay()
    {
        return _CurrentTime < SunsetTime;
    }

    // get bool if after sunset
    public bool AfterSunset()
    {
        return _CurrentTime > SunsetTime / NightLength;
    }

    
}
