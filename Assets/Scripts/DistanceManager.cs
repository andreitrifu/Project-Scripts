using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceManager : MonoBehaviour
{
    public Text TextUIDistance;
    private UnitMovement unitMovement;
    private bool isRightMouseButtonDown = false; // Flag to track right mouse button press

    private int _dist;
    public int Distance
    {
        get { return _dist; }
        set
        {
            _dist = value;
            TextUIDistance.text ="Distance:" + Distance.ToString()+ "m";
        }
    }

    void Start()
    {
        // Find the UnitMovement script in the scene
        unitMovement = FindObjectOfType<UnitMovement>();
    }

    void Update()
    {
        // Check for right mouse button press
        if (Input.GetMouseButtonDown(2))
        {
            isRightMouseButtonDown = true;
        }
    }

    void FixedUpdate()
    {
        if (unitMovement != null && isRightMouseButtonDown)
        {
            // Get the distance value from UnitMovement
            int newDistance = Mathf.FloorToInt(unitMovement.CalculateDistanceToCenterOfSelection());
            Distance = newDistance;
        }

        // Reset the flag
        isRightMouseButtonDown = false;
    }
}