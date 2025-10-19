using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollowNavMesh : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 2f;

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackRate = 1.0f; // attacks per second
    public float attackDelay = 0.2f; // optional delay for animations

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Automatically find player by tag if not assigned
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
            else
                Debug.LogWarning("EnemyFollowNavMesh: No GameObject with tag 'Player' found!");
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Always aggro: follow player regardless of distance
        if (distance <= stopDistance)
        {
            agent.isStopped = true;
            TryAttack();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void TryAttack()
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + (1f / Mathf.Max(attackRate, 0.0001f));

        if (attackDelay > 0f)
            StartCoroutine(DelayedDamage(attackDelay));
        else
            ApplyDamage();
    }

    System.Collections.IEnumerator DelayedDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        ApplyDamage();
    }

    void ApplyDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            return;
        }

        var dmgable = player.GetComponent<IDamageable>();
        if (dmgable != null)
        {
            dmgable.TakeDamage(attackDamage);
            return;
        }

        player.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
