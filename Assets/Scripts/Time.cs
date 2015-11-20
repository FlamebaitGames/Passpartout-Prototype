using UnityEngine;
using System.Collections;

public class Time {
    private float dayInSeconds      = 120.0f;
    private float currentTime       = 0.0f;
    private float secondsPerHour    = 5.0f;

    private int d;
    private int m;
    private int y;
    
    public Time(float dayInSeconds, int day, int month, int year)
    {
        this.dayInSeconds = dayInSeconds;
        d = day;
        m = month;
        y = year;
    }

    public string ToString()
    {
        int hour = (int)(currentTime / secondsPerHour) % 12;
        int minute = (int)(currentTime / (secondsPerHour / 60.0f));
        string timeSuffix = (currentTime < (dayInSeconds / 2.0f)) ? "PM" : "AM";
        return hour.ToString() + ":" + minute.ToString() + " " + timeSuffix;
    }
}
