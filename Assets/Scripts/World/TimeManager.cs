using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Serialized Fields
    // serialized fields for easier editing via inspector
    [Tooltip("Length of a day in hours")]
    [SerializeField] float DayLength = 24f;

    [Tooltip("Starting time in hours relative to midnight")]
    [SerializeField] float StartingTime = 0f;

    [Tooltip("Controls how fast time advances (eg. 60 = 1 irl second = 1 game minute)")]

    [SerializeField] float TimeFactor = 60f;

    [Tooltip("Time bridges to send data to")]
    [SerializeField] List<TimeBridge_Base> Bridges;
    #endregion
    public static TimeManager Instance { get; private set;} = null;

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

        CurrentTime = StartingTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime = (DayLength + CurrentTime + Time.deltaTime * TimeFactor / 3600f) % DayLength;
        foreach (var bridge in Bridges)
        {
            bridge.OnTick(CurrentTime);
        }
    }
}
