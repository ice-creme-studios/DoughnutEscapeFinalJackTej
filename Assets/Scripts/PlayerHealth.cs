using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;

    [Header("UI References")]
    public Slider healthBar;        // optional
    public TextMeshProUGUI healthText; // optional

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {amount} damage. HP = {currentHealth}/{maxHealth}");

        UpdateUI();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }

    void Die()
    {
        Debug.Log("💀 Player died!");
        SceneManager.LoadScene("SampleScene"); // Replace with your scene name
        // Disable movement, show game over UI, etc.
    }
}
