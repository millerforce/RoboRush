using UnityEngine;

public class BreakRoom : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    [SerializeField]
    ClipBoard clipBoard;

    bool starting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starting = true;

        int currentDay = PlayerPrefs.GetInt("Day", -1);
        Debug.Log("Current Day:" + currentDay);
        clipBoard.clipboardText.text = currentDay.ToString();
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
