
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] int targetScore = 8;
    [SerializeField] int lives = 3;

    [Header("Timer")]
    [SerializeField] bool useTimer = true;
    [SerializeField] float timeLimit = 60f;
    [SerializeField] TMP_Text timerText;

    [Header("Health System")]
    [SerializeField] bool useHealthSystem = false;
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] float respawnDelay = 0.6f;
    [SerializeField] float spawnProtection = 1.5f;

    [Header("UI")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text healthText;

    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    int score;
    float timeRemaining;
    bool gameEnded;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        timeRemaining = timeLimit;

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
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
    }

    public void AddScore(int amount)
    {
        if (gameEnded)
            return;

        score += amount;
        UpdateUI();

        if (score >= targetScore)
            Win();
    }

    public void ReachGoal()
    {
        if (score >= targetScore)
            Win();
    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score} / {targetScore}";

        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";

        if (livesText != null)
            livesText.text = $"Lives: {lives}";

        if (healthText != null && player != null)
            healthText.text = $"Health: {Mathf.Max(player.Current, 0)} / {player.Max}";
    }

    public void PlayerDied()
    {
        if (!useHealthSystem || gameEnded)
            return;

        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            Lose();
        }
        else
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (player == null || respawnPoint == null)
            yield break;

        Rigidbody body = player.GetComponent<Rigidbody>();

        body.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;

        player.Restore(spawnProtection);
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
