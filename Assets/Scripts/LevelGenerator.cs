using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject pillarPrefab;
    public GameObject[] workstations;
    int cap = 10;
    public int numberOfWorkstations;
    public GameObject[] obstacles;
    public int numberOfObstacles;

    int[,] grid = new int[9, 9]; // 9x9 grid
    int gridOffset = 4; // Align grid with world positions

    Vector2Int playerStart = new Vector2Int(4, 0); // Player always starts here
    List<Vector2Int> workstationPositions = new List<Vector2Int>(); // Stores workstation positions

    Vector3Int[] allowedWorkstationAndObstaclePositions = {
        new Vector3Int(-2, -3, 0), new Vector3Int(-1, -3, 0), new Vector3Int(1, -3, 0), new Vector3Int(2, -3, 0),
        new Vector3Int(-3, -2, 0), new Vector3Int(-3, -1, 0), new Vector3Int(0, -2, 0), new Vector3Int(0, -1, 0),
        new Vector3Int(3, -2, 0), new Vector3Int(3, -1, 0), new Vector3Int(-2, 0, 0), new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0), new Vector3Int(2, 0, 0), new Vector3Int(-3, 1, 0), new Vector3Int(-3, 2, 0),
        new Vector3Int(0, 1, 0), new Vector3Int(0, 2, 0), new Vector3Int(3, 1, 0), new Vector3Int(3, 2, 0),
        new Vector3Int(-2, 3, 0), new Vector3Int(-1, 3, 0), new Vector3Int(1, 3, 0), new Vector3Int(2, 3, 0)
    };

    void Start()
    {
        InitializeGrid();
        GenerateWorkstations();
    }

    public void InitializeGrid()
    {
        Vector3Int[] fixedPillarPositions = {
            new Vector3Int(-3, -3, 0), new Vector3Int(-3, 0, 0), new Vector3Int(-3, 3, 0),
            new Vector3Int(0, -3, 0), new Vector3Int(0, 0, 0), new Vector3Int(0, 3, 0),
            new Vector3Int(3, -3, 0), new Vector3Int(3, 0, 0), new Vector3Int(3, 3, 0)
        };

        foreach (var pos in fixedPillarPositions)
        {
            grid[pos.x + gridOffset, pos.y + gridOffset] = 1; // Mark as a pillar
            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
            Instantiate(pillarPrefab, worldPos, Quaternion.identity);
        }
    }

    public void GenerateWorkstations()
    {
        bool validLayout = false;

        while (!validLayout)
        {
            workstationPositions.Clear();
            List<Vector3Int> validPositions = allowedWorkstationAndObstaclePositions.ToList();
            validPositions = validPositions.OrderBy(a => Random.value).ToList(); // Shuffle positions

            int placedWorkstations = 0;
            for (int i = 0; i < numberOfWorkstations && i < validPositions.Count; i++)
            {
                Vector3Int pos = validPositions[i];

                int gridX = pos.x + gridOffset;
                int gridY = pos.y + gridOffset;

                int[] possibleRotations = { 0, 90, -180, -90 };
                possibleRotations = possibleRotations.OrderBy(a => Random.value).ToArray(); // Shuffle rotation options

                foreach (int rotation in possibleRotations)
                {
                    if (CanPlaceWorkstation(pos, rotation))
                    {
                        PlaceObject(workstations[Random.Range(0, workstations.Length)], pos, rotation);
                        grid[gridX, gridY] = 2; // Mark as workstation

                        switch (rotation)//Mark corresponding space as a robot
                        {
                            case 0:
                                grid[gridX, gridY - 1] = 3;
                                break;
                            case 90:
                                grid[gridX - 1, gridY] = 3;
                                break;
                            case -90:
                                grid[gridX + 1, gridY] = 3;
                                break;
                            case -180:
                                grid[gridX, gridY + 1] = 3;
                                break;
                        }
                    }
                }

                if (placedWorkstations >= cap) break; // Ensure max cap workstations
            }

            validLayout = AreAllWorkstationsReachable();
        }
    }

    bool AreAllWorkstationsReachable()
    {
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(playerStart);
        visited.Add(playerStart);

        List<Vector2Int> remainingWorkers = new List<Vector2Int>(workstationPositions);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            remainingWorkers.Remove(current);

            if (remainingWorkers.Count == 0) return true; // All workers reached!

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (IsValidTile(neighbor) && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return false;
    }

    bool IsValidTile(Vector2Int pos)
    {
        return grid[pos.x, pos.y] == 0 || grid[pos.x, pos.y] == 1 || grid[pos.x, pos.y] == 2 || grid[pos.x, pos.y] == 3; // Walkable if empty, workstation, or player access/worker
    }

    bool CanPlaceWorkstation(Vector3Int pos, int rotation)
    {
        Vector3Int workerPos = GetWorkerPosition(pos, rotation);

        int workstationGridX = pos.x + gridOffset;
        int workstationGridY = pos.y + gridOffset;

        int workerGridX = workerPos.x + gridOffset;
        int workerGridY = workerPos.y + gridOffset;

        if (grid[workstationGridX, workstationGridY] != 0) return false;
        if (grid[workerGridX, workerGridY] != 0) return false;

        return true;
    }

    Vector3Int GetWorkerPosition(Vector3Int workstationPos, int rotation)
    {
        switch (rotation)
        {
            case 0: return workstationPos + new Vector3Int(0, -1, 0);
            case -90: return workstationPos + new Vector3Int(1, 0, 0);
            case -180:
            case 180: return workstationPos + new Vector3Int(0, 1, 0);

            case 90: return workstationPos + new Vector3Int(-1, 0, 0);
            default: return workstationPos;
        }
    }

    void PlaceObject(GameObject obj, Vector3Int gridPos, int yRotation)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(gridPos);
        Quaternion rotation = Quaternion.AngleAxis(yRotation, Vector3.up);
        Instantiate(obj, worldPos, rotation);
    }
}
