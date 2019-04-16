using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    Grid grid;
    int instanceId;
    RaycastHit raycastInfoLeft, raycastInfoRight;
    List<Node> openList;
    HashSet<Node> closedList;
    Node startNode, goalNode, currentNode;
    int costOffset, sameRoadCostOffset = 0, aroundCostOffset = 5, radiusFactor = 4;
    public List<Node> FinalPath
    {
        get
        {
            if (grid.finalPaths.ContainsKey(instanceId))
                return grid.finalPaths[instanceId];
            else return null;
        }
    }

    public Pathfinding(int instanceId, Grid grid)
    {
        this.instanceId = instanceId;
        this.grid = grid;
    }

    public void FindPath(Vector3 startPos, Vector3 goalPos, Vector3 left, Vector3 right)
    {
        startNode = grid.NodeFromWorldPosition(startPos);
        goalNode = grid.NodeFromWorldPosition(goalPos);

        openList = new List<Node>();
        closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            //seleziono il nodo a costo minore
            costOffset = 0;
            selectMinCostNode();
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //greedy
            exploreDirectRoadToGoal(goalPos, left, right);
            //finegreedy

            exploreAreaAroundCurrent();

            if (currentNode == goalNode)
                getFinalPath(startPos, startNode, goalNode, left, right);
            else
                exploreNeighborNodes();
        }
    }

    void selectMinCostNode()
    {
        currentNode = openList[0];

        //seleziono quello a costo minimo tra quelli che sto visitando
        for (int i = 1; i < openList.Count; i++)
            if (openList[i].FCost <= currentNode.FCost && openList[i].hCost < currentNode.hCost)
                currentNode = openList[i];

    }

    void exploreDirectRoadToGoal(Vector3 goalPos, Vector3 left, Vector3 right)
    {
        //il raycast all'inizio si scontra con il NPC stesso, al secondo passo incontra effettivamente il primo ostacolo tra NPC e player, se non c'è nulla incontra player
        if (currentNode != startNode && currentNode != goalNode)
        {
            Physics.Linecast(currentNode.position + left, goalPos + left, out raycastInfoLeft);
            Physics.Linecast(currentNode.position + right, goalPos + right, out raycastInfoRight);
            if (raycastInfoLeft.collider != null && raycastInfoRight.collider != null)
            {
                //se vedo che tra me e il player non ci sono ostacoli, interrompo il pathfinding
                if (raycastInfoLeft.collider.tag == "Player" && raycastInfoRight.collider.tag == "Player")
                {
                    Debug.DrawLine(currentNode.position + left, goalPos + left);
                    Debug.DrawLine(currentNode.position + right, goalPos + right);
                    goalNode.parent = currentNode;
                    currentNode = goalNode;
                    openList = new List<Node>();
                }
                //se vedo che tra me e il player ci sono altri NPC, aumento il costo del path nel grafo
                else if (raycastInfoLeft.collider.tag == "NPC" || raycastInfoRight.collider.tag == "NPC")
                {
                    costOffset += sameRoadCostOffset;
                }
            }
        }
    }

    void exploreAreaAroundCurrent()
    {
        RaycastHit[] hits = Physics.SphereCastAll(currentNode.position, grid.nodeRadius * radiusFactor, Vector3.forward, grid.nodeRadius * radiusFactor);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "NPC" && hit.transform.GetInstanceID() != instanceId)
                costOffset += aroundCostOffset;
            // Debug.Log(instanceId + " hit " + hit.transform.GetInstanceID());
        }
    }

    void exploreNeighborNodes()
    {
        foreach (Node neighborNode in grid.GetNeighboringNodes(currentNode))
        {
            if (!neighborNode.isWall || closedList.Contains(neighborNode))
                continue;

            // float moveCost = currentNode.gCost + GetManhattanDistance(currentNode, neighborNode);
            float moveCost = currentNode.gCost + getEuclideanDistance(currentNode, neighborNode);

            if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
            {
                neighborNode.gCost = moveCost + costOffset;
                // neighborNode.hCost = GetManhattanDistance(neighborNode, goalNode);
                neighborNode.hCost = getEuclideanDistance(neighborNode, goalNode) + costOffset;
                neighborNode.parent = currentNode;

                if (!openList.Contains(neighborNode))
                    openList.Add(neighborNode);
            }
        }
    }

    void getFinalPath(Vector3 startPos, Node startingNode, Node endNode, Vector3 left, Vector3 right)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startingNode)
        {
            finalPath.Add(currentNode);

            //greedy
            Physics.Linecast(currentNode.position + left, startPos + left, out raycastInfoLeft);
            Physics.Linecast(currentNode.position + right, startPos + right, out raycastInfoRight);
            Debug.DrawLine(currentNode.position + left, startPos + left);
            Debug.DrawLine(currentNode.position + right, startPos + right);
            if ((raycastInfoLeft.transform != null && raycastInfoLeft.transform.GetInstanceID() == instanceId) && (raycastInfoRight.transform != null && raycastInfoRight.transform.GetInstanceID() == instanceId))
            {
                currentNode = startingNode;
            }
            else
                //finegreedy
                currentNode = currentNode.parent;
        }

        finalPath.Reverse();

        if (!grid.finalPaths.ContainsKey(instanceId))
            grid.finalPaths.Add(instanceId, finalPath);
        else
            grid.finalPaths[instanceId] = finalPath;
    }

    float GetManhattanDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }

    float getEuclideanDistance(Node nodeA, Node nodeB)
    {
        return (nodeA.position - nodeB.position).magnitude;
    }
}
