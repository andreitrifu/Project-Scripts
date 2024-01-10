using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private SoldierHeatMapVisual soldierHeatMapVisual;
    public Grid grid { get; private set; } // Make it a property
    public Grid gridSoldiers { get; private set; }

    private void Start()
    {
        grid = new Grid(50, 50, 50f, new Vector3(1500f, 480f, 1500f));
        gridSoldiers = new Grid(50, 50, 50f, new Vector3(1500f, 482f, 1500f));
        heatMapVisual.SetGrid(grid);
        soldierHeatMapVisual.SetSoldierGrid(gridSoldiers);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            grid.AddValue(position, 20, 5, 5);
        }
    }
}
