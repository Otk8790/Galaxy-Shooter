using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCoopMode = false;
    public bool gameOver = true;
    [SerializeField]
    private GameObject _player;
    public GameObject _coopPlayers;
    [SerializeField] 
    private GameObject _PauseMenuPanel;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    private Animator _pauseAnimator;
    public bool isPaused = false;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _uiManager.isTitleScreenActive = true;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isCoopMode == false)
                {
                    Instantiate(_player, new Vector3(0f,-3f,0f), Quaternion.identity);
                }
                else
                {
                  Instantiate(_coopPlayers, new Vector3(0f, -3f, 0f), Quaternion.identity);
                }
                gameOver = false;
                _uiManager.isTitleScreenActive = false;
                _uiManager.HideTitleScreen();
                _spawnManager.StartSpawnRoutine();
            }

            else if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene("Main_Menu");
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_uiManager.isTitleScreenActive == false)
            {
                _PauseMenuPanel.SetActive(true);
                _pauseAnimator.SetBool("isPaused",true);
                isPaused = true;
                Time.timeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ResumeGame()
    {
        _PauseMenuPanel.SetActive(false);
        isPaused = false;
        _pauseAnimator.SetBool("isPaused",false);
        Time.timeScale = 1f;
    }
}
