using System.Collections;//
using System.Collections.Generic;
using UnityEngine;

public class GotoPosition : MonoBehaviour
{
    public Vector3 targetPosition;
    public float moveSpeed = 5.0f;

    private bool isMoving = false;
    public Transform cameraTransform;

    private void Start()
    {
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
