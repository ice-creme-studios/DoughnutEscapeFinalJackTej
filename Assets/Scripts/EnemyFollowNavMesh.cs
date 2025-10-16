using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollowNavMesh : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float stopDistance = 2f;

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackRate = 1.0f; // attacks per second (cooldown = 1/attackRate)
    public float attackDelay = 0.2f; // optional delay before applying damage (for animation timing)

    private NavMeshAgent agent;
    private bool isAggro = false;
    private float nextAttackTime = 0f;
    private PlayerHealth playerHealth; // cached reference for performance

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        // optional: cache the PlayerHealth component if it exists
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Latch the aggro when player enters detection radius
        if (!isAggro && distance <= detectionRadius)
        {
            isAggro = true;
        }

        if (isAggro)
        {
            // If we are within stopping/attack range, stop moving and attack
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
    }

    void TryAttack()
    {
        // check cooldown
        if (Time.time < nextAttackTime) return;

        // set next attack time
        nextAttackTime = Time.time + (1f / Mathf.Max(attackRate, 0.0001f));

        // Optionally play attack animation here and then apply damage after a short delay
        // StartCoroutine is used so animation timing can line up with when damage is applied.
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

        // fallback: try to find any damage method on the player dynamically
        var dmgable = player.GetComponent<IDamageable>();
        if (dmgable != null)
        {
            dmgable.TakeDamage(attackDamage);
            return;
        }

        // last resort: use SendMessage (less safe/performant)
        player.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
