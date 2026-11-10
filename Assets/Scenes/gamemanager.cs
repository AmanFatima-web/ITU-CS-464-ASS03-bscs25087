
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] int targetScore = 8;
    [SerializeField] string levelName = "Level 1";

    [Header("Timer")]
    [SerializeField] bool useTimer = false;
    [SerializeField] float timeLimit = 60f;
    [SerializeField] TMP_Text timerText;

    [Header("Health System")]
    [SerializeField] Health player;

    [Header("UI")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text healthText;

    [Header("Win Panel")]
    [SerializeField] GameObject winPanel;
    [SerializeField] TMP_Text levelCompleteText;

    [Header("Lose Panel")]
    [SerializeField] GameObject losePanel;

    int score;
    float timeRemaining;
    bool gameEnded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        timeRemaining = timeLimit;

        if (player == null)
        {
            player = FindFirstObjectByType<Health>();
        }

        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        if (levelCompleteText != null)
        {
            levelCompleteText.text =
                levelName + " Completed!";
        }

        UpdateUI();
    }

    void Update()
    {
        if (gameEnded || !useTimer)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            UpdateUI();
            Lose();
            return;
        }

        UpdateUI();
    }

    // COIN COLLECTION
    public void AddScore(int amount)
    {
        if (gameEnded)
            return;

        score += amount;

        UpdateUI();

        if (score >= targetScore)
        {
            Win();
        }
    }

    // UPDATE UI
    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text =
                $"Score: {score} / {targetScore}";
        }

        if (timerText != null)
        {
            timerText.text =
                $"Time: {Mathf.CeilToInt(timeRemaining)}";
        }

        if (healthText != null && player != null)
        {
            healthText.text =
                $"Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
        }
    }

    // HIDE GAME UI
    void HideGameUI()
    {
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        if (healthText != null)
            healthText.gameObject.SetActive(false);

        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    // PLAYER DIED
    public void PlayerDied()
    {
        if (gameEnded)
            return;

        UpdateUI();
        Lose();
    }

    // WIN PANEL
    void Win()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        HideGameUI();

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Win Panel is not assigned in Inspector!");
        }

        if (levelCompleteText != null)
        {
            levelCompleteText.text =
                levelName + " Completed!";
        }

        Time.timeScale = 0f;
    }

    // LOSE PANEL
    void Lose()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        HideGameUI();

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Lose Panel is not assigned in Inspector!");
        }

        Time.timeScale = 0f;
    }

    // RESTART
    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // NEXT LEVEL
    public void NextLevel()
    {
        Time.timeScale = 1f;

        int nextSceneIndex =
            SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("All Levels Completed!");
        }
    }
}
