using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;	//Static instance of GameManager which allows it to be accessed by any other script.

    public float levelStartDelay = 2f;  //Time to wait before starting a new level, in seconds.

    public float roundTime = 120f; // Round time, in seconds

    private BoardManager boardScript;  // Store a reference to our BoardManager which will set up the level.
    private int level = 1;  //  Current level number.

    private bool paused;

    private float timeLeft = 0;

    private int targetScore = 0;
    private int levelScore = 0;

    [SerializeField]
    private TMP_Text levelText;	//Text to display current level number.
    [SerializeField]
    private TMP_Text levelTargetText; //Text to display current level target score.
    [SerializeField]
    private TMP_Text levelScoreText;	//Text to display current score.
    [SerializeField]
    private RectTransform levelScoreBar; // Bar to track level score
    [SerializeField]
    private TMP_Text levelTimerText;	//Text to display current score.

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    // This is called only once, and the paramter tell it to be called only after the scene was loaded
    // (otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;

        instance.InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
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
    }

    //GameOver is called when the timer reaches 0
    public void GameOver()
    {
        // Show game over
        StartCoroutine(ShowLevelText("Time's up!\nGame Over", 5.0f));

        //Disable this GameManager.
        enabled = false;
    }
}
