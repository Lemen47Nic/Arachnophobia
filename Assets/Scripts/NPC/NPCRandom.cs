using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRandom
{

    SharedKnowledge sharedKnowledge;
    Transform transform;

    NPCUtils utils;

    Vector3 randomPosition;
    DateTime randomTimeSaved;
    double randomTimeTravel;

    Animation animation;

    public NPCRandom(SharedKnowledge sharedKnowledge, Transform transform, NPCUtils utils)
    {
        this.sharedKnowledge = sharedKnowledge;
        this.transform = transform;

        this.utils = utils;

        randomPosition = transform.position;
        randomTimeSaved = DateTime.Now;
        randomTimeTravel = (utils.ReportTimeValidation * 1.5) * UnityEngine.Random.Range(0f, 1f);
        double subtract = (utils.ReportTimeValidation * 1.5) * UnityEngine.Random.Range(0f, 1f);
        randomTimeSaved = randomTimeSaved.AddMilliseconds(-subtract);

        animation = utils.MB.GetComponent<Animation>();
    }

    //TRIGGERS

    public bool SawPlayer()
    {
        foreach (Report r in sharedKnowledge.GetReports(utils.ReportTimeValidation).Values)
            if (Vector3.Distance(this.transform.position, r.ReportingNPC.position) < utils.NpcTriggerRange)
                return true;

        return false;
    }

    //ACTIONS

    public void RandomPath()
    {
        Vector3 randomPosition = GetRandomPosition();

        Vector3 target = utils.GetTarget(randomPosition);

        utils.MoveTo(target, utils.RandomSpeed);

        if (!animation.IsPlaying("Walk"))
            animation.Play("Walk");
    }

    Vector3 GetRandomPosition()
    {
        if ((DateTime.Now - randomTimeSaved).TotalMilliseconds > randomTimeTravel)
        {
            randomPosition = utils.Grid.GetRandomPosition(transform.position, utils.NpcTriggerRange);
            randomTimeSaved = DateTime.Now;
            randomTimeTravel = (utils.ReportTimeValidation * 1.5) * UnityEngine.Random.Range(0f, 1f);
        }

        return randomPosition;
    }


}
