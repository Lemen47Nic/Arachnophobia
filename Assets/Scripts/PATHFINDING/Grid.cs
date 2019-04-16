using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    Node[,] grid;
    // public List<Node> finalPath;
    public Dictionary<int, List<Node>> finalPaths = new Dictionary<int, List<Node>>();

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    // Use this for initialization
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2 - Vector3.up * transform.position.y;
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool wall = true;
                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                    wall = false;

                grid[x, y] = new Node(wall, worldPoint, x, y);
            }
    }

    public void UpdateGrid()
    {
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2 - Vector3.up * transform.position.y;
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool wall = true;
                if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                    wall = false;

                grid[x, y] = new Node(wall, worldPoint, x, y);
            }
    }

    public List<Node> GetNeighboringNodes(Node node)
    {
        List<Node> neighboringNodes = new List<Node>();
        int xCheck, yCheck;

        xCheck = node.gridX + 1;
        yCheck = node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
            if (yCheck >= 0 && yCheck < gridSizeY)
                neighboringNodes.Add(grid[xCheck, yCheck]);

        xCheck = node.gridX - 1;
        yCheck = node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
            if (yCheck >= 0 && yCheck < gridSizeY)
                neighboringNodes.Add(grid[xCheck, yCheck]);

        xCheck = node.gridX;
        yCheck = node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
            if (yCheck >= 0 && yCheck < gridSizeY)
                neighboringNodes.Add(grid[xCheck, yCheck]);

        xCheck = node.gridX;
        yCheck = node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
            if (yCheck >= 0 && yCheck < gridSizeY)
                neighboringNodes.Add(grid[xCheck, yCheck]);

        return neighboringNodes;
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float xPoint = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    public Vector3 GetRandomPosition(Vector3 npcPosition, float npcTriggerRange)
    {
        Node n;
        do
        {
            int x = Random.Range(0, gridSizeX);
            int y = Random.Range(0, gridSizeY);
            n = grid[x, y];
        } while (!n.isWall && Vector3.Distance(npcPosition, n.position) > npcTriggerRange);
        Vector3 position = new Vector3(n.position.x, 0, n.position.z);
        return position;
    }

    // function that draws the wireframe
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
            foreach (Node n in grid)
            {
                if (n.isWall)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.yellow;

                foreach (KeyValuePair<int, List<Node>> finalPath in finalPaths)
                {
                    if (finalPath.Value != null)
                        if (finalPath.Value.Contains(n))
                            Gizmos.color = Color.blue;
                }

                Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distance));//Draw the node at the position of the node.
            }
    }
}