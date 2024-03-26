using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour //IDataPersistance
{
    // serialized fields for easier editing via inspector
    [Tooltip("Length of a day in hours")]
    [SerializeField] float _DayLength = 24f;

    [Tooltip("Starting time in hours relative to midnight")]
    [SerializeField] float _StartingTime = 9f;

    [Tooltip("Controls how fast time advances (eg. 60 = 1 irl second = 1 game minute)")]

    [SerializeField] float _TimeFactor = 60f;

    [Tooltip("Time bridges to send data to")]
    [SerializeField] List<TimeBridge_Base> Bridges;

    private readonly DateTime _startDate = new DateTime(2024, 1, 1);
    public float DayLength => _DayLength;

    public float DeltaTime => Time.deltaTime * _TimeFactor;

    //private float _elapsedTime;

    //public DateTime CurrentDate { get; private set; }


    public static TimeManager Instance { get; private set; } = null;

    // this variable is public for testing but should probably be private in the future
    public float CurrentTime = 0f;

    void Awake()
    {
        if (Instance != null)
        {
            // lets make sure that we are not running multiple TimeManagers
            Debug.LogError("Duplicate TimeManager found on " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentTime = _StartingTime;

    }

    // Update is called once per frame
    void Update()
    {
        
        CurrentTime = (DayLength + CurrentTime + Time.deltaTime * _TimeFactor / 3600f) % DayLength;
        //_elapsedTime += (DeltaTime / DayLength);
        //var elapsedDays = _elapsedTime / _TimeFactor;
        //var elapsedTimeSpan = TimeSpan.FromDays(elapsedDays);
        // CurrentDate = _startDate.Add(elapsedTimeSpan);
        
        foreach (var bridge in Bridges)
        {
            bridge.OnTick(CurrentTime);
        }
    }
/*
    public void LoadData(GameData gameData)
    {
        CurrentTime = gameData.time;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.time = CurrentTime;
    }
    */
}
