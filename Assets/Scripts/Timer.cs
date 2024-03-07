using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private static float staticStartTime;
    private static float staticTimeElapsed;
    private static bool timerInitialized = false;

    public TextMeshProUGUI timerText;
    public float timeElapsed;
    private bool timerRunning = true;

    void Start()
    {
        if (!timerInitialized)
        {
            // This is the first instance of the timer, initialize it.
            staticStartTime = Time.time;
            timerInitialized = true;
        }

        // For new instances, use the static values to maintain continuity.
        timeElapsed = staticTimeElapsed;
    }

    public static float GetCurrentTime()
    {
        return staticTimeElapsed;
    }


    void Update()
    {
        if (!timerRunning) return;

        // Update using staticStartTime to ensure continuity across scenes.
        timeElapsed = Time.time - staticStartTime;
        staticTimeElapsed = timeElapsed; // Update the static variable for continuity.

        string minutes = ((int)timeElapsed / 60).ToString();
        string seconds = (timeElapsed % 60).ToString("f2");

        if (timerText != null)
            timerText.text = "Time: " + minutes + ":" + seconds;
    }

    public void ResetTimer()
    {
        staticStartTime = Time.time;
        // Ensure this affects all instances by resetting the static variables.
        timeElapsed = 0;
        staticTimeElapsed = 0;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    // Optionally, add methods to start or pause the timer as needed.
}
