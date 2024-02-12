using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
public class Timer : MonoBehaviour
{
    private float startTime;
    public TextMeshProUGUI timerText; 

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
      
        float timeElapsed = Time.time - startTime;

        string minutes = ((int)timeElapsed / 60).ToString();
        string seconds = (timeElapsed % 60).ToString("f2"); 
       
        if(timerText != null)
            timerText.text = "Time: " + minutes + ":" + seconds;
    }
}
