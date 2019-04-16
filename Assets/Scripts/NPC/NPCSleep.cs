using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSleep
{

    SharedKnowledge sharedKnowledge;
    Transform transform;

    NPCUtils utils;

    Animation animation;

    bool isSleeping = false;

    public NPCSleep(SharedKnowledge sharedKnowledge, Transform transform, NPCUtils utils)
    {
        this.sharedKnowledge = sharedKnowledge;
        this.transform = transform;

        this.utils = utils;

        animation = utils.MB.GetComponent<Animation>();
    }

    // TRIGGERS

    public bool NotSleeping()
    {
        return !isSleeping;
    }

    // ACTIONS

    public void Sleep()
    {
        isSleeping = true;

        if (!animation.IsPlaying("Idle"))
            animation.Play("Idle");

        utils.MB.StartCoroutine(Sleeping());
    }

    IEnumerator Sleeping()
    {
        yield return new WaitForSeconds(utils.SleepingSeconds);

        isSleeping = false;
    }
}
