using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldNPC : MonoBehaviour
{

    Grid grid;
    Transform player;
    Pathfinding pathfinder;
    public float speed = 3;
    Vector3 target;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pathfinder = new Pathfinding(transform.GetInstanceID(), grid);
    }

    void Update()
    {

        // Vector3 left = Vector3.forward;
        // Vector3 right = Vector3.forward;
        // pathfinder.FindPath(transform.position, goalPosition.position, left, right);

        // List<Node> finalPath = pathfinder.FinalPath;
        // if (finalPath != null)
        // {
        //     Vector3 firstStep = finalPath[0].position;
        //     MoveTo(firstStep);
        // }

        //greedy
        Vector3 left;
        Vector3 right;
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
        Physics.Linecast(transform.position + left, player.position + left, out raycastInfoLeft);
        Physics.Linecast(transform.position + right, player.position + right, out raycastInfoRight);
        // Debug.DrawLine(transform.position + Vector3.right * grid.nodeRadius, goalPosition.position);
        // Debug.DrawLine(transform.position + Vector3.left * grid.nodeRadius, goalPosition.position);
        if ((raycastInfoLeft.collider != null && raycastInfoLeft.collider.tag == "Wall") || (raycastInfoRight.collider != null && raycastInfoRight.collider.tag == "Wall"))
        {
            pathfinder.FindPath(transform.position, player.position, left, right);

            List<Node> finalPath = pathfinder.FinalPath;
            if (finalPath != null)
            {
                Vector3 firstStep = finalPath[0].position;
                // Vector3 firstStep = finalPath.Count > 1 ? finalPath[1].position : finalPath[0].position;
                target = firstStep;
            }
        }
        else
            target = player.position;
    }

    void FixedUpdate()
    {
        MoveTo(target);
    }

    void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}
