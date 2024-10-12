using UnityEngine;
using System.Collections;

public class SoldierShooting : MonoBehaviour
{
    public Transform projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 20f;
    public float detectionRadius = 15f;
    public string enemyTag = "Enemy";  // Tag used to identify enemies
    public int maxAmmo = 10;
    public float reloadTime = 2f;

    private float fireCooldown;
    private int currentAmmo;
    private bool isReloading;

    void Start()
    {
        fireCooldown = 0f;
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        fireCooldown -= Time.deltaTime;

        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy != null && fireCooldown <= 0f)
        {
            Shoot(closestEnemy);
            fireCooldown = 1f / fireRate;
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance && distanceToEnemy <= detectionRadius)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    void Shoot(Transform target)
    {
        if (target == null)
        {
            return;
        }

        Transform projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }

        currentAmmo--;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
