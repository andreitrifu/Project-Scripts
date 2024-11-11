using UnityEngine;
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
        UpdateTimer();

        if (timer >= timeInterval && !IsAreaConquered())
        {
            timer = 0.0f;
            UpdateTeamCounts();
            UpdateProgress();
        }

        CheckConquerStatus();
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    private bool IsAreaConquered()
    {
        return currentProgress1 >= maxProgress || currentProgress2 >= maxProgress;
    }

    private void UpdateTeamCounts()
    {
        team1Count = 0;
        team2Count = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Team1")) team1Count++;
            else if (collider.CompareTag("Team2")) team2Count++;
        }
    }

    private void UpdateProgress()
    {
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

    private void CheckConquerStatus()
    {
        if (currentProgress1 >= maxProgress && !team1Taken)
        {
            ConquerArea("Team 1", ref team1Taken, team1ProgressBar);
        }

        if (currentProgress2 >= maxProgress && !team2Taken)
        {
            ConquerArea("Team 2", ref team2Taken, team2ProgressBar);
        }
    }

    private void ConquerArea(string teamName, ref bool teamTaken, Slider progressBar)
    {
        Debug.Log($"{teamName} conquered the area!");
        progressBar.value = 100f;
        teamTaken = true;
        RemoveControlPoint();
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
