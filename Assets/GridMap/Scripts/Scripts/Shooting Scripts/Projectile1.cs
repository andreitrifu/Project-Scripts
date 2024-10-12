using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    public float damageAmount = 10f; 
    public float destroyDelay = 2f; 

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object on the "Ground" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Destroy the projectile upon collision with the ground
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Team1"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            // Destroy the projectile after hitting the target
            Destroy(gameObject);
        }
    }
}
