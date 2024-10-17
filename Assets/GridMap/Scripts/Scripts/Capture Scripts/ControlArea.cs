using UnityEngine;//
using UnityEngine.UI;

public class ControlArea : MonoBehaviour
{
    public int maxProgress = 100;
    public float progressPerSecond = 1.0f;
    public float searchRadius = 5.0f;

    public int team1Count = 0;
    public int team2Count = 0;

    private float timer = 0.0f;
    private float timeInterval = 1.0f;

    private float currentProgress1 = 0f;
    private float currentProgress2 = 0f;
    public bool team1Taken = false;
    public bool team2Taken = false;

    public Slider team1ProgressBar;
    public Slider team2ProgressBar;

    private void Start()
    {
        if (team1ProgressBar == null || team2ProgressBar == null)
        {
            Debug.LogError("Please assign the Team 1 and Team 2 progress bar Sliders in the Inspector.");
        }
    }

    private void FixedUpdate()
    {
        // Check if the timer has reached the interval
        if (timer >= timeInterval && (currentProgress1 < maxProgress && currentProgress2 < maxProgress))
        {
            // Reset the timer
            timer = 0.0f;

            // Search for units within the search radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);

            team1Count = 0;
            team2Count = 0;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Team1"))
                {
                    team1Count++;
                }
                else if (collider.CompareTag("Team2"))
                {
                    team2Count++;
                }
            }

            if (team1Count > 0 && team2Count == 0)
            {
                currentProgress1 += progressPerSecond;
                UpdateUIProgress();
            }
            else if (team2Count > 0 && team1Count == 0)
            {
                currentProgress2 += progressPerSecond;
                UpdateUIProgress();
            }
        }
        timer += Time.deltaTime;

        if (currentProgress1 >= maxProgress && !team1Taken)
        {
            // Team 1 conquered the area
            Debug.Log("Team 1 conquered the area!");
            team1ProgressBar.value = 100f;
            team1Taken = true;
            RemoveControlPoint();
        }

        if (currentProgress2 >= maxProgress && !team2Taken)
        {
            // Team 2 conquered the area
            Debug.Log("Team 2 conquered the area!");
            team2ProgressBar.value = 100f;
            team2Taken = true;
            RemoveControlPoint();
        }
    }

    private void UpdateUIProgress()
    {
        team1ProgressBar.value = currentProgress1 / maxProgress;
        team2ProgressBar.value = currentProgress2 / maxProgress;
    }

    private void RemoveControlPoint()
    {
        // Remove the conquerable point's transform
       // Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
