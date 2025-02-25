using UnityEngine;

public class BreakRoom : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    bool starting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (starting)
        {
            starting = player.StartAnimation();
        }
    }
}
