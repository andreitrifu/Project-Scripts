using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private SoldierHeatMapVisual soldierHeatMapVisual;
    [SerializeField] private SoldierHeatMapYouVisual soldierYouHeatMapVisual;
    [SerializeField] private SoldierHeatMapAIVisual soldierAIHeatMapVisual;
    [SerializeField] private RiflemenYouHeat riflemenYouHeatMapVisual;
    [SerializeField] private RiflemenAIHeatMap riflemenAIHeatMapVisual;
    [SerializeField] private RiflemenHeatMapVisual riflemenHeatMapVisual;
    [SerializeField] private MachineGunsHeat machinegunsHeatMapVisual;
    [SerializeField] private MachineGunsYouHeat machinegunsYouHeatMapVisual;
    [SerializeField] private MachineGunsAIHeat machinegunsAIHeatMapVisual;
    [SerializeField] private SnipersHeatMapVisual snipersHeatMapVisual;
    [SerializeField] private SniperYouHeatMapVisual snipersYouHeatMapVisual;
    [SerializeField] private SnipersAIHeatMapVisual snipersAIHeatMapVisual;
    [SerializeField] private DamageHeatMapVisual damageHeatMapVisual;
    [SerializeField] private DeathsHeatMapVisual deathsHeatMapVisual;

    // Dictionary to store grids
    private Dictionary<string, Grid> grids;

    public Grid gridSoldiers { get; private set; }
    public Grid gridSoldiersYou { get; private set; }
    public Grid gridSoldiersAI { get; private set; }
    public Grid gridRiflemen { get; private set; }
    public Grid gridRiflemenYou { get; private set; }
    public Grid gridRiflemenAI { get; private set; }
    public Grid gridMachineguns { get; private set; }
    public Grid gridMachinegunsYou { get; private set; }
    public Grid gridMachinegunsAI { get; private set; }
    public Grid gridSnipers { get; private set; }
    public Grid gridSnipersYou { get; private set; }
    public Grid gridSnipersAI { get; private set; }
    public Grid gridDamage { get; private set; }
    public Grid gridDeaths { get; private set; }

    private void Start()
    {
        grids = new Dictionary<string, Grid>(); // Initialize the dictionary
        CreateGrids();
        SetGrids();
    }

    private void CreateGrids()
    {
        InitializeGrids();
        AssignGrids();
    }

    private void InitializeGrids()
    {
        grids["Soldiers"] = new Grid(100, 100, 25f, new Vector3(1500f, 480f, 1500f));
        grids["SoldiersYou"] = new Grid(100, 100, 25f, new Vector3(1500f, 482f, 1500f));
        grids["SoldiersAI"] = new Grid(100, 100, 25f, new Vector3(1500f, 482f, 1500f));
        grids["Riflemen"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["RiflemenYou"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["RiflemenAI"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["Machineguns"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["MachinegunsYou"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["MachinegunsAI"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["Snipers"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["SnipersYou"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["SnipersAI"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["Damage"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        grids["Deaths"] = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
    }

    private void AssignGrids()
    {
        gridSoldiers = grids["Soldiers"];
        gridSoldiersYou = grids["SoldiersYou"];
        gridSoldiersAI = grids["SoldiersAI"];
        gridRiflemen = grids["Riflemen"];
        gridRiflemenYou = grids["RiflemenYou"];
        gridRiflemenAI = grids["RiflemenAI"];
        gridMachineguns = grids["Machineguns"];
        gridMachinegunsYou = grids["MachinegunsYou"];
        gridMachinegunsAI = grids["MachinegunsAI"];
        gridSnipers = grids["Snipers"];
        gridSnipersYou = grids["SnipersYou"];
        gridSnipersAI = grids["SnipersAI"];
        gridDamage = grids["Damage"];
        gridDeaths = grids["Deaths"];
    }

    private void SetGrids()
    {
        heatMapVisual.SetGrid(grids["Soldiers"]);
        soldierHeatMapVisual.SetSoldierGrid(grids["Soldiers"]);
        soldierYouHeatMapVisual.SetSoldierYouGrid(grids["SoldiersYou"]);
        soldierAIHeatMapVisual.SetSoldierGridAI(grids["SoldiersAI"]);
        riflemenHeatMapVisual.SetRiflemenGrid(grids["Riflemen"]);
        riflemenYouHeatMapVisual.SetRiflemenYouGrid(grids["RiflemenYou"]);
        riflemenAIHeatMapVisual.SetRiflemenGridAI(grids["RiflemenAI"]);
        machinegunsHeatMapVisual.SetMachinegunsGrid(grids["Machineguns"]);
        machinegunsYouHeatMapVisual.SetMachinegunsYouGrid(grids["MachinegunsYou"]);
        machinegunsAIHeatMapVisual.SetMachinegunsGridAI(grids["MachinegunsAI"]);
        snipersHeatMapVisual.SetSnipersGrid(grids["Snipers"]);
        snipersYouHeatMapVisual.SetSniperYouGrid(grids["SnipersYou"]);
        snipersAIHeatMapVisual.SetSnipersGridAI(grids["SnipersAI"]);
        damageHeatMapVisual.SetDamageGrid(grids["Damage"]);
        deathsHeatMapVisual.SetDeathsGrid(grids["Deaths"]);
    }
}