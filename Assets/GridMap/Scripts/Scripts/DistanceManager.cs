using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceManager : MonoBehaviour
{
    public Text TextUIDistance;
    private UnitMovement unitMovement;
    private bool isRightMouseButtonDown = false;

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
        unitMovement = FindObjectOfType<UnitMovement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isRightMouseButtonDown = true;
        }
    }

    void FixedUpdate()
    {
        if (unitMovement != null && isRightMouseButtonDown)
        {
            int newDistance = Mathf.FloorToInt(unitMovement.CalculateDistanceToCenterOfSelection());
            Distance = newDistance;
        }

        isRightMouseButtonDown = false;
    }
}