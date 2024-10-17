using System.Collections;//
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
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray= myCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {//if hit a clickable obj
                if (hit.collider.CompareTag("Team2")) // Check if the clicked object has the "Team2" tag
                {
                    return; // Skip selection if the object has the "Team2" tag
                }
                /* if(Input.GetKey(KeyCode.LeftShift))
                 {
                     UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                 }
                 else if (Input.GetKey(KeyCode.LeftControl))
                 {
                     UnitSelections.Instance.ControlClickSelect(hit.collider.gameObject);
                 }
                 else
                 {
                     UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                 }       */

            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.DeselectAll();
                }              
            }
        }else if (Input.GetKeyDown(KeyCode.LeftAlt)) // Added condition for Left Alt key
    {
        UnitSelections.Instance.DeselectAll();
    }
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }
        if (Input.GetMouseButton(2))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                distanceMarker.transform.position = hit.point;
                distanceMarker.SetActive(false);
                distanceMarker.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("Left Alt key pressed");
            UnitSelections.Instance.DeselectAll();
        }
    }
}
