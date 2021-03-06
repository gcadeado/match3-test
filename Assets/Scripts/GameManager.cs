﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    private BoardManager boardScript;  // Store a reference to our BoardManager which will set up the level.

    [Header("Scriptable Objects Architecture")]
    [SerializeField]
    private IntVariable levelObject = null;
    [SerializeField]
    private IntVariable scoreObject = null;
    [SerializeField]
    private IntVariable targetScoreObject = null;

    [Header("Game settings")]
    [SerializeField]
    private string[] passedPhrases = { "Passed" };  // Phrases to show when round is done

    public float levelStartDelay = 2f;  //Time to wait before starting a new level, in seconds.

    public float roundTime = 120f; // Round time, in seconds

    [Header("Game info")]
    [SerializeField]
    [ReadOnly]
    private bool paused;
    private float timeLeft = 0;

    private TMP_Text levelText = null;	//Text to display current level number.
    private TMP_Text levelScoreText = null;	//Text to display current score.
    private RectTransform levelScoreBar = null; // Bar to track level score
    private TMP_Text levelTimerText = null; //Text to display current score.

    public int levelScore
    {
        get
        {
            return scoreObject.Value;
        }
        set
        {
            scoreObject.Value = value;
        }
    }

    public int level
    {
        get
        {
            return levelObject.Value;
        }
        set
        {
            levelObject.Value = value;
        }
    }

    public int targetScore
    {
        get
        {
            return targetScoreObject.Value;
        }
        set
        {
            targetScoreObject.Value = value;
        }
    }

    void Awake()
    {
        // Increase current level
        level = level + 1;

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Get references to our UI text component by finding it by name and calling GetComponent.
        levelText = GameObject.Find("LevelText").GetComponent<TMP_Text>();
        levelScoreText = GameObject.Find("LevelScore").GetComponent<TMP_Text>();
        levelTimerText = GameObject.Find("LevelTimer").GetComponent<TMP_Text>();
        levelScoreBar = GameObject.Find("ScoreBarImage").GetComponent<RectTransform>();

        // Show current round
        ShowLevelText("Round " + level, 2.0f);

        // Reset score
        levelScore = 0;

        // Set level score
        levelScoreText.text = levelScore.ToString();

        // Set level score bar to 0
        levelScoreBar.localScale = new Vector3(0f, 1f, 1f);

        // Set level target score using a simple formula
        targetScore = (int)(level * 1000f);

        //Call the Init function of the BoardManager script.
        boardScript.Init();

        // Reset timer
        timeLeft = roundTime;

        //Reset the text of levelTimer
        levelTimerText.text = timeLeft.ToString();

    }

    private void ShowLevelText(string text, float duration, bool pause = true)
    {
        if (pause)
        {
            paused = true;
            boardScript.canPlay = false;
        }
        //Set the text of levelText to the string "Level" and append the current level number.
        levelText.text = text;

        // TODO create a nice animation to text level
        levelText.enabled = true;

        // yield return new WaitForSecondsRealtime(duration);
        Invoke("HideLevelText", duration);

    }

    private void HideLevelText()
    {
        levelText.enabled = false;

        paused = false;
        boardScript.canPlay = true;
    }

    //Update is called every frame.
    void Update()
    {
        if (paused)
        {
            return;
        }

        // Update time left
        timeLeft -= Time.deltaTime;
        levelTimerText.text = ((int)timeLeft).ToString();

        if (timeLeft < 0)
        {
            GameOver();
        }

        if (levelScore >= targetScore)
        {
            boardScript.canPlay = false;
            string passedText = passedPhrases[Random.Range(0, passedPhrases.Length)];
            ShowLevelText(passedText, levelStartDelay);
            Invoke("RestartScene", levelStartDelay);
        }
    }

    private void RestartScene()
    {
        // Load the last scene loaded, in this case Playground, the only scene in the game. 
        // And we load it in "Single" mode so it replace the existing one
        // and not load all the scene object in the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    //GameOver is called when the timer reaches 0
    public void GameOver()
    {
        ShowLevelText("Time's up!\nGame Over", 5.0f); // Show game over
        level = 0; // Reset level variable
        Invoke("BackToMenu", 2.0f); // Go back to menu
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
