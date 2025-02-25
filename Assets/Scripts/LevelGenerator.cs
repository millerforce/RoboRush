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

    int[,] grid = new int[9, 9];
    int gridOffset = 4;

    Vector3Int[] allowedWorkstationPositions = {
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

            // ✅ Convert tilemap position to world position
            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);

            // ✅ Instantiate pillar at the correct world position
            Instantiate(pillarPrefab, worldPos, Quaternion.identity);

            Debug.Log($"Pillar spawned at: {worldPos}");
        }
    }

    void GenerateWorkstations()
    {
        List<Vector3Int> validPositions = allowedWorkstationPositions.ToList();
        validPositions = validPositions.OrderBy(a => Random.value).ToList(); // Shuffle positions

        Debug.Log($"Spawning {numberOfWorkstations} workstations");

        for (int i = 0; i < numberOfWorkstations && i < validPositions.Count; i++)
        {
            Vector3Int pos = validPositions[i];

            // Convert to grid array index
            int gridX = pos.x + gridOffset;
            int gridY = pos.y + gridOffset;

            // Ensure it's empty before placing
            if (grid[gridX, gridY] == 0)
            {
                PlaceObject(workstations[i % workstations.Length], pos);
                grid[gridX, gridY] = 2; // Mark as workstation
            }
        }
    }

    void PlaceObject(GameObject obj, Vector3Int gridPos)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(gridPos);
        Debug.Log($"Workstation spawned at: {worldPos}");
        Instantiate(obj, worldPos, Quaternion.identity);
    }
}
