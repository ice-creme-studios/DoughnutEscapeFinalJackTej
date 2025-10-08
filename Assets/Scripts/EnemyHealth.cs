using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyHealth : MonoBehaviour

{
    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;

    [Header("Death Settings")]
    public GameObject deathEffect; // optional VFX prefab
    public float destroyDelay = 3f; // allow death animation to play

    private bool isDead = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(); 
        currentHealth = maxHealth;
    }


    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRadius)
        {
            Attack();
        }
        else if (distance <= detectionRadius)
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
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
            // play hurt/take damage animation
            if (animator) animator.SetTrigger("takeDamage");
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;

        // disable colliders
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        // play death animation
        if (animator)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", true);
        }

        // spawn optional VFX
        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // destroy after animation
        Destroy(gameObject, destroyDelay);
    }
}
