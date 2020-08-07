using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;	//Static instance of GameManager which allows it to be accessed by any other script.
    private BoardManager boardScript;  // Store a reference to our BoardManager which will set up the level.

    [Header("Scriptable Objects Architecture")]
    [SerializeField]
    private IntVariable levelObject = null;
    [SerializeField]
    private IntVariable scoreObject = null;


    [Header("Gameplay times")]
    public float levelStartDelay = 2f;  //Time to wait before starting a new level, in seconds.

    public float roundTime = 120f; // Round time, in seconds

    [Header("Game info")]
    [SerializeField]
    [ReadOnly]
    private bool paused;
    [SerializeField]
    [ReadOnly]
    private int targetScore = 0;
    private float timeLeft = 0;

    private TMP_Text levelText = null;	//Text to display current level number.
    private TMP_Text levelTargetText = null; //Text to display current level target score.
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

    void Awake()
    {
        // Enforces singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {

            Destroy(gameObject);
        }

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
        levelTargetText = GameObject.Find("LevelTarget").GetComponent<TMP_Text>();
        levelTimerText = GameObject.Find("LevelTimer").GetComponent<TMP_Text>();
        levelScoreBar = GameObject.Find("ScoreBarImage").GetComponent<RectTransform>();

        // Show current round
        StartCoroutine(ShowLevelText("Round " + level, 2.0f));

        // Reset score
        levelScore = 0;

        // Set level score
        levelScoreText.text = levelScore.ToString();

        // Set level score bar to 0
        levelScoreBar.localScale = new Vector3(0f, 1f, 1f);

        // Set level target score using a simple formula
        targetScore = (int)(level * 1000f);

        //Set the text of levelTarget
        levelTargetText.text = targetScore.ToString();

        //Call the Init function of the BoardManager script.
        boardScript.Init();

        // Reset timer
        timeLeft = roundTime;

        //Reset the text of levelTimer
        levelTimerText.text = timeLeft.ToString();

    }

    IEnumerator ShowLevelText(string text, float duration, bool pause = true)
    {
        if (pause)
        {
            paused = true;
        }
        //Set the text of levelText to the string "Level" and append the current level number.
        levelText.text = text;

        // TODO create a nice animation to text level
        levelText.enabled = true;

        yield return new WaitForSeconds(duration);

        levelText.enabled = false;

        paused = false;
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
            StartCoroutine(ShowLevelText("OK", levelStartDelay));
            Invoke("RestartScene", levelStartDelay);
        }
    }

    private void RestartScene()
    {
        // Load the last scene loaded, in this case Main, the only scene in the game. 
        // And we load it in "Single" mode so it replace the existing one
        // and not load all the scene object in the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    //GameOver is called when the timer reaches 0
    public void GameOver()
    {
        // Show game over
        StartCoroutine(ShowLevelText("Time's up!\nGame Over", 5.0f));

        // Disable this GameManager.
        enabled = false;
    }
}
