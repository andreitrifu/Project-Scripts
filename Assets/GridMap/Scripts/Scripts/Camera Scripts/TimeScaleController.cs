using UnityEngine;//
using UnityEngine.UI;

public class TimeScaleController : MonoBehaviour
{
    // Array of time scales to loop through (normal, slow motion, fast motion)
    public float[] timeScales = { 1.0f, 0.2f, 2.0f };
    private int currentTimeScaleIndex = 0;

    // Text objects to display the current time scale state
    public Text normalSpeedText;
    public Text slowMotionText;
    public Text fastMotionText;

    void Start()
    {
        // Ensure only the initial state text is enabled
        UpdateTimeScaleText();
    }

    void Update()
    {
        // Check for user input to toggle the time scale
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Increment the index and wrap around if necessary
            currentTimeScaleIndex = (currentTimeScaleIndex + 1) % timeScales.Length;
            // Set the time scale to the new value
            Time.timeScale = timeScales[currentTimeScaleIndex];
            // Ensure that fixedDeltaTime is also scaled to maintain physics accuracy
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            // Update the text to reflect the current time scale state
            UpdateTimeScaleText();
        }
    }

    void UpdateTimeScaleText()
    {
        // Disable all text objects
        normalSpeedText.gameObject.SetActive(false);
        slowMotionText.gameObject.SetActive(false);
        fastMotionText.gameObject.SetActive(false);

        // Enable the correct text object based on the current time scale index
        switch (currentTimeScaleIndex)
        {
            case 0:
                normalSpeedText.gameObject.SetActive(true);
                break;
            case 1:
                slowMotionText.gameObject.SetActive(true);
                break;
            case 2:
                fastMotionText.gameObject.SetActive(true);
                break;
        }
    }
}
