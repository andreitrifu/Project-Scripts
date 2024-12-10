using UnityEngine;
using UnityEngine.UI;

public class ArtilleryManager : MonoBehaviour
{
    public GameObject projectilePrefab;
    public LayerMask groundLayer;
    public float spawnRadius = 2f;
    public int projectileCount = 5;
    public float cooldownTime = 10f;
    public Slider cooldownSlider;

    private float cooldownTimer = 0f;
    private bool isCooldown = false;

    void Update()
    {
        HandleCooldownTimer();
        HandleInput();
    }

    void HandleCooldownTimer()
    {
        if (!isCooldown) return;

        cooldownTimer -= Time.deltaTime;
        cooldownSlider.value = 1f - (cooldownTimer / cooldownTime); // Update slider value

        if (cooldownTimer <= 0)
        {
            isCooldown = false;
            cooldownSlider.gameObject.SetActive(false); // Hide slider when ready
        }
    }

    void HandleInput()
    {
        if (isCooldown || !Input.GetKey(KeyCode.B) || !Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            SpawnProjectiles(hit.point);
            StartCooldown();
        }
    }

    void SpawnProjectiles(Vector3 targetPosition)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = targetPosition + new Vector3(randomOffset.x, Random.Range(350f, 400f), randomOffset.y);
            Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        }
    }

    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
        cooldownSlider.gameObject.SetActive(true);
        cooldownSlider.value = 0f;
    }
}
