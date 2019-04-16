using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuild
{

    SharedKnowledge sharedKnowledge;
    Transform transform;

    NPCUtils utils;

    Animation animation;

    bool isBuilding = false;
    float wallTime = 15f;

    public NPCBuild(SharedKnowledge sharedKnowledge, Transform transform, NPCUtils utils)
    {
        this.sharedKnowledge = sharedKnowledge;
        this.transform = transform;

        this.utils = utils;

        animation = utils.MB.GetComponent<Animation>();
    }

    // TRIGGERS

    public bool NotBuilding()
    {
        return !isBuilding;
    }

    // ACTIONS

    public void BuildWall()
    {
        isBuilding = true;

        if (!animation.IsPlaying("Death"))
            animation.Play("Death");

        Vector3 wallPosition = transform.position + transform.forward * 1f - transform.right * 1;

        for (int x = 0; x < 3; x++)
        {
            Vector3 tempPoint = wallPosition + transform.right * x;

            RaycastHit[] hits = Physics.SphereCastAll(tempPoint, utils.Grid.nodeRadius, Vector3.forward, utils.Grid.nodeRadius);
            bool hitOther = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "NPC" && hit.transform.GetInstanceID() != transform.GetInstanceID() || hit.collider.tag == utils.TargetTag)
                    hitOther = true;
            }

            if (!hitOther)
            {
                var wall = GameObject.Instantiate(
                        utils.WallPrefab,
                        tempPoint,
                        transform.rotation);
                UnityEngine.Object.Destroy(wall, wallTime);
            }
        }

        utils.Grid.UpdateGrid();

        utils.MB.StartCoroutine(Building());

        utils.MB.StartCoroutine(ReUpdateGrid());
    }

    IEnumerator Building()
    {
        yield return new WaitForSeconds(utils.BuildingSeconds);

        isBuilding = false;
    }

    IEnumerator ReUpdateGrid()
    {
        yield return new WaitForSeconds(wallTime);

        utils.Grid.UpdateGrid();
    }
}
