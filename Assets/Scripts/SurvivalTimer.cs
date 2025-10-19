using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SurvivalTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float survivalTime = 180f; // 3 minutes
    private float timeRemaining;

    [Header("UI References")]
    public TextMeshProUGUI timerText; // 👈 drag a TMP text here in the inspector
    public TextMeshProUGUI winText;   // 👈 optional “You Survived!” text

    private bool gameEnded = false;

    void Start()
    {
        timeRemaining = survivalTime;

        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameEnded)
            return;

        // Countdown
        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(0, timeRemaining);

        UpdateTimerUI();

        // Check if finished
        if (timeRemaining <= 0)
        {
            OnSurvived();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    void OnSurvived()
    {
        gameEnded = true;
        Debug.Log("🎉 Player survived!");

        if (winText != null)
        {
            winText.text = "YOU SURVIVED! can we have an 8? :)";
            winText.gameObject.SetActive(true);
        }

        // Optional: Load next scene or show menu after a delay
        // StartCoroutine(NextSceneAfterDelay(3f));
    }

    IEnumerator NextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("NextSceneName"); // change to your next scene
    }
}
