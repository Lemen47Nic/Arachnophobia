using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Report
{
    DateTime reportedTime;
    Transform reportedPlayerPosition;
    Transform reportingNPCPosition;

    public DateTime ReportedTime
    {
        get
        {
            return reportedTime;
        }
    }

    public Transform ReportedPlayer
    {
        get
        {
            return reportedPlayerPosition;
        }
    }

    public Transform ReportingNPC
    {
        get
        {
            return reportingNPCPosition;
        }
    }

    public Report(DateTime reportedTime, Transform reportedPlayer, Transform reportingNPC)
    {
        this.reportedTime = reportedTime;
        //se salvo il transform, si aggiorna in automatico con i movimenti
        //se salvo il Vector3 rimane fisso
        this.reportedPlayerPosition = reportedPlayer;
        this.reportingNPCPosition = reportingNPC;
    }
}
