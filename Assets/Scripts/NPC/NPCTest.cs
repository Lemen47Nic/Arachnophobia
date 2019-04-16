using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTest : MonoBehaviour
{
    Transform player;
    private string targetTag = "Player";
    public float triggerRange = 5f, reactionTime = 3f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    void Update()
    {
        if (SeePlayer())
            Debug.Log("visto");
        else
            Debug.Log("dov'è?");
    }

    public bool SeePlayer()
    {
        RaycastHit raycastInfo;
        Physics.Linecast(transform.position, player.position, out raycastInfo);
        // Debug.DrawLine(transform.position, goalPosition.position);
        if (raycastInfo.collider != null && raycastInfo.collider.tag == targetTag && raycastInfo.distance < triggerRange)
            return true;
        return false;
    }
}
