using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;

    [Header("UI References")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI messageText;     // 👈 Drop your text object here in Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        // Start countdown to hide text
        if (messageText != null)
            StartCoroutine(HideTextAfterDelay(7f));
    }

    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.gameObject.SetActive(false); // 👈 Hides the text object
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {amount} damage. HP = {currentHealth}");

        UpdateUI();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }

    void Die()
    {
        Debug.Log("💀 Player died!");
        SceneManager.LoadScene("SampleScene");
    }
}
