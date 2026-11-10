
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] int targetScore = 8;

    [Header("Timer")]
    [SerializeField] bool useTimer = false;
    [SerializeField] float timeLimit = 60f;
    [SerializeField] TMP_Text timerText;

    [Header("Health System")]
    [SerializeField] Health player;

    [Header("UI")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text healthText;

    [SerializeField] GameObject winPanel;
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

        if (timerText != null)
        {
            timerText.text =
                $"Time: {Mathf.CeilToInt(timeRemaining)}";
        }
    }

    public void AddScore(int amount)
    {
        if (gameEnded)
            return;

        score += amount;

        UpdateUI();
    }

    public void ReachGoal()
    {
        if (gameEnded)
            return;

        if (score >= targetScore)
        {
            Win();
        }
        else
        {
            Debug.Log("Collect all coins first!");
        }
    }

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

    public void PlayerDied()
    {
        if (gameEnded)
            return;

        UpdateUI();
        Lose();
    }

    void Win()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void Lose()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (losePanel != null)
            losePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }
}
