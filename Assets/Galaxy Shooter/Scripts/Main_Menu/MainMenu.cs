using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        QuitGame();
    }

    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("Single_Player");
    }
    
    public void LoadCoOpGame()
    {
        SceneManager.LoadScene("Cooperative_Mode");
    }

    public void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Exit Game");
        }
    }
}
