using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollowNavMesh : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float stopDistance = 2f;

    private NavMeshAgent agent;
    private bool isAggro = false; // NEW: latch flag

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

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
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
