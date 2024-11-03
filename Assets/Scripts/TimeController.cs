using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public GameObject gameTimer;
    //public PacStudentControl studentControl;

    private float startTime = 0;
    private float timer;
    private bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        startCounting();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning) { SetTimer(); }
    }

    void startCounting ()
    {
        startTime = Time.time;
        isRunning = true;
    }

    void SetTimer ()
    {
        timer = Time.time - startTime;
        int totalSeconds = (int)timer;
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);
        int ms = (int)((timer - (float)totalSeconds) * 100);

        string timerText = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + ms.ToString("D2");
        Text gameTimerText = gameTimer.GetComponent<Text>();
        gameTimerText.text = timerText;
    }

    public float GetTimer ()
    {
        return timer;
    }

    public float StopTimer ()
    {
        isRunning = false;
        return timer;
    }
}
