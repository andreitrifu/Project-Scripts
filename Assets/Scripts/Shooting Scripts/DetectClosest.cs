using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClosest : MonoBehaviour
{
    public string tagToDetect = "Team2";
    public GameObject[] allEnemies;
    public GameObject closestEnemy;
    public LayerMask groundLayer;
    public float shootingRange = 100f;
    public float bullets = 10;
    public float maxBullets = 10;
    public float betweenShots = 0.1f;
    public float reloadTime = 2f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;
    private Animator animator;
    public bool canShoot = true;
    private bool reloadsNow = false;
    private bool shootsNow = false;

    void Start()
    {
        bullets = maxBullets;
        animator = GetComponent<Animator>();
        allEnemies = GameObject.FindGameObjectsWithTag(tagToDetect);
    }

    void FixedUpdate()
    {
        UpdateEnemyList();  // Update the list of enemies in each FixedUpdate
        closestEnemy = ClosestEnemy();
        if (shootsNow)
            return;
        if (reloadsNow)
            return;
        if (closestEnemy != null)
        {
            // Check if the target is within shooting range
            float distanceToTarget = Vector3.Distance(transform.position, closestEnemy.transform.position);

            if (distanceToTarget <= shootingRange)
            {
                // Check line of sight
                RaycastHit hit;
                Vector3 start = transform.position + Vector3.up;
                Vector3 end = closestEnemy.transform.position + Vector3.up;
                Debug.DrawLine(start, end, Color.green, 0.1f);

                if (Physics.Linecast(start, end, out hit, groundLayer))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        // Your shooting logic here
                        //Debug.Log("Obstacle detected between the unit and the target.");                        
                    }

                }
                else if (bullets > 0)
                {
                    //canShoot = false;
                    Shoot();
                    //Debug.Log("Shooting!");
                }
                else if (bullets == 0)
                {
                    Reload();
                }
            }
        }
    }

    void UpdateEnemyList()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tagToDetect);
        allEnemies = enemies;
    }

    GameObject ClosestEnemy()
    {
        GameObject closestHere = null;
        float leastDistance = Mathf.Infinity;

        foreach (var enemy in allEnemies)
        {
            float distanceHere = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceHere < leastDistance)
            {
                leastDistance = distanceHere;
                closestHere = enemy;
            }
        }

        return closestHere;
    }

    void Shoot()
    {
        // Check if bullets are available
        if (bullets > 0)
        {
            StartCoroutine(ShootCoroutine());
        }
        /* else
         {
             // Implement reloading logic or other actions when out of bullets
             animator.SetBool("IsReloading", true);
             Debug.Log("Out of bullets! Reloading...");
             StartCoroutine(Reload());
         }*/
    }

    IEnumerator ShootCoroutine()
    {
        animator.SetBool("IsShooting", true);
        shootsNow = true;
        GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity);

        Vector3 directionToTarget = (closestEnemy.transform.position - transform.position).normalized;

        // Apply a force or velocity to the projectile to make it move towards the target
        projectile.GetComponent<Rigidbody>().velocity = directionToTarget * projectileSpeed;

        bullets--;

        yield return new WaitForSeconds(betweenShots);
        shootsNow = false;
        animator.SetBool("IsShooting", false);
    }

    void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }
    IEnumerator ReloadCoroutine()
    {
        reloadsNow = true;
        animator.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("IsReloading", false);
        bullets = maxBullets;
        reloadsNow = false;
    }
}