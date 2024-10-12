using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{

    public GameObject targetImage;

    private void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("Target Image not assigned!");
        }
    }

    public void ToggleVisibility()
    {
        targetImage.SetActive(!targetImage.activeSelf);
    }
}
