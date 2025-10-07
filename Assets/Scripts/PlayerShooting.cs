using UnityEngine;
using UnityEngine.InputSystem; // new input system

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;

    [Header("Settings")]
    public float damage = 50f;
    public float range = 100f;
    public float fireRate = 5f; // shots per second

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootClip;

    private float nextTimeToFire = 0f;

    void Update()
    {
        // Only proceed if mouse is detected
        if (Mouse.current == null || cam == null)
            return;

        // Fire when left mouse button is pressed (hold allowed)
        bool isPressed = Mouse.current.leftButton.isPressed;

        if (isPressed && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash) muzzleFlash.Play();
        if (audioSource && shootClip) audioSource.PlayOneShot(shootClip);

        // Create a ray from the center of the screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Optional: Spawn impact effect
            if (hitEffectPrefab)
                Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            // Check if the hit object has an EnemyHealth script
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy)
                enemy.TakeDamage(damage);
        }
    }
}
