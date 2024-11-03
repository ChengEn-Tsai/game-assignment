using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagers : MonoBehaviour
{
    public void ExitLevel()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void GoToLevel1()
    {
        Debug.Log("GOGOGOG LEVEL1");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
