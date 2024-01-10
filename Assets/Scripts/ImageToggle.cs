using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{

    public GameObject targetImage;

    private void Start()
    {
        // Make sure the targetImage is assigned in the Unity Editor
        if (targetImage == null)
        {
            Debug.LogError("Target Image not assigned!");
        }
    }

    public void ToggleVisibility()
    {
        // Toggle the visibility of the entire GameObject
        targetImage.SetActive(!targetImage.activeSelf);
    }
}
