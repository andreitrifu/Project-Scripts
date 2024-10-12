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
    public void FixedUpdate()
    {
        if ((aSite.team1Taken && bSite.team1Taken) || (aSite.team1Taken && cSite.team1Taken) || (cSite.team1Taken && bSite.team1Taken))
        {
            team1Won = true;
            winImage.gameObject.SetActive(true);
        }
        if ((aSite.team2Taken && bSite.team2Taken) || (aSite.team2Taken && cSite.team2Taken) || (cSite.team2Taken && bSite.team2Taken))
        {
            team2Won = true;
            loseImage.gameObject.SetActive(true);
        }
    }
}