using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public static List<GameObject> unitsSelected = new List<GameObject>();

    private static UnitSelections instance;
    public static UnitSelections Instance { get { return instance; } }

   // public float spacing = 20.0f; // Spacing between selected units in the formation.
   //  private Vector3 formationCenter; // Center of the formation.

    private void Awake()
    {
        if(instance != null&& instance != this)
        {
            Destroy(this.gameObject); 
        }
        else
        {
            instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        unitToAdd.GetComponent<UnitMovement>().enabled = true;
    }
    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if(!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            int lastIndex = unitsSelected.IndexOf(unitsSelected.Last()) + 1;
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
            foreach (var unit in unitList)
            {
                NavMeshAgent navAgent = unit.GetComponent<NavMeshAgent>();
                if (navAgent != null)
                {
                    navAgent.stoppingDistance = (float)(lastIndex * 0.7);
                }
            }
        }
        else
        {
            unitToAdd.GetComponent<UnitMovement>().enabled = false;
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitsSelected.Remove(unitToAdd);          
        }
    }
    public void ControlClickSelect(GameObject clickedUnit)
    {
        DeselectAll(); // First, deselect all previously selected units

        int companyToSelect = clickedUnit.GetComponent<Unit>().companyNumber;

        foreach (var unit in unitList)
        {
            if (unit.GetComponent<Unit>().companyNumber == companyToSelect)
            {
                unitsSelected.Add(unit);
                int lastIndex = unitsSelected.IndexOf(unitsSelected.Last()) + 1;
                unit.transform.GetChild(0).gameObject.SetActive(true);
                unit.GetComponent<UnitMovement>().enabled = true;
                NavMeshAgent navAgent = unit.GetComponent<NavMeshAgent>();
                if (navAgent != null)
                {
                    navAgent.stoppingDistance = (float)(lastIndex * 2);
                }
            }
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        if(!unitsSelected.Contains(unitToAdd))
        {            
            unitsSelected.Add(unitToAdd);
            int lastIndex = unitsSelected.IndexOf(unitsSelected.Last())+1;
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
            foreach (var unit in unitsSelected)
            {
                NavMeshAgent navAgent = unit.GetComponent<NavMeshAgent>();
                if (navAgent != null)
                {
                    navAgent.stoppingDistance = (float)(lastIndex * 0.7);
                }
            }               
        }
    }

    public void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.GetChild(0).gameObject.gameObject.SetActive(false);
        }
        unitsSelected.Clear();
    }
    public void Deselect(GameObject unitToDeselect) 
    { 

    }

   /* public void CalculateDistancesToCursor(Vector3 cursorPosition)
    {
        foreach (var selectedUnit in unitsSelected)
        {
            float distance = Vector3.Distance(selectedUnit.transform.position, cursorPosition);
            Debug.Log("Distance to Cursor for " + selectedUnit.name + ": " + distance);
        }
    }*/
}