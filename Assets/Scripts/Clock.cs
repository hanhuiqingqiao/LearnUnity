using System;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    Transform hoursPivot;
    [SerializeField]
    Transform minutesPivot;
    [SerializeField]
    Transform secondsPivot;

    public bool isTimeSpan = false;

    const float hoursToDegrees = -30;
    const float minutesToDegrees = -6f;
    const float secondsToDegrees = -6f;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimeSpan)
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            hoursPivot.localRotation = Quaternion.Euler(0, 0, hoursToDegrees * (float)time.TotalHours);
            minutesPivot.localRotation = Quaternion.Euler(0, 0, minutesToDegrees * (float)time.TotalMinutes);
            secondsPivot.localRotation = Quaternion.Euler(0, 0, secondsToDegrees * (float)time.TotalSeconds);
        }
        else
        {
            DateTime time = DateTime.Now;
            hoursPivot.localRotation = Quaternion.Euler(0, 0, hoursToDegrees * time.Hour);
            minutesPivot.localRotation = Quaternion.Euler(0, 0, minutesToDegrees * time.Minute);
            secondsPivot.localRotation = Quaternion.Euler(0, 0, secondsToDegrees * time.Second);
        }
    }
}
