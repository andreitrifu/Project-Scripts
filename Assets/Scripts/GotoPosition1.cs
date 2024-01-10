using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPosition : MonoBehaviour
{
    public Vector3 targetPosition; // This allows you to set the target position directly in the Inspector
    public float moveSpeed = 5.0f; // Adjust the speed as needed

    private bool isMoving = false;
    public Transform cameraTransform; // Reference to the camera's transform component

    private void Start()
    {
        // Assuming this script is attached to the camera GameObject
        cameraTransform = transform;
    }

    private void Update()
    {
        if (isMoving)
        {
            cameraTransform.position = targetPosition;
            isMoving = false;
        }
    }

    public void TeleportCameraToTarget()
    {
        isMoving = true;
    }
}
