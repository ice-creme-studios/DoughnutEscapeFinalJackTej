using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
