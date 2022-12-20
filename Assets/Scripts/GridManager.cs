using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : Manager<GridManager>
{  
    public GameObject terrainGrid;

    private Graph graph;
    
    public List<Tile> allTiles = new List<Tile>();
    protected void Awake()
    {
        base.Awake();
        allTiles = terrainGrid.GetComponentsInChildren<Tile>().ToList();
        
        InitializeGraph();
    }

    private void InitializeGraph()
    {
        graph = new Graph();

        for (int i = 0; i < allTiles.Count; i++)
        {
            Vector3 place = allTiles[i].transform.position;
            graph.AddNode(place);
        }

        var allNodes = graph.Nodes;
        foreach (Node from in allNodes)
        {
            foreach (Node to in allNodes)
            {
                if (Vector3.Distance(from.worldPosition, to.worldPosition) < 11f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }
    public Node GetNodeFromTile(Tile t)
    {
        var allNodes = graph.Nodes;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (t.transform.GetSiblingIndex() == allNodes[i].index)
            {
                return allNodes[i];
            }
        }

        return null;
    }
    
    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }
    
    public List<Node> GetPath(Node from, Node to)
    {
        return graph.GetShortestPath(from, to);
    }



    public Node GetFreeNode(Team forTeam)
    {
        List<Node> enemySpawnPositions = new List<Node>();
        foreach (var node in graph.Nodes)
        {
            if (node.worldPosition.x < -15)
            {
                enemySpawnPositions.Add(node);
            }
        }

        int startIndex = 0;
        int currentIndex = startIndex;

        while(enemySpawnPositions[currentIndex].IsOccupied)
        {
            if(startIndex == 0)
            {
                currentIndex++;
                if (currentIndex == enemySpawnPositions.Count)
                    return null;
            }
            else
            {
                currentIndex--;
                if (currentIndex == -1)
                    return null;
            }
            
        }
        return enemySpawnPositions[currentIndex];
    }
}