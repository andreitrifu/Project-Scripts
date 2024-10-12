using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 10f;
    public float destroyDelay = 2f;

    private void Start()
    {
        // Destroy the projectile after a delay
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "Team2" tag
        if (other.CompareTag("Team2"))
        {
            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }
}
