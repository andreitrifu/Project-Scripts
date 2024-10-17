using System.Collections;//
using System.Collections.Generic;
using UnityEngine;
using Utils.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private SoldierHeatMapVisual soldierHeatMapVisual;
    [SerializeField] private SoldierHeatMapYouVisual soldierYouHeatMapVisual;
    [SerializeField] private SoldierHeatMapAIVisual soldierAIHeatMapVisual;
    [SerializeField] private RiflemenYouHeatMapVisual riflemenYouHeatMapVisual;
    [SerializeField] private RiflemenAIHeatMapVisual riflemenAIHeatMapVisual;
    [SerializeField] private RiflemenHeatMapVisual riflemenHeatMapVisual;
    [SerializeField] private MachineGunsHeat machinegunsHeatMapVisual;
    [SerializeField] private MachineGunsYouHeat machinegunsYouHeatMapVisual;
    [SerializeField] private MachineGunsAIHeat machinegunsAIHeatMapVisual;
    [SerializeField] private SnipersHeatMapVisual snipersHeatMapVisual;
    [SerializeField] private SnipersYouHeatMapVisual snipersYouHeatMapVisual;
    [SerializeField] private SnipersAIHeatMapVisual snipersAIHeatMapVisual;
    [SerializeField] private DamageHeatMapVisual damageHeatMapVisual;
    [SerializeField] private DeathsHeatMapVisual deathsHeatMapVisual;
    public Grid grid { get; private set; } // Make it a property
    public Grid gridSoldiers { get; private set; }
    public Grid gridSoldiersYou { get; private set; }
    public Grid gridSoldiersAI { get; private set; }
    public Grid gridRiflemen { get; private set; }
    public Grid gridRiflemenYou { get; private set; }
    public Grid gridRiflemenAI { get; private set; }
    public Grid gridMachineguns { get; private set; }
    public Grid gridMachinegunsYou { get; private set; }
    public Grid gridMachinegunsAI { get; private set; }
    public Grid gridSnipers{ get; private set; }
    public Grid gridSnipersYou { get; private set; }
    public Grid gridSnipersAI { get; private set; }
    public Grid gridDamage { get; private set; }
    public Grid gridDeaths { get; private set; }

    private void Start()
    {
        grid = new Grid(100, 100, 25f, new Vector3(1500f, 480f, 1500f));
        gridSoldiers = new Grid(100, 100, 25f, new Vector3(1500f, 482f, 1500f));
        gridSoldiersYou = new Grid(100, 100, 25f, new Vector3(1500f, 482f, 1500f));
        gridSoldiersAI = new Grid(100, 100, 25f, new Vector3(1500f, 482f, 1500f));
        gridRiflemen = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridRiflemenYou = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridRiflemenAI= new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridMachineguns = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridMachinegunsYou = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridMachinegunsAI = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridSnipers = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridSnipersYou = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridSnipersAI = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        gridDamage = new Grid(100, 100, 25f, new Vector3(1500f, 485f, 1500f));
        heatMapVisual.SetGrid(grid);
        soldierHeatMapVisual.SetSoldierGrid(gridSoldiers);
        soldierYouHeatMapVisual.SetSoldierGridYou(gridSoldiersYou);
        soldierAIHeatMapVisual.SetSoldierGridAI(gridSoldiersAI);
        riflemenHeatMapVisual.SetRiflemenGrid(gridRiflemen);
        riflemenYouHeatMapVisual.SetRiflemenGridYou(gridRiflemenYou);
        riflemenAIHeatMapVisual.SetRiflemenGridAI(gridRiflemenAI);
        machinegunsHeatMapVisual.SetMachinegunsGrid(gridMachineguns);
        machinegunsYouHeatMapVisual.SetMachinegunsGridYou(gridMachinegunsYou);
        machinegunsAIHeatMapVisual.SetMachinegunsGridAI(gridMachinegunsAI);
        snipersHeatMapVisual.SetSnipersGrid(gridSnipers);
        snipersYouHeatMapVisual.SetSnipersGridYou(gridSnipersYou);
        snipersAIHeatMapVisual.SetSnipersGridAI(gridSnipersAI);
        damageHeatMapVisual.SetDamageGrid(gridDamage);
        deathsHeatMapVisual.SetDeathsGrid(gridDeaths);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            grid.AddValue(position, 20, 5, 10);
        }
    }
}