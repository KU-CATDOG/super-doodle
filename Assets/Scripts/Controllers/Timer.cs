using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro time;

    private float StartTime;
    private static float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartTime = Time.time;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCheck();
    }

    private void TimeCheck()
    {
        timer = Time.time - StartTime;
        time.text = "Time: " + timer.ToString("N2");
    }

    public static float CurrentTime()
    {
        return timer;
    }
}