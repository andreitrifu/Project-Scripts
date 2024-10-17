using System.Collections;//
using System.Collections.Generic;
using UnityEngine;

public class DetectClosest : MonoBehaviour
{
    public string tagToDetect = "Team2";
    public Transform[] allEnemies;
    public Transform closestEnemy;
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
    private Testing testing;
    private Vector3 previousPosition;
    public float rotationSpeed = 5f;

    void Start()
    {
        bullets = maxBullets;
        animator = GetComponent<Animator>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(tagToDetect);
        allEnemies = new Transform[enemyObjects.Length];
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            allEnemies[i] = enemyObjects[i].transform;
        }
        testing = FindObjectOfType<Testing>();
        previousPosition = transform.position;
    }

    void Update()
    {
        UpdateEnemyList();
        closestEnemy = ClosestEnemy();
        if (shootsNow || reloadsNow)
            return;

        if (closestEnemy != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, closestEnemy.transform.position);

            if (distanceToTarget <= shootingRange && bullets > 0)
            {
                // Continuous shooting
                Shoot();

                // Continuous raycasting
                RaycastToTarget();
            }
            else if (bullets == 0)
            {
                Reload();
            }
            if (distanceToTarget <= 120f)
            {
                // Rotate towards the closest enemy if within 120 meters
                RotateTowards(closestEnemy.transform.position);
            }
        }

        if (maxBullets == 8) // riflemen
        {
            HandleGridMovement(testing.gridRiflemen, testing.gridRiflemenYou, testing.gridRiflemenAI);
        }
        else if (maxBullets == 20) // machineguns
        {
            HandleGridMovement(testing.gridMachineguns, testing.gridMachinegunsYou, testing.gridMachinegunsAI);
        }
        else if (maxBullets == 3) // snipers
        {
            HandleGridMovement(testing.gridSnipers, testing.gridSnipersYou, testing.gridSnipersAI);
        }
    }

    void HandleGridMovement(Grid mainGrid, Grid youGrid, Grid aiGrid)
    {
        mainGrid.GetXY(transform.position, out int currentX, out int currentY);
        mainGrid.GetXY(previousPosition, out int previousX, out int previousY);

        if (currentX != previousX || currentY != previousY)
        {
            mainGrid.AddValue(previousPosition, -10, 3, 3);
            if (tag == "Team1")
                youGrid.AddValue(previousPosition, -10, 3, 3);
            else
                aiGrid.AddValue(previousPosition, -10, 3, 3);

            mainGrid.AddValue(transform.position, 10, 3, 3);
            if (tag == "Team1")
                youGrid.AddValue(transform.position, 10, 3, 3);
            else
                aiGrid.AddValue(transform.position, 10, 3, 3);

            previousPosition = transform.position;

            Health healthComponent = GetComponent<Health>();
            float currentHealth = healthComponent.currentHealthVar;
            if (currentHealth <= 0)
            {
                mainGrid.AddValue(transform.position, -10, 3, 3);
                if (tag == "Team1")
                    youGrid.AddValue(transform.position, -10, 3, 3);
                else
                    aiGrid.AddValue(transform.position, -10, 3, 3);
            }
        }
    }

    void RaycastToTarget()
    {
        RaycastHit hit;
        Vector3 start = transform.position + Vector3.up;
        Vector3 end = closestEnemy.transform.position + Vector3.up;
        Debug.DrawLine(start, end, Color.green, 0.1f);

        if (Physics.Linecast(start, end, out hit, groundLayer))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                // Obstacle detected between the unit and the target.
            }
        }
    }

    void UpdateEnemyList()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(tagToDetect);
        allEnemies = new Transform[enemyObjects.Length];
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            allEnemies[i] = enemyObjects[i].transform;
        }
    }

    Transform ClosestEnemy()
    {
        Transform closestHere = null;
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
        if (bullets > 0 && !shootsNow && !reloadsNow)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        animator.SetBool("IsShooting", true);
        shootsNow = true;

        if (closestEnemy != null)
        {
            RotateTowards(closestEnemy.transform.position);
            RaycastHit hit;
            Vector3 start = transform.position + Vector3.up;
            Vector3 directionToEnemy = (closestEnemy.transform.position + Vector3.up) - start;

            Debug.DrawRay(start, directionToEnemy.normalized * shootingRange, Color.red, 0.1f);

            // Check if there is a clear line of sight to the enemy
            if (Physics.Raycast(start, directionToEnemy, out hit, shootingRange))
            {
                if (hit.collider.gameObject == closestEnemy.gameObject)
                {
                    // Instantiate the projectile
                    Rigidbody projectile = Instantiate(projectilePrefab, start, Quaternion.identity).GetComponent<Rigidbody>();

                    if (projectile != null)
                    {
                        // Set the projectile's velocity towards the enemy
                        projectile.velocity = directionToEnemy.normalized * projectileSpeed;
                        bullets--;

                        Debug.Log("Shot fired!");

                        yield return new WaitForSeconds(betweenShots);
                    }
                    else
                    {
                        Debug.LogError("Projectile prefab is missing Rigidbody component.");
                    }
                }
                else
                {
                    Debug.Log("Raycast hit an object other than the closest enemy.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object within the shooting range.");
            }
        }

        animator.SetBool("IsShooting", false);
        shootsNow = false;
    }

    void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        reloadsNow = true;
        if (closestEnemy != null)
        {
            RotateTowards(closestEnemy.transform.position);
        }
        animator.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("IsReloading", false);
        bullets = maxBullets;
        reloadsNow = false;
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
