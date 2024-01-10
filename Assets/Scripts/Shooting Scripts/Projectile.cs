using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 10f; // Adjust the damage amount as needed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "Team2" tag
        if (other.CompareTag("Team2"))
        {
            // Retrieve the health component from the collided object
            Health health = other.GetComponent<Health>();

            // If the object has a Health component, reduce its health by the damage amount
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            // Destroy the projectile after hitting the target
            Destroy(gameObject);
        }
    }
}
