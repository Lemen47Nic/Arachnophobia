using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(player.position);
        // così va quasi bene se vogliamo un gioco in prima persona
        // transform.position = player.position;

        // così otteniamo la camera che ti segue in terza persona
        transform.position = player.position + offset;
    }
}
