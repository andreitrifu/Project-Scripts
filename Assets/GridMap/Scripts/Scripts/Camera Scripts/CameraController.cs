using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    private void Start()
    {
        instance = this;

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void FixedUpdate()
    {
        HandleMovementSpeed();
        UpdatePosition();
        UpdateRotation();
        UpdateZoom();
        ApplyCameraTransforms();
    }

    private void LateUpdate()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            AdjustZoom(Input.mouseScrollDelta.y * zoomAmount / 3);
        }
    }

    private void HandleMovementSpeed()
    {
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;
    }

    private void UpdatePosition()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) MoveCamera(transform.forward);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) MoveCamera(-transform.forward);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) MoveCamera(transform.right);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) MoveCamera(-transform.right);
    }

    private void MoveCamera(Vector3 direction)
    {
        newPosition += direction * movementSpeed;
    }

    private void UpdateRotation()
    {
        if (Input.GetKey(KeyCode.Q)) RotateCamera(-rotationAmount);
        if (Input.GetKey(KeyCode.E)) RotateCamera(rotationAmount);
    }

    private void RotateCamera(float rotation)
    {
        newRotation *= Quaternion.Euler(Vector3.up * rotation);
    }

    private void UpdateZoom()
    {
        if (Input.GetKey(KeyCode.R)) AdjustZoom(zoomAmount);
        if (Input.GetKey(KeyCode.F)) AdjustZoom(-zoomAmount);
    }

    private void AdjustZoom(Vector3 zoomChange)
    {
        Vector3 potentialZoom = newZoom + zoomChange;
        if (IsZoomWithinLimits(potentialZoom))
        {
            newZoom = potentialZoom;
        }
    }

    private bool IsZoomWithinLimits(Vector3 zoom)
    {
        return zoom.y < 200;  
    }

    private void ApplyCameraTransforms()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    public void SetNewPosition(Vector3 position)
    {
        newPosition = position;
    }
}
