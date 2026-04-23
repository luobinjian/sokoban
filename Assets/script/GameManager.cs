using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int finishBox = 0;
    public int totalBox = 0;
    public GameObject WinPannel;

    public string SceneName;

    void Start()
    {
        WinPannel = GameObject.FindGameObjectWithTag("WinPannel");
        if(WinPannel != null)
        {
            WinPannel.SetActive(false);
        }
    }
    private void Update()
    {
        if(finishBox == totalBox)
        {
            if(WinPannel != null)
            {
                WinPannel.SetActive(true);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}