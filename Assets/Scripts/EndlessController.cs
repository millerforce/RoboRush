using UnityEngine;

public class EndlessController : MonoBehaviour
{

    [SerializeField]
    LevelGenerator generator;

    private void Start()
    {
        int day = PlayerPrefs.GetInt("Day");

        //generator.InitializeGrid();
        //generator.GenerateWorkstations();

    }

}
