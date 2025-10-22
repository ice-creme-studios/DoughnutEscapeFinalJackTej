using UnityEngine;
using UnityEngine.AI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public float attackCooldown = 1.5f;

    [Header("Death Settings")]
    public GameObject deathEffect;
    public float destroyDelay = 3f;

    private bool isDead = false;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;

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

        if (!isAttacking)
            agent.SetDestination(player.position);

        FacePlayer();

        if (distance <= attackRadius && Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (distance <= detectionRadius && !isAttacking)
        {
            Chase();
        }
        else if (!isAttacking)
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
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(1.0f);

        animator.SetBool("isAttacking", false);
        isAttacking = false;
        agent.isStopped = false;
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
        }
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0f)
            Die();
        else
            animator?.SetTrigger("takeDamage");
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        if (animator)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", true);
        }

        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject, destroyDelay);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        int segments = 24;
        float radius = detectionRadius;
        Vector3 center = transform.position;

        for (int i = 0; i < segments; i++)
        {
            float theta1 = Mathf.PI * i / segments * 0.5f;
            float theta2 = Mathf.PI * (i + 1) / segments * 0.5f;

            for (int j = 0; j <= segments; j++)
            {
                float phi1 = 2 * Mathf.PI * j / segments;
                Vector3 p1 = new Vector3(
                    radius * Mathf.Sin(theta1) * Mathf.Cos(phi1),
                    radius * Mathf.Cos(theta1),
                    radius * Mathf.Sin(theta1) * Mathf.Sin(phi1)
                );
                Vector3 p2 = new Vector3(
                    radius * Mathf.Sin(theta2) * Mathf.Cos(phi1),
                    radius * Mathf.Cos(theta2),
                    radius * Mathf.Sin(theta2) * Mathf.Sin(phi1)
                );
                Gizmos.DrawLine(center + p1, center + p2);
            }
        }
    }
#endif
}
