using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);
        float time = PlayerPrefs.GetFloat("BestTime", 0);
        Debug.Log("time:" + time);
        scoreText.text = score.ToString();
        timeText.text = formatTime(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string formatTime (float timer)
    {
        int totalSeconds = (int)timer;
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);
        int ms = (int)((timer - (float)totalSeconds) * 100);

        return minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + ms.ToString("D2");
    }
}
