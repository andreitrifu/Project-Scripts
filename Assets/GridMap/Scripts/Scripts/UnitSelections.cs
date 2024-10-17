using System.Collections.Generic;//
using UnityEngine;
using UnityEngine.AI;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public static List<GameObject> unitsSelected = new List<GameObject>();
    private List<GameObject> dragSelectedUnits = new List<GameObject>(); // Newly added

    private static UnitSelections instance;
    public static UnitSelections Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        if (!dragSelectedUnits.Contains(unitToAdd))
        {
            dragSelectedUnits.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            FinishDragSelection();
        }
    }
    private void FinishDragSelection()
    {
        // Clear units previously selected by dragging
        foreach (var unit in dragSelectedUnits)
        {
            unit.transform.GetChild(0).gameObject.SetActive(false);
            unit.GetComponent<UnitMovement>().enabled = false;
        }
        dragSelectedUnits.Clear();

        // Update unitsSelected list
        unitsSelected.Clear();
        unitsSelected.AddRange(dragSelectedUnits);

        // Update stopping distances
        UpdateStoppingDistances();
    }

    private void UpdateStoppingDistances()
    {
        for (int i = 0; i < unitsSelected.Count; i++)
        {
            NavMeshAgent navAgent = unitsSelected[i].GetComponent<NavMeshAgent>();
            if (navAgent != null)
            {
                navAgent.stoppingDistance = (i + 1) * 0.9f;
            }
        }
    }

    public void DeselectAll()
    {
        Debug.Log("Deselecting all units");

        foreach (var unit in unitsSelected)
        {
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.GetChild(0).gameObject.gameObject.SetActive(false);
        }
        unitsSelected.Clear();
    }


    public void Deselect(GameObject unitToDeselect)
    {
        if (unitsSelected.Contains(unitToDeselect))
        {
            unitToDeselect.GetComponent<UnitMovement>().enabled = false;
            unitToDeselect.transform.GetChild(0).gameObject.SetActive(false);
            unitsSelected.Remove(unitToDeselect);
        }
    }
}
