
using System.Collections;
using UnityEngine;
using TMPro;

public class PowerUpMessage : MonoBehaviour
{
    public static PowerUpMessage Instance;

    [SerializeField] GameObject messageObject;
    [SerializeField] TMP_Text messageText;

    Coroutine messageRoutine;

    void Awake()
    {
        Instance = this;

        if (messageObject != null)
            messageObject.SetActive(false);
    }

    public void ShowMessage(string message, float duration)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(
            ShowRoutine(message, duration)
        );
    }

    IEnumerator ShowRoutine(string message, float duration)
    {
        messageObject.SetActive(true);

        float remaining = duration;

        while (remaining > 0f)
        {
            messageText.text =
                message + "\nTime Remaining: " +
                Mathf.CeilToInt(remaining) + "s";

            yield return null;

            remaining -= Time.deltaTime;
        }

        messageText.text = "SPEED BOOST FINISHED!";

        yield return new WaitForSeconds(1f);

        messageObject.SetActive(false);
        messageRoutine = null;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
