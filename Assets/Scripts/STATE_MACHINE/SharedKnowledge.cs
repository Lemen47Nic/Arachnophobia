using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedKnowledge
{
    private static SharedKnowledge instance = null;
    public static SharedKnowledge SharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new SharedKnowledge();
            }
            return instance;
        }
    }

    Transform player;
    public Transform Player
    {
        get
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").transform;
            return player;
        }
    }

    Dictionary<int, Report> reports = new Dictionary<int, Report>();

    public void Report(Transform reportingNPC)
    {
        if (!reports.ContainsKey(reportingNPC.GetInstanceID()))
            reports.Add(reportingNPC.GetInstanceID(), new Report(DateTime.Now, player, reportingNPC));
        else
            reports[reportingNPC.GetInstanceID()] = new Report(DateTime.Now, player, reportingNPC);
    }

    public Dictionary<int, Report> GetReports(double reportTimeValidation)
    {
        List<int> toRemove = new List<int>();

        foreach (KeyValuePair<int, Report> kvp in reports)
        {
            if ((DateTime.Now - kvp.Value.ReportedTime).TotalMilliseconds > reportTimeValidation)
                toRemove.Add(kvp.Key);
        }

        foreach (var key in toRemove)
        {
            reports.Remove(key);
        }

        return reports;
    }

    public void ResetInstance()
    {
        instance = null;
    }
}
