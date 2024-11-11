using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;
    public GameObject groundMarker;
    public GameObject distanceMarker;

    public LayerMask clickable;
    public LayerMask ground;

    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        HandleLeftClick();
        HandleRightClick();
        HandleMiddleClick();
        HandleAltKey();
    }

    private void HandleLeftClick()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (PerformRaycast(clickable, out hit))
            {
                if (hit.collider.CompareTag("Team2"))
                {
                    return; // Skip selection if the object has the "Team2" tag
                }
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                UnitSelections.Instance.DeselectAll();
            }
        }
    }

    private void HandleRightClick()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (PerformRaycast(ground, out hit))
            {
                ActivateMarker(groundMarker, hit.point);
            }
        }
    }

    private void HandleMiddleClick()
    {
        if (Input.GetMouseButton(2))
        {
            RaycastHit hit;
            if (PerformRaycast(ground, out hit))
            {
                ActivateMarker(distanceMarker, hit.point);
            }
        }
    }

    private void HandleAltKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("Left Alt key pressed");
            UnitSelections.Instance.DeselectAll();
        }
    }

    private bool PerformRaycast(LayerMask layer, out RaycastHit hit)
    {
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layer);
    }

    private void ActivateMarker(GameObject marker, Vector3 position)
    {
        marker.transform.position = position;
        marker.SetActive(false);
        marker.SetActive(true);
    }
}
