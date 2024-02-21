using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
public class Timer : MonoBehaviour
{
    public  float startTime;
    public TextMeshProUGUI timerText; 
    float timeElapsed; 

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
      
        timeElapsed = Time.time - startTime;

        string minutes = ((int)timeElapsed / 60).ToString();
        string seconds = (timeElapsed % 60).ToString("f2"); 
       
        if(timerText != null)
            timerText.text = "Time: " + minutes + ":" + seconds;
    }

    public void ResetTimer(){
        startTime = timeElapsed;
    }
}
