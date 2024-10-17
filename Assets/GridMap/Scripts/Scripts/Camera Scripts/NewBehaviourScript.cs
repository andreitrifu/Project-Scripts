using UnityEngine;//

public class TimeScaleController1 : MonoBehaviour
{
    // Change this value to adjust the time scale (1.0 is normal speed)
    public float timeScaleFactor1 = 0.5f;

    void Update()
    {
        // Check for user input or conditions to activate time scaling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle between normal and scaled time
            if (Time.timeScale == 1.0f)
            {
                // Set the time scale to the desired factor for slow motion
                Time.timeScale = timeScaleFactor1;
            }
            else
            {
                // Reset the time scale to normal speed
                Time.timeScale = 1.0f;
            }
        }

        // Ensure that fixedDeltaTime is also scaled to maintain physics accuracy
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
