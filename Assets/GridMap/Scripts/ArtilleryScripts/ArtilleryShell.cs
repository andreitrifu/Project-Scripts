using UnityEngine;

public class ArtilleryShell : MonoBehaviour
{
    public GameObject explosionPrefab;
    public LayerMask groundLayer;
    public float explosionRadius = 6f;
    public float damageAmount = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if shell hit ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Spawn explosion
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            DealDamageToNearbySoldiers();
            Destroy(gameObject);
        }
    }

    void DealDamageToNearbySoldiers()
    {
        // Find all colliders within explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in hitColliders)
        {
            // Check if the collider belongs to a soldier
            Health health = collider.GetComponent<Health>();
            if (health != null && (collider.CompareTag("Team1") || collider.CompareTag("Team2")))
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
