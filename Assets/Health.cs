using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth = 100f;
    public float currentHealthVar = 100;
    private Testing testing;
    private Vector3 previousPosition;

    void Start()
    {
        currentHealth = maxHealth;

        // Find and assign the Testing component in the scene
        testing = FindObjectOfType<Testing>();
    }
    private void FixedUpdate()
    {
        if (currentHealth > 0)
        {
            // Get the grid coordinates for the current position
            testing.gridSoldiers.GetXY(transform.position, out int currentX, out int currentY);

            // Get the grid coordinates for the previous position
            testing.gridSoldiers.GetXY(previousPosition, out int previousX, out int previousY);

            // Check if the soldier has moved to a new grid cell
            if (currentX != previousX || currentY != previousY)
            {
                // Remove value from the previous position
                testing.gridSoldiers.AddValue(previousPosition, -10, 3,3);//
                if (tag == "Team1")
                    testing.gridSoldiersYou.AddValue(previousPosition, -10, 3, 3);
                else testing.gridSoldiersAI.AddValue(previousPosition, -10, 3, 3);
                // Add value to the current position
                testing.gridSoldiers.AddValue(transform.position, 10,3,3);
                if (tag == "Team1")
                    testing.gridSoldiersYou.AddValue(transform.position, 10, 3, 3);
                else testing.gridSoldiersAI.AddValue(transform.position, 10, 3, 3);
                // Update the previous position
                previousPosition = transform.position;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealthVar = currentHealth;
        testing.gridDamage.GetXY(transform.position, out int currentX, out int currentY);
        testing.gridDamage.AddValue(currentX, currentY, 2);
        if (currentHealth <= 0)
        {
            // Call the Die method from the Testing component
            testing.gridSoldiers.AddValue(transform.position, -10, 3, 3);

            if (tag == "Team1")
                testing.gridSoldiersYou.AddValue(transform.position, -10, 3, 3);
            else testing.gridSoldiersAI.AddValue(transform.position, -10, 3, 3);
            Die();
        }
    }

    public void Die()
    {
        // Get the position of the object in world space
        Vector3 deathPosition = transform.position;
        Destroy(gameObject);
    }
}
