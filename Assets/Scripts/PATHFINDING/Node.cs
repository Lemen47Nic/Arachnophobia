using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX, gridY; //x and y position in the Node array

    public bool isWall; //Tells if the node is obstructed

    public Vector3 position; //The world position of the node

    public Node parent; //Will store what node it previously came from so it can trace the shortest path

    public float gCost, hCost; // gCost is the cost to moving to next square, hCost is the distance to the goal from this node

    public float FCost { get { return gCost + hCost; } }

    public Node(bool isWall, Vector3 position, int gridX, int gridY)
    {
        this.isWall = isWall;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
