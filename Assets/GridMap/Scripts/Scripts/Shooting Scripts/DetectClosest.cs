using System.Collections;
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
        Initialize();
    }

    void Update()
    {
        UpdateEnemyList();
        closestEnemy = FindClosestEnemy();

        if (shootsNow || reloadsNow) return;

        if (closestEnemy != null)
        {
            HandleShooting();
            HandleRotation();
        }

        HandleGridMovementBasedOnRole();
    }

    private void Initialize()
    {
        bullets = maxBullets;
        animator = GetComponent<Animator>();
        UpdateEnemyList();
        testing = FindObjectOfType<Testing>();
        previousPosition = transform.position;
    }

    private void HandleShooting()
    {
        float distanceToTarget = Vector3.Distance(transform.position, closestEnemy.position);

        if (distanceToTarget <= shootingRange && bullets > 0)
        {
            Shoot();
        }
        else if (bullets == 0)
        {
            Reload();
        }
    }

    private void HandleRotation()
    {
        float distanceToTarget = Vector3.Distance(transform.position, closestEnemy.position);

        if (distanceToTarget <= 120f)
        {
            RotateTowards(closestEnemy.position);
        }
    }

    private void HandleGridMovementBasedOnRole()
    {
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

    private void HandleGridMovement(Grid mainGrid, Grid youGrid, Grid aiGrid)
    {
        mainGrid.GetXY(transform.position, out int currentX, out int currentY);
        mainGrid.GetXY(previousPosition, out int previousX, out int previousY);

        if (currentX != previousX || currentY != previousY)
        {
            UpdateGridPosition(mainGrid, youGrid, aiGrid, previousPosition, -10);
            UpdateGridPosition(mainGrid, youGrid, aiGrid, transform.position, 10);
            previousPosition = transform.position;

            CheckHealthAndRemoveFromGrid(mainGrid, youGrid, aiGrid);
        }
    }

    private void UpdateGridPosition(Grid mainGrid, Grid youGrid, Grid aiGrid, Vector3 position, int value)
    {
        mainGrid.AddValue(position, value, 3, 3);

        if (tag == "Team1")
            youGrid.AddValue(position, value, 3, 3);
        else
            aiGrid.AddValue(position, value, 3, 3);
    }

    private void CheckHealthAndRemoveFromGrid(Grid mainGrid, Grid youGrid, Grid aiGrid)
    {
        Health healthComponent = GetComponent<Health>();

        if (healthComponent != null && healthComponent.currentHealthVar <= 0)
        {
            UpdateGridPosition(mainGrid, youGrid, aiGrid, transform.position, -10);
        }
    }

    private void RaycastToTarget()
    {
        RaycastHit hit;
        Vector3 start = transform.position + Vector3.up;
        Vector3 end = closestEnemy.position + Vector3.up;
        Debug.DrawLine(start, end, Color.green, 0.1f);

        if (Physics.Linecast(start, end, out hit, groundLayer))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                // Obstacle detected between the unit and the target.
            }
        }
    }

    private void UpdateEnemyList()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(tagToDetect);
        allEnemies = new Transform[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            allEnemies[i] = enemyObjects[i].transform;
        }
    }

    private Transform FindClosestEnemy()
    {
        Transform closest = null;
        float leastDistance = Mathf.Infinity;

        foreach (var enemy in allEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);

            if (distance < leastDistance)
            {
                leastDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void Shoot()
    {
        if (bullets > 0 && !shootsNow && !reloadsNow)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        animator.SetBool("IsShooting", true);
        shootsNow = true;

        if (closestEnemy != null)
        {
            RotateTowards(closestEnemy.position);

            if (IsEnemyInLineOfSight())
            {
                FireProjectileAtEnemy();
                bullets--;
                yield return new WaitForSeconds(betweenShots);
            }
            else
            {
                Debug.Log("Enemy not in line of sight.");
            }
        }

        animator.SetBool("IsShooting", false);
        shootsNow = false;
    }
    private bool IsEnemyInLineOfSight()
    {
        RaycastHit hit;
        Vector3 start = transform.position + Vector3.up;
        Vector3 directionToEnemy = (closestEnemy.position + Vector3.up) - start;

        if (Physics.Raycast(start, directionToEnemy, out hit, shootingRange))
        {
            return hit.collider.gameObject == closestEnemy.gameObject;
        }
        return false;
    }

    private void FireProjectileAtEnemy()
    {
        Rigidbody projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<Rigidbody>();
        if (projectile != null)
        {
            projectile.velocity = (closestEnemy.position - transform.position).normalized * projectileSpeed;
            Debug.Log("Shot fired!");
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody component.");
        }
    }


    private void AttemptRaycastAndShoot()
    {
        RaycastHit hit;
        Vector3 start = transform.position + Vector3.up;
        Vector3 directionToEnemy = (closestEnemy.position + Vector3.up) - start;

        if (Physics.Raycast(start, directionToEnemy, out hit, shootingRange) && hit.collider.gameObject == closestEnemy.gameObject)
        {
            FireProjectile(directionToEnemy);
        }
    }

    private void FireProjectile(Vector3 directionToEnemy)
    {
        Rigidbody projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<Rigidbody>();

        if (projectile != null)
        {
            projectile.velocity = directionToEnemy.normalized * projectileSpeed;
            bullets--;
            Debug.Log("Shot fired!");
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody component.");
        }
    }

    private void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        reloadsNow = true;
        if (closestEnemy != null) RotateTowards(closestEnemy.position);

        animator.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("IsReloading", false);
        bullets = maxBullets;
        reloadsNow = false;
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
