using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ScaryModeController : MonoBehaviour
{
    // Start is called before the first frame update
    
    private bool IsScaryMode = false;
    private bool IsRecovering = false;
    private float totalScaryDuration = 10.0f;
    private float recoveringDuration = 3.0f;
    private float scaryModeStartTime;

    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource scaryAudioSource;

    [SerializeField] private GameObject GhostScaryTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsScaryMode)
        {
            float scaryTime = Time.time - scaryModeStartTime;
            if (scaryTime <= totalScaryDuration)
            {
                updateGhostScaryTimer();
                if (totalScaryDuration - scaryTime <= recoveringDuration)
                {
                    IsRecovering = true;
                }
            }
            else
            {
                Debug.Log("Scary time over");
                TurnOffScaryMode();
            }
        } 
    }

    public void TurnOnScaryMode ()
    {
        IsScaryMode = true;
        scaryModeStartTime = Time.time;
        backgroundAudioSource.Stop();
        scaryAudioSource.Play();
        avtivateGhostScaryTimer(true);
    }

    public void TurnOffScaryMode ()
    {
        IsScaryMode = false;
        IsRecovering = false;
        scaryAudioSource.Stop();
        backgroundAudioSource.Play();
        avtivateGhostScaryTimer(false);
    }

    private void avtivateGhostScaryTimer(bool isActive)
    {
        GhostScaryTimer.SetActive(isActive);
    }

    private void updateGhostScaryTimer ()
    {
        float remainingTime = 10.0f - (Time.time - scaryModeStartTime);
        int remainingSecond = (int)remainingTime;
        int remainingMS = (int)((remainingTime - (float)remainingSecond) * 100);
        string remainingTimeString = remainingSecond.ToString("D2") + " : " + remainingMS.ToString("D2");
        Text ghostScaryTimerText = GhostScaryTimer.GetComponent<Text>();
        ghostScaryTimerText.text = remainingTimeString;
    }

    public bool GetScaryMode ()
    {
        return IsScaryMode;
    }

    public bool GetRecoveringMode ()
    {
        return IsRecovering;
    }
}
