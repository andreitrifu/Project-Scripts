using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreasManager : MonoBehaviour
{
    public ControlArea aSite;
    public ControlArea bSite;
    public ControlArea cSite;
    public bool team1Won = false;
    public bool team2Won = false;
    public Image winImage;
    public Image loseImage;

    private void FixedUpdate()
    {
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (IsTeam1Victory())
        {
            team1Won = true;
            winImage.gameObject.SetActive(true);
        }
        else if (IsTeam2Victory())
        {
            team2Won = true;
            loseImage.gameObject.SetActive(true);
        }
    }

    private bool IsTeam1Victory()
    {
        return (aSite.team1Taken && bSite.team1Taken) || (aSite.team1Taken && cSite.team1Taken) || (cSite.team1Taken && bSite.team1Taken);
    }

    private bool IsTeam2Victory()
    {
        return (aSite.team2Taken && bSite.team2Taken) || (aSite.team2Taken && cSite.team2Taken) || (cSite.team2Taken && bSite.team2Taken);
    }
}
