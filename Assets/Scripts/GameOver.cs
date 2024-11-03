using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Tilemap palletTileMap;
    public TileBase normalPallet;
    public PacStudentControl studentControl;
    public TimeController timeController;

    int PelletNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        CountNormalPellet();
    }

    // Update is called once per frame
    void Update()
    {
        GameOverIfAllNormalPelletGotEaten();
        GameOverIfPacStudentFuckedUp();
    }

    void CountNormalPellet ()
    {
        foreach (Vector3Int pos in palletTileMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = palletTileMap.GetTile(pos);
            if (tile == normalPallet) { PelletNumber++; }
        }
    }

    void GameOverIfAllNormalPelletGotEaten ()
    {
        int NumberOfEatenPellet = studentControl.getNumberOfEatenNormalPellet();

        if (NumberOfEatenPellet == PelletNumber)
        {
            studentControl.turnInvinsible();
            studentControl.StopMovement();
            EndTheGame();
        }
    }

    void GameOverIfPacStudentFuckedUp()
    {
        if (studentControl.getIsPlayerDead() && studentControl.getPlayerLives() == 0)
        {
            EndTheGame();
        }
    }

    void EndTheGame ()
    {
        timeController.StopTimer();
        Text GameOverText = gameObject.GetComponent<Text>();
        GameOverText.text = "Game Over"; 
        int currentBestScore = PlayerPrefs.GetInt("Score", 0);
        int score = studentControl.GetScore();
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        float timer = timeController.GetTimer();
        if (score > currentBestScore || (score == currentBestScore && timer < bestTime))
        {
            Debug.Log("UPDATE SCORE");
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.Save();
            Debug.Log("Check update score: " + PlayerPrefs.GetInt("Score", 0));
            PlayerPrefs.SetFloat("BestTime", timer);
            PlayerPrefs.Save();
            
        }
        Invoke("GoBackToStartScene", 3f);
    }

    void GoBackToStartScene ()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
