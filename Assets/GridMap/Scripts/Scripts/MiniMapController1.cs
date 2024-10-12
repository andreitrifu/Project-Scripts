using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public RawImage minimapImage;
    public CameraController cameraController; // Reference to your CameraController script
    public CameraRestrictions cameraRestrictions; // Reference to your CameraRestrictions script

    private RectTransform minimapRectTransform;

    private void Start()
    {
        minimapRectTransform = minimapImage.rectTransform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localCursor;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, Input.mousePosition, null, out localCursor))
            {
                // Calculate the normalized position within the minimap (0 to 1)
                float normalizedX = Mathf.InverseLerp(0, minimapRectTransform.rect.width, localCursor.x);
                float normalizedY = Mathf.InverseLerp(0, minimapRectTransform.rect.height, localCursor.y);

                // Use the normalized coordinates to determine the target world position
                float maxX = cameraRestrictions.maxX;
                float maxZ = cameraRestrictions.maxZ;

                Vector3 targetPosition = new Vector3(
                    Mathf.Lerp(cameraRestrictions.minX, maxX, normalizedX),
                    cameraController.transform.position.y, // Keep the Y position as the camera's current Y position
                    Mathf.Lerp(cameraRestrictions.minZ, maxZ, normalizedY)
                );

                // Set the new position in your CameraController
                cameraController.SetNewPosition(targetPosition);
            }
        }
    }
}
