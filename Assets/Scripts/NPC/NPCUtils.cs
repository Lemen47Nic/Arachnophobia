using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUtils
{

    SharedKnowledge sharedKnowledge;
    Transform transform;


    MonoBehaviour mb;
    public MonoBehaviour MB
    {
        get
        {
            return mb;
        }
    }
    Grid grid;
    public Grid Grid
    {
        get
        {
            return grid;
        }
    }
    Pathfinding pathfinder;
    public Pathfinding Pathfinder
    {
        get
        {
            return pathfinder;
        }
    }
    string targetTag;
    public string TargetTag
    {
        get
        {
            return targetTag;
        }
    }
    float goalTriggerRange;
    public float GoalTriggerRange
    {
        get
        {
            return goalTriggerRange;
        }
    }
    float npcTriggerRange;
    public float NpcTriggerRange
    {
        get
        {
            return npcTriggerRange;
        }
    }
    double reportTimeValidation;
    public double ReportTimeValidation
    {
        get
        {
            return reportTimeValidation;
        }
    }
    int buildingSeconds;
    public int BuildingSeconds
    {
        get
        {
            return buildingSeconds;
        }
    }
    int sleepingSeconds;
    public int SleepingSeconds
    {
        get
        {
            return sleepingSeconds;
        }
    }
    float followSpeed;
    public float FollowSpeed
    {
        get
        {
            return followSpeed;
        }
    }
    float randomSpeed;
    public float RandomSpeed
    {
        get
        {
            return randomSpeed;
        }
    }
    GameObject wallPrefab;
    public GameObject WallPrefab
    {
        get
        {
            return wallPrefab;
        }
    }

    // CONSTRUCTOR

    public NPCUtils(MonoBehaviour mb, SharedKnowledge sharedKnowledge, Transform transform, Grid grid, string targetTag, float goalTriggerRange, float npcTriggerRange, double reportTimeValidation, int buildingSeconds, int sleepingSeconds, float followSpeed, float randomSpeed, GameObject wallPrefab)
    {
        this.mb = mb;
        this.sharedKnowledge = sharedKnowledge;
        this.transform = transform;

        this.grid = grid;
        pathfinder = new Pathfinding(transform.GetInstanceID(), grid);

        this.targetTag = targetTag;
        this.goalTriggerRange = goalTriggerRange;
        this.npcTriggerRange = npcTriggerRange;
        this.reportTimeValidation = reportTimeValidation;
        this.buildingSeconds = buildingSeconds;
        this.sleepingSeconds = sleepingSeconds;

        this.followSpeed = followSpeed;
        this.randomSpeed = randomSpeed;

        this.wallPrefab = wallPrefab;
    }

    // METHODS

    public bool SeePlayer()
    {
        RaycastHit raycastInfo;
        Physics.Linecast(transform.position, sharedKnowledge.Player.position, out raycastInfo);
        // Debug.DrawLine(transform.position, goalPosition.position);
        if (raycastInfo.collider != null && raycastInfo.collider.tag == targetTag && raycastInfo.distance < goalTriggerRange)
        {
            ReportPlayer();
            return true;
        }
        return false;
    }

    public void ReportPlayer()
    {
        sharedKnowledge.Report(transform);
        // Debug.Log(transform.GetInstanceID() + " report");
    }

    public Vector3 GetTarget(Vector3 playerPosition)
    {
        Vector3 target = new Vector3();

        //greedy
        Vector3 left, right;
        if (transform.forward.x >= 0.5 || transform.forward.x <= -0.5)
        {
            left = Vector3.forward * grid.nodeRadius;
            right = Vector3.back * grid.nodeRadius;
        }
        else
        {
            left = Vector3.left * grid.nodeRadius;
            right = Vector3.right * grid.nodeRadius;
        }

        RaycastHit raycastInfoLeft, raycastInfoRight;
        Physics.Linecast(transform.position + left, playerPosition + left, out raycastInfoLeft);
        Physics.Linecast(transform.position + right, playerPosition + right, out raycastInfoRight);
        // Debug.DrawLine(transform.position + Vector3.right * grid.nodeRadius, goalPosition.position);
        // Debug.DrawLine(transform.position + Vector3.left * grid.nodeRadius, goalPosition.position);
        if ((raycastInfoLeft.collider != null && (raycastInfoLeft.collider.tag == "Wall" || raycastInfoLeft.collider.tag == "NPC"))
            || (raycastInfoRight.collider != null && (raycastInfoRight.collider.tag == "Wall" || raycastInfoRight.collider.tag == "NPC")))
        {
            pathfinder.FindPath(transform.position, playerPosition, left, right);

            List<Node> finalPath = pathfinder.FinalPath;
            if (finalPath != null && finalPath.Count > 0)
            {
                Vector3 firstStep = finalPath[0].position;
                // Vector3 firstStep = finalPath.Count > 1 ? finalPath[1].position : finalPath[0].position;
                target = firstStep;
            }
        }
        else
            target = playerPosition;

        return target;
    }

    public void MoveTo(Vector3 destination, float speed)
    {
        if ((destination - transform.position) == Vector3.zero)
            return;
        Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // Vector3 verticalAdj = new Vector3(destination.x, transform.position.y, destination.z);
        // Vector3 toDestination = verticalAdj - transform.position;
        // transform.LookAt(verticalAdj);
        // Vector3 rotagionAdj = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        // transform.rotation = Quaternion.Euler(rotagionAdj);

        // transform.position += transform.forward * speed * Time.deltaTime;
        // rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
}
