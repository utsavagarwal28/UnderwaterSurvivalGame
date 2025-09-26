using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimeStamp
{
    public int day;
    public int hour;
    public int minute;

    // class constructor - sets up the class
    public GameTimeStamp(int day, int hour, int minute)
    {
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public GameTimeStamp(GameTimeStamp timeStamp)
    {
        this.day = timeStamp.day;
        this.hour = timeStamp.hour;
        this.minute = timeStamp.minute;
    }

    public void UpdateClock()
    {
        // 1 in-game minute = 1 real-time Second
        minute++;

        // 60 minutes in an hour
        // 1 in-game hour = 1 realtime Minute or 60 realtime Seconds
        if (minute >= 60)
        {
            //reset minutes
            minute = 0;
            hour++;
        }

        // 20 hours in 1 day
        if (hour >= 20)
        {
            // reset hours
            hour = 0;
            day++;
        }
    }

    // convert hours to minutes
    public static int HoursToMinutes(int hour)
    {
        // 60 minute = 1 hour
        return hour * 60;
    }

    // returns the current time stamp in minutes
    public static int TimeStampInMinutes(GameTimeStamp timeStamp)
    {
        return (HoursToMinutes(timeStamp.day * 20) + HoursToMinutes(timeStamp.day) + timeStamp.minute);
    }

}
