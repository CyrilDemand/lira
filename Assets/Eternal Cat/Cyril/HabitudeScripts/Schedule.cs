using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Schedule
{
    public List<TimeSlot> dailySchedule;
}

[System.Serializable]
public class TimeSlot
{
    public string time;
    public string location;
    public string activity;
    public string dialogueKey;
}
