using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// bridge class for UI clock
public class TimeBridge_Clock : TimeBridge_Base
{
    [SerializeField] TextMeshProUGUI TimeDisplay;
    public override void OnTick(float CurrentTime)
    {
        int hours = Mathf.FloorToInt(CurrentTime);

        float remainder = (CurrentTime - hours) * 60f; // remainder in minutes
        int minutes = Mathf.FloorToInt(remainder);
        
        remainder = (remainder - minutes) * 60f; //remainder in seconds

        TimeDisplay.text = hours.ToString() + ":" + minutes.ToString("00") + ":" + remainder.ToString("00.0");
    }

}
