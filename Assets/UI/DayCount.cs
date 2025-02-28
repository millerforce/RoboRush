using UnityEngine;
using TMPro; // Add this if you're using TextMeshPro
using System.Collections; // Add this to use IEnumerator

public class DayDisplay : MonoBehaviour
{
    public static DayDisplay instance;
    public TextMeshProUGUI dayText; // Use this if you're using TextMeshPro
    public Canvas dayCanvas; // Reference to the Canvas


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateDayText();
        StartCoroutine(ShowDayCanvas());
    }

    void UpdateDayText()
    {
        int dayValue = PlayerPrefs.GetInt("Day", 1);
        dayText.text = "Day " + dayValue.ToString();
    }


    public IEnumerator ShowDayCanvas()
    {
        UpdateDayText();
        dayCanvas.gameObject.SetActive(true); // Show the canvas
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        dayCanvas.gameObject.SetActive(false); // Hide the canvas
    }
}