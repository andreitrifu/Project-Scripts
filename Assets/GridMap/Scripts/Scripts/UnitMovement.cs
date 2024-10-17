using System.Collections;//
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    float distance = 0f; // Initialize distance
    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                myAgent.SetDestination(hit.point); 
            }
        }
        else if (Input.GetMouseButtonDown(2))
        {
            CalculateDistanceToCenterOfSelection();
        }
    }
    public float CalculateDistanceToCenterOfSelection()
    {
        if (UnitSelections.unitsSelected.Count == 0)
        {
           // Debug.Log("No units selected.");
            return distance;
        }

        Vector3 centerPosition = Vector3.zero;
        foreach (var selectedUnit in UnitSelections.unitsSelected)
        {
            centerPosition += selectedUnit.transform.position;
        }
        centerPosition /= UnitSelections.unitsSelected.Count;

        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
        {
            distance = Vector3.Distance(centerPosition, hit.point); // Update the class-level variable
            //Debug.Log("Distance to Center: " + distance);
        }
        else
        {
            Debug.Log("Cursor not over the ground.");
        }

        return distance;
    }

}
