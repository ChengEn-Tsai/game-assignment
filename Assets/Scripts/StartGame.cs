using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public PacStudentControl pacStudentControl;
    public AudioSource backgroundAudioSource;
    public TimeController timeController;
    public GhostController[] ghostControllers;

    private bool isGameStarted = false;
    private float countFrom = 3.5f;
    private float startCountingTime;
    private Text countdownText;
    // Start is called before the first frame update
    void Start()
    {
        startCountingTime = Time.time;
        countdownText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if  (!isGameStarted)
        {
            float count = countFrom - (Time.time - startCountingTime);
            if (count >= 0)
            {
                int intCount = (int)(count);
                if (intCount != 0)
                {
                    countdownText.text = intCount.ToString();
                }
                else
                {
                    countdownText.text = "GO!";
                }
                
            }
            else
            {
                isGameStarted = true;
                backgroundAudioSource.Play();
                pacStudentControl.enabled = true;
                timeController.enabled = true;
                foreach (GhostController controller in ghostControllers)
                {
                    controller.enabled = true;
                }
                gameObject.SetActive(false);
            }
        }
        
    }

    public bool getIsGameStarted ()
    {
        return isGameStarted;
    }
}
