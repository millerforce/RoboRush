using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject pillarPrefab;
    public GameObject[] workstations;
    public int numberOfWorkstations;
    public GameObject[] obstacles;
    public int numberOfObstacles;

    int[,] grid = new int[9, 9];
    int gridOffset = 4;

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
        // GenerateObstacles();
    }

    void InitializeGrid()
    {
        // Fixed pillar positions (relative to center)
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

            Debug.Log($"Pillar spawned at: {worldPos}");
        }
    }

    void GenerateWorkstations()
    {
        List<Vector3Int> validPositions = allowedWorkstationAndObstaclePositions.ToList();
        validPositions = validPositions.OrderBy(a => Random.value).ToList(); // Shuffle positions

        Debug.Log($"Spawning {numberOfWorkstations} workstations");

        for (int i = 0; i < numberOfWorkstations && i < validPositions.Count; i++)
        {
            Vector3Int pos = validPositions[i];

            // Convert to grid array index
            int gridX = pos.x + gridOffset;
            int gridY = pos.y + gridOffset;

            // Try random rotations while ensuring accessibility
            int[] possibleRotations = { 0, 90, 180, 270 };
            possibleRotations = possibleRotations.OrderBy(a => Random.value).ToArray(); // Shuffle rotation options

            foreach (int rotation in possibleRotations)
            {
                if (CanPlaceWorkstation(pos, rotation))
                {
                    PlaceObject(workstations[i % workstations.Length], pos, rotation);
                    grid[gridX, gridY] = 2; // Mark as workstation
                    break;
                }
            }
        }
    }

    bool CanPlaceWorkstation(Vector3Int pos, int rotation)
    {
        Vector3Int workerPos = GetWorkerPosition(pos, rotation);
        Vector3Int playerAccessPos = GetPlayerAccessPosition(pos, rotation);

        int workerGridX = workerPos.x + gridOffset;
        int workerGridY = workerPos.y + gridOffset;

        int accessGridX = playerAccessPos.x + gridOffset;
        int accessGridY = playerAccessPos.y + gridOffset;

        // Ensure the worker position is not occupied by a pillar (1) or another workstation (2)
        if (grid[workerGridX, workerGridY] != 0) return false;

        // Ensure the player access position is open (not occupied)
        if (grid[accessGridX, accessGridY] != 0) return false;

        return true; // Placement is valid
    }



    Vector3Int GetWorkerPosition(Vector3Int workstationPos, int rotation)
    {
        switch (rotation)
        {
            case 0: return workstationPos + new Vector3Int(0, -1, 0);  // Worker below
            case 90: return workstationPos + new Vector3Int(1, 0, 0);  // Worker right
            case 180: return workstationPos + new Vector3Int(0, 1, 0); // Worker above
            case 270: return workstationPos + new Vector3Int(-1, 0, 0); // Worker left
            default: return workstationPos;
        }
    }

    Vector3Int GetPlayerAccessPosition(Vector3Int workstationPos, int rotation)
    {
        switch (rotation)
        {
            case 0: return workstationPos + new Vector3Int(0, 1, 0);  // Player access above
            case 90: return workstationPos + new Vector3Int(-1, 0, 0); // Player access left
            case 180: return workstationPos + new Vector3Int(0, -1, 0); // Player access below
            case 270: return workstationPos + new Vector3Int(1, 0, 0); // Player access right
            default: return workstationPos;
        }
    }



    // void GenerateObstacles()
    // {
    //     List<Vector3Int> validPositions = allowedWorkstationAndObstaclePositions.ToList();
    //     validPositions = validPositions.OrderBy(a => Random.value).ToList(); // Shuffle positions

    //     Debug.Log($"Spawning {numberOfObstacles} obstacles");

    //     for (int i = 0; i < numberOfObstacles && i < validPositions.Count; i++)
    //     {
    //         Vector3Int pos = validPositions[i];

    //         // Convert to grid array index
    //         int gridX = pos.x + gridOffset;
    //         int gridY = pos.y + gridOffset;

    //         // Ensure it's empty before placing
    //         if (grid[gridX, gridY] == 0)
    //         {
    //             PlaceObject(obstacles[i % obstacles.Length], pos);
    //             grid[gridX, gridY] = 3; // Mark as obstacle
    //         }
    //     }
    // }

    void PlaceObject(GameObject obj, Vector3Int gridPos, int yRotation)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(gridPos);
        Debug.Log($"Workstation spawned at: {worldPos} with rotation {yRotation}°");

        // Apply rotation
        Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
        Instantiate(obj, worldPos, rotation);
    }

}
