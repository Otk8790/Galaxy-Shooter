using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Sprite[] lives;
    public Image livesImageDisplay;
    public GameObject titleScreen;
    public Text scoreText, bestText, nextStage, ultimateStage, ContinueText;
    public int score, bestScore;
    private SpawnManager _spawnManager;
    public int setScore = 20;
    public bool isTitleScreenActive = false;
    public AudioClip newTrack, newTrack2, newTrackStart;
    private AudioManager theAM;
    private GameManager _gameManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        theAM = FindObjectOfType<AudioManager>();
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
       bestText.text = "Best: " + bestScore;
       nextStage.gameObject.SetActive(false);
       ultimateStage.gameObject.SetActive(false);
    }

    public void UpdateLives(int currentLives)
    {
        Debug.Log("Player lives: " + currentLives);
        livesImageDisplay.sprite = lives[currentLives];
    }

    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
        if (score == 100)
        {
         Debug.Log("Next Stage");
         nextStage.gameObject.SetActive(true);
         _spawnManager.StartSpawnRoutine2();
         StartCoroutine(StageText());
         if (newTrack != null)
             theAM.ChangeBGM(newTrack);
        }
        if (score == 400)
        {
            Debug.Log("Ultimate Stage");
            ultimateStage.gameObject.SetActive(true);
            _spawnManager.StartSpawnRoutine3();
            StartCoroutine(StageText());
            if (newTrack != null)
                theAM.ChangeBGM(newTrack2);
        }
    }

    public void CheckForBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            bestText.text = "Best: " + bestScore;
        }
    }

    public void ShowTitleScreen()
    {
        titleScreen.gameObject.SetActive(true);
        ContinueText.gameObject.SetActive(true);
        isTitleScreenActive = true;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if (newTrack != null)
            theAM.ChangeBGM(newTrackStart);
    }
    public void HideTitleScreen()
    {
        titleScreen.gameObject.SetActive(false);
        ContinueText.gameObject.SetActive(false);
        score = 0;
        scoreText.text = "Score: 0";
        isTitleScreenActive = false;
    }
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void ResumePlay()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.ResumeGame();
    }

    public IEnumerator StageText()
    {
        yield return new WaitForSeconds(5f);
        nextStage.gameObject.SetActive(false);
        ultimateStage.gameObject.SetActive(false);
    }
}
