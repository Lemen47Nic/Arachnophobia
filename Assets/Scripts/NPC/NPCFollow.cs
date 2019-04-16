using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow
{

    SharedKnowledge sharedKnowledge;
    Transform transform;

    NPCUtils utils;

    Animation animation;

    public NPCFollow(SharedKnowledge sharedKnowledge, Transform transform, NPCUtils utils)
    {
        this.sharedKnowledge = sharedKnowledge;
        this.transform = transform;

        this.utils = utils;

        animation = utils.MB.GetComponent<Animation>();
    }

    //TRIGGERS

    public bool NotSawPlayer()
    {
        foreach (KeyValuePair<int, Report> kvp in sharedKnowledge.GetReports(utils.ReportTimeValidation))
            if (Vector3.Distance(transform.position, kvp.Value.ReportingNPC.position) < utils.NpcTriggerRange)
                return false;

        return true;
    }

    // ACTIONS

    public void FollowPlayer()
    {
        utils.SeePlayer();

        Vector3 playerPosition = GetPlayerPosition();
        if (playerPosition == Vector3.zero) return;

        Vector3 target = utils.GetTarget(playerPosition);

        utils.MoveTo(target, utils.FollowSpeed);

        if (!animation.IsPlaying("Run"))
            animation.Play("Run");
    }

    Vector3 GetPlayerPosition()
    {
        Report bestReport = null;
        foreach (Report r in sharedKnowledge.GetReports(utils.ReportTimeValidation).Values)
        {
            if (bestReport == null || bestReport.ReportedTime < r.ReportedTime)
                if (Vector3.Distance(transform.position, r.ReportingNPC.position) < utils.NpcTriggerRange)
                    bestReport = r;
        }
        if (bestReport != null)
            return bestReport.ReportedPlayer.position;
        else
            return Vector3.zero;
    }

}
