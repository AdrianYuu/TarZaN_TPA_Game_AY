using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneratePath : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject point;
    [SerializeField] private GameObject path;
    [SerializeField] private GameObject optimumPath;

    private Node[,] gridMap;
    private const int xSize = 100;
    private const int zSize = 100;

    private int pointCount = 4;
    private float mapScale = 10;
    private bool isHorizontal;
    private bool isOptimum;

    private Vector3 northEntryPoint = new Vector3(49f, 0.001f, 99f); // Utara
    private Vector3 southEntryPoint = new Vector3(49f, 0.001f, 1f); // Selatan
    private Vector3 eastEntryPoint = new Vector3(99f, 0.001f, 49f); // Timur
    private Vector3 westEntryPoint = new Vector3(1f, 0.001f, 49f); // Barat

    private Vector3 northExitPoint = new Vector3(50f, 0.001f, 51f); // Utara
    private Vector3 southExitPoint = new Vector3(50f, 0.001f, 49f); // Selatan
    private Vector3 eastExitPoint = new Vector3(51f, 0.001f, 50f); // Timur
    private Vector3 westExitPoint = new Vector3(49f, 0.001f, 50f); // Barat

    // Random Points
    private List<Vector3> northPoints = new List<Vector3>();
    private List<Vector3> southPoints = new List<Vector3>();
    private List<Vector3> eastPoints = new List<Vector3>();
    private List<Vector3> westPoints = new List<Vector3>();

    // Path
    private List<Vector3> northPaths = new List<Vector3>();
    private List<Vector3> southPaths = new List<Vector3>();
    private List<Vector3> eastPaths = new List<Vector3>();
    private List<Vector3> westPaths = new List<Vector3>();

    // Path List
    public List<Vector3> optimumNorthPaths = new List<Vector3>();
    public List<Vector3> optimumSouthPaths = new List<Vector3>();
    public List<Vector3> optimumEastPaths = new List<Vector3>();
    public List<Vector3> optimumWestPaths = new List<Vector3>();
    

    private void Awake()
    {
        // GenerateNorthRandomPoints();
        // GenerateSouthRandomPoints();
        // GenerateEastRandomPoints();
        // GenerateWestRandomPoints();

        // Draw(northPoints);
        // Draw(southPoints);
        // Draw(westPoints);
        // Draw(eastPoints);

        GenerateNorthPath();
        GenerateSouthPath();
        GenerateEastPath();
        GenerateWestPath();

        GenerateNorthOptimumPath();
        GenerateSouthOptimumPath();
        GenerateEastOptimumPath();
        GenerateWestOptimumPath();
    }

    private void Draw(List<Vector3> positions)
    {
        foreach(Vector3 pos in positions)
        {
            DrawPoint(pos);
        }
    }

    private void DrawPoint(Vector3 position)
    {
        Instantiate(point, position * mapScale, Quaternion.identity);
    }

    private void DrawPath(Vector3 position)
    {
        Instantiate(path, position * mapScale, Quaternion.identity);
    }

    private void DrawOptimumPath(Vector3 position)
    {
        // Instantiate(optimumPath, position * mapScale, Quaternion.identity);
    }

    private void GenerateNorthPath()
    {
        GenerateNorthRandomPoints();
        GridInit();
        isHorizontal = false;

        for(int i = 0; i < pointCount + 1; i++)
        {
            Node src = GetNodeFromPosition(northPoints[i]);
            Node dst = GetNodeFromPosition(northPoints[i + 1]);

            List<Vector3> pathList = FindPath(src, dst);

            foreach(Vector3 path in pathList)
            {
                northPaths.Add(path);
                DrawPath(new Vector3(path.x, 0.001f, path.z));
            }
        }
    }

    private void GenerateSouthPath()
    {
        GenerateSouthRandomPoints();
        GridInit();
        isHorizontal = false;

        for(int i = 0; i < pointCount + 1; i++)
        {
            Node src = GetNodeFromPosition(southPoints[i]);
            Node dst = GetNodeFromPosition(southPoints[i + 1]);

            List<Vector3> pathList = FindPath(src, dst);

            foreach(Vector3 path in pathList)
            {
                southPaths.Add(path);
                DrawPath(new Vector3(path.x, 0.001f, path.z));
            }
        }
    }

    private void GenerateEastPath()
    {
        GenerateEastRandomPoints();
        GridInit();
        isHorizontal = true;

        for(int i = 0; i < pointCount + 1; i++)
        {
            Node src = GetNodeFromPosition(eastPoints[i]);
            Node dst = GetNodeFromPosition(eastPoints[i + 1]);

            List<Vector3> pathList = FindPath(src, dst);

            foreach(Vector3 path in pathList)
            {
                eastPaths.Add(path);
                DrawPath(new Vector3(path.x, 0.001f, path.z));
            }
        }
    }

    private void GenerateWestPath()
    {
        GenerateWestRandomPoints();
        GridInit();
        isHorizontal = true;

        for(int i = 0; i < pointCount + 1; i++)
        {
            Node src = GetNodeFromPosition(westPoints[i]);
            Node dst = GetNodeFromPosition(westPoints[i + 1]);

            List<Vector3> pathList = FindPath(src, dst);

            foreach(Vector3 path in pathList)
            {
                westPaths.Add(path);
                DrawPath(new Vector3(path.x, 0.001f, path.z));
            }
        }
    }

    private void GenerateNorthOptimumPath()
    {
        GridInitAvailable(northPaths);

        Node src = GetNodeFromPosition(northPoints[0]);
        Node dst = GetNodeFromPosition(northPoints[5]);

        optimumNorthPaths = FindOptimumPath(src, dst);

        foreach(Vector3 path in optimumNorthPaths)
        {
            DrawOptimumPath(new Vector3(path.x, 0.001f, path.z));
        }
    }

    private void GenerateSouthOptimumPath()
    {
        GridInitAvailable(southPaths);

        Node src = GetNodeFromPosition(southPoints[0]);
        Node dst = GetNodeFromPosition(southPoints[5]);

        optimumSouthPaths = FindOptimumPath(src, dst);

        foreach(Vector3 path in optimumSouthPaths)
        {
            DrawOptimumPath(new Vector3(path.x, 0.001f, path.z));
        }
    }

    private void GenerateEastOptimumPath()
    {
        GridInitAvailable(eastPaths);

        Node src = GetNodeFromPosition(eastPoints[0]);
        Node dst = GetNodeFromPosition(eastPoints[5]);

        optimumEastPaths = FindOptimumPath(src, dst);

        foreach(Vector3 path in optimumEastPaths)
        {
            DrawOptimumPath(new Vector3(path.x, 0.001f, path.z));
        }
    }

    private void GenerateWestOptimumPath()
    {
        GridInitAvailable(westPaths);

        Node src = GetNodeFromPosition(westPoints[0]);
        Node dst = GetNodeFromPosition(westPoints[5]);

        optimumWestPaths = FindOptimumPath(src, dst);

        foreach(Vector3 path in optimumWestPaths)
        {
            DrawOptimumPath(new Vector3(path.x, 0.001f, path.z));
        }
    }

    private void GenerateNorthRandomPoints()
    {
        northPoints.Add(northEntryPoint);

        for(int i = 0; i < pointCount; i++)
        {
            int z = Random.Range(60, 90);
            int x = Random.Range(100 - z, z);
            northPoints.Add(new Vector3(x, 0.001f, z));
        }

        northPoints.Add(northExitPoint);
    }

    private void GenerateSouthRandomPoints()
    {
        southPoints.Add(southEntryPoint);

        for(int i = 0; i < pointCount; i++)
        {
            int z = Random.Range(10, 40); 
            int x = Random.Range(z, 100 - z);
            southPoints.Add(new Vector3(x, 0.001f, z));
        }

        southPoints.Add(southExitPoint);
    }

    private void GenerateEastRandomPoints()
    {
        eastPoints.Add(eastEntryPoint);

        for(int i = 0; i < pointCount; i++)
        {
            int x = Random.Range(60, 90);
            int z = Random.Range(100 - x, x);
            eastPoints.Add(new Vector3(x, 0.001f, z));
        }

        eastPoints.Add(eastExitPoint);
    }

    private void GenerateWestRandomPoints()
    {
        westPoints.Add(westEntryPoint);

        for(int i = 0; i < pointCount; i++)
        {
            int x = Random.Range(10, 40);
            int z = Random.Range(0, 100 - x);
            westPoints.Add(new Vector3(x, 0.001f, z));
        }

        westPoints.Add(westExitPoint);
    }

    private void GridInitAvailable(List<Vector3> currPaths)
    {
        gridMap = new Node[xSize, zSize];

        for(int x = 0; x < xSize; x++)
        {
            for(int z = 0; z < zSize; z++)
            {
                gridMap[x, z] = new Node(new Vector3(x, 0.001f, z));
                foreach(Vector3 currPath in currPaths)
                {
                    if(currPath == gridMap[x, z].position)
                    {
                        gridMap[x, z].isAvailable = true;
                        break;
                    }
                }
            }
        }
    }

    private void GridInit()
    {
        gridMap = new Node[xSize, zSize];

        for(int x = 0; x < xSize; x++)
        {
            for(int z = 0; z < zSize; z++)
            {
                gridMap[x, z] = new Node(new Vector3(x, 0.001f, z));
            }
        }
    }

    private Node GetNodeFromPosition(Vector3 position)
    {
        return gridMap[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z)];
    }

    private List<Vector3> FindPath(Node src, Node dst)
    {
        List<Vector3> path = new List<Vector3>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        isOptimum = false;

        openList.Add(src);

        while(openList.Count > 0)
        {
            Node currNode = openList[0];

            for(int i = 0; i < openList.Count; i++)
            {
                if(openList[i].fCost < currNode.fCost)
                {
                    currNode = openList[i];
                }
            } 

            openList.Remove(currNode);
            closedList.Add(currNode);

            if(currNode == dst)
            {
                path = BackTrack(src, dst);
                break;
            }

            foreach(Node neighbour in GetNeighboursList(currNode))
            {
                if(closedList.Contains(neighbour))
                {
                    continue;
                }

                float newGCost = currNode.gCost + GetDistance(currNode, neighbour);

                if(!openList.Contains(neighbour) || newGCost < neighbour.gCost)
                {
                    neighbour.gCost = newGCost;
                    neighbour.hCost = GetDistance(neighbour, dst);
                    neighbour.parent = currNode;

                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return path;
    }

    private List<Vector3> FindOptimumPath(Node src, Node dst)
    {
        List<Vector3> path = new List<Vector3>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        isOptimum = true;

        openList.Add(src);

        while(openList.Count > 0)
        {
            Node currNode = openList[0];

            for(int i = 0; i < openList.Count; i++)
            {
                if(openList[i].fCost < currNode.fCost)
                {
                    if(openList[i].isAvailable && currNode.isAvailable)
                    {
                        currNode = openList[i];
                    }
                }
            }

            openList.Remove(currNode);
            closedList.Add(currNode);

            if(currNode == dst)
            {
                path = BackTrack(src, dst);
            }

            foreach(Node neighbour in GetNeighboursList(currNode))
            {
                if(closedList.Contains(neighbour))
                {
                    continue;
                }

                float newGCost = currNode.gCost + GetDistance(currNode, neighbour);

                if(!openList.Contains(neighbour) || newGCost < neighbour.gCost)
                {
                    neighbour.gCost = newGCost;
                    neighbour.hCost = GetDistance(neighbour, dst);
                    neighbour.parent = currNode;

                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return path;
    }

    private List<Vector3> BackTrack(Node src, Node dst)
    {
        List<Vector3> path = new List<Vector3>();

        Node currNode = dst;

        while(currNode != null && currNode != src)
        {
            path.Add(currNode.position);
            currNode = currNode.parent;
        }

        if(currNode == src)
        {
            path.Add(currNode.position);
        }

        path.Reverse();

        return path;
    }

    private List<Node> GetNeighboursList(Node curr)
    {
        int oldX = Mathf.FloorToInt(curr.position.x);
        int oldZ = Mathf.FloorToInt(curr.position.z);

        List<Node> neighbourList = new List<Node>();

        Vector3[] nodeDirections1 = 
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0)
        };

        Vector3[] nodeDirections2 = 
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };

        if(isHorizontal)
        {
            foreach(Vector3 nodeDirection in nodeDirections1)
            {
                int newX = oldX + Mathf.FloorToInt(nodeDirection.x);
                int newZ = oldZ + Mathf.FloorToInt(nodeDirection.z);

                if(isOptimum)
                {
                    if(newX >= 0 && newX < xSize && newZ >= 0 && newZ < zSize && gridMap[newX, newZ].isAvailable)
                    {
                        neighbourList.Add(gridMap[newX, newZ]);
                    }
                } 
                else
                {
                    if(newX >= 0 && newX < xSize && newZ >= 0 && newZ < zSize)
                    {
                        neighbourList.Add(gridMap[newX, newZ]);
                    }
                }
            }
        }
        else
        {
            foreach(Vector3 nodeDirection in nodeDirections2)
            {
                int newX = oldX + Mathf.FloorToInt(nodeDirection.x);
                int newZ = oldZ + Mathf.FloorToInt(nodeDirection.z);

                if(isOptimum)
                {
                    if(newX >= 0 && newX < xSize && newZ >= 0 && newZ < zSize && gridMap[newX, newZ].isAvailable)
                    {
                        neighbourList.Add(gridMap[newX, newZ]);
                    }
                } 
                else
                {
                    if(newX >= 0 && newX < xSize && newZ >= 0 && newZ < zSize)
                    {
                        neighbourList.Add(gridMap[newX, newZ]);
                    }
                }
            }
        }

        return neighbourList;
    }

    private float GetDistance(Node src, Node dst)
    {
        float xDistance = Mathf.Abs(src.position.x - dst.position.x);
        float zDistance = Mathf.Abs(src.position.z - dst.position.z);
        return Mathf.Abs(xDistance + zDistance);
    }
}
