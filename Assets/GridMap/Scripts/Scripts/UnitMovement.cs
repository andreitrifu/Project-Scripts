using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    float distance = 0f; // Initialize distance
    public float proximityThreshold = 20f; // Define how close a unit needs to be to a conquered point to trigger relocation

    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }
    public void Update()
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

        // Check proximity and set new destination if conditions are met
        CheckProximityToConqueredPoints();
    }

    public float CalculateDistanceToCenterOfSelection()
    {
        if (UnitSelections.unitsSelected.Count == 0)
        {
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
            distance = Vector3.Distance(centerPosition, hit.point);
        }
        else
        {
            Debug.Log("Cursor not over the ground.");
        }

        return distance;
    }

    public void CheckProximityToConqueredPoints()
    {
        // Gather all points with the "VictoryPoints" tag
        List<Transform> movePoints = new List<Transform>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("VictoryPoints"))
        {
            movePoints.Add(obj.transform);
        }

        // Check if near a conquered point
        foreach (Transform point in movePoints)
        {
            ControlArea controlArea = point.GetComponent<ControlArea>();
            if (controlArea != null && (controlArea.team1Taken || controlArea.team2Taken))
            {
                float distanceToPoint = Vector3.Distance(transform.position, point.position);

                if (distanceToPoint < proximityThreshold)
                {
                    // Move to the closest unconquered point
                    Transform newDestination = FindUnconqueredPoint(movePoints);
                    if (newDestination != null)
                    {
                        Debug.Log("Moving to closest unconquered point: " + newDestination.name);
                        myAgent.SetDestination(newDestination.position);
                    }
                    else
                    {
                        Debug.Log("No unconquered points available.");
                    }
                    break;
                }
            }
        }
    }

    public Transform FindUnconqueredPoint(List<Transform> movePoints)
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in movePoints)
        {
            ControlArea controlArea = point.GetComponent<ControlArea>();
            if (controlArea == null || (!controlArea.team1Taken && !controlArea.team2Taken))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count > 0)
        {
            // Find the closest unconquered point
            Transform closestPoint = null;
            float closestDistance = float.MaxValue;

            foreach (Transform availablePoint in availablePoints)
            {
                float distance = Vector3.Distance(transform.position, availablePoint.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = availablePoint;
                }
            }

            return closestPoint;
        }

        return null; // No unconquered points available
    }
}
