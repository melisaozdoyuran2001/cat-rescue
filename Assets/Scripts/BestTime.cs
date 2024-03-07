using UnityEngine;
using TMPro;

public class BestTime : MonoBehaviour
{
    public TextMeshProUGUI currentTimerText;
    public float currentTime; 
    
    public TextMeshProUGUI bestTimerText;
    public Timer timer; 

    private float bestTime;

    void Start()
    {
        currentTime = timer.timeElapsed;
        UpdateBestTime(currentTime);
        
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

    public void UpdateBestTime(float currentTime)
    {
        // If the current time is better than the best time, update the best time.
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            // Save the new best time to PlayerPrefs.
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // Update the display texts.
        DisplayTimes(currentTime);
    }

    void DisplayTimes(float currentTime)
    {
        // Format and display the current time.
        string currentMinutes = ((int)currentTime / 60).ToString();
        string currentSeconds = (currentTime % 60).ToString("f2");
        currentTimerText.text = "Current Time: " + currentMinutes + ":" + currentSeconds;

        // Format and display the best time, if it exists.
        if (bestTime != float.MaxValue)
        {
            string bestMinutes = ((int)bestTime / 60).ToString();
            string bestSeconds = (bestTime % 60).ToString("f2");
            bestTimerText.text = "Best Time: " + bestMinutes + ":" + bestSeconds;
        }
        else
        {
            bestTimerText.text = "Best Time: N/A";
        }
    }
}
