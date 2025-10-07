using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    float currentHealth;
    public GameObject deathEffect; // optional VFX prefab
    public float destroyDelay = 0f; // set >0 if you have death animation

    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            // optional: play hurt animation or sound
            if (animator) animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        isDead = true;

        // play death animation if present
        if (animator)
        {
            animator.SetTrigger("Die");
        }

        // spawn death effect
        if (deathEffect)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // disable collider and any movement scripts so enemy stops interacting
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        // destroy object after delay (gives time for death animation)
        Destroy(gameObject, destroyDelay);
    }
}
