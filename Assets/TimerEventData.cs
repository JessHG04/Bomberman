using UnityEngine;

public class TimerEventData
{
    public float totaltime;
    public int lifes;
    public GameObject bombPrefab;

    public TimerEventData(float totalTime, int lifeNumber, GameObject bombPrefab)
    {
        this.totaltime = totalTime;
        this.lifes = lifeNumber;
        this.bombPrefab = bombPrefab;
    }
}