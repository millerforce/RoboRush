using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CookieClickerMinigame : MonoBehaviour, IMinigameBase
{
    public GameObject minigameCanvas; // Reference to the Canvas
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform
    public CookieType[] cookieTypes; // Array of different cookie types

    [SerializeField]
    public int maxCookies; // Number of cookie buttons to generate
    [SerializeField]
    public int minCookies; // Number of cookie buttons to generate
    [SerializeField]
    public Vector3 cookieScale;
    [SerializeField]
    public int maxLevel; // At what level will the max difficulty of game happen

    private List<Button> cookieButtons = new List<Button>(); // List of buttons
    private List<int> buttonClickCounts = new List<int>(); // Click counts per button
    private List<int> buttonCookieTypes = new List<int>(); // Cookie type per button

    private int cookiesClicked = 0;
    private bool gamecompleted = false;

    void Start()
    {
        int day = PlayerPrefs.GetInt("Day");

        GenerateCookies(determineAmountOfCookies(day));
        minigameCanvas.SetActive(false); // Hide the minigame initially
    }

    int determineAmountOfCookies(int day)
    {
        return Mathf.FloorToInt(Mathf.Lerp(minCookies, maxCookies, day / maxLevel));
    }

    void GenerateCookies(int amount)
    {
        // Clear old buttons if re-generating
        foreach (Button btn in cookieButtons)
        {
            Destroy(btn.gameObject);
        }
        cookieButtons.Clear();
        buttonClickCounts.Clear();
        buttonCookieTypes.Clear();

        for (int i = 0; i < amount; i++)
        {
            CreateCookieButton();
        }
    }

    void CreateCookieButton()
    {
        // Create a new UI Button
        GameObject newButtonObj = new GameObject("CookieButton", typeof(RectTransform), typeof(Button), typeof(RawImage));
        newButtonObj.transform.SetParent(minigameCanvas.transform, false);

        Button newButton = newButtonObj.GetComponent<Button>();
        RawImage buttonImage = newButtonObj.GetComponent<RawImage>();

        // Configure button visuals
        int cookieType = Random.Range(0, cookieTypes.Length);
        buttonImage.texture = cookieTypes[cookieType].cookieStages[0];

        // Store button details
        int index = cookieButtons.Count;
        newButton.onClick.AddListener(() => OnCookieClick(index));

        cookieButtons.Add(newButton);
        buttonClickCounts.Add(0);
        buttonCookieTypes.Add(cookieType);

        // Set the scale of the button
        RectTransform rect = newButton.GetComponent<RectTransform>();
        rect.localScale = cookieScale;

        // Position button randomly in the canvas
        PlaceButtonRandomly(newButton);
    }

    void OnCookieClick(int index)
    {
        buttonClickCounts[index]++;

        RawImage buttonImage = cookieButtons[index].GetComponent<RawImage>();

        if (buttonClickCounts[index] < 3)
        {
            // Update texture to show a more eaten cookie
            buttonImage.texture = cookieTypes[buttonCookieTypes[index]].cookieStages[buttonClickCounts[index]];
        }
        else
        {
            // Hide the button after 3 clicks
            cookieButtons[index].gameObject.SetActive(false);
            cookiesClicked++;

            if (cookiesClicked >= cookieButtons.Count)
            {
                MinigameCompleted();
            }
        }
    }

    void PlaceButtonRandomly(Button button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        RectTransform panelRectTransform = canvasRectTransform.GetComponent<RectTransform>(); // Reference the spawning panel

        // Set pivot & anchors to center
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        // Get panel dimensions
        float panelWidth = panelRectTransform.rect.width;
        float panelHeight = panelRectTransform.rect.height;

        // Safe padding to prevent overlap
        float padding = 50f;

        // Calculate random position within the panel
        float x = Random.Range(-panelWidth / 2 + padding, panelWidth / 2 - padding);
        float y = Random.Range(-panelHeight / 2 + padding, panelHeight / 2 - padding);

        // Convert local panel position to world position
        Vector3 worldPosition = panelRectTransform.TransformPoint(new Vector3(x, y, 0));

        // Assign position relative to the panel
        rect.position = worldPosition;
    }


    void MinigameCompleted()
    {
        minigameCanvas.SetActive(false);
        cookiesClicked = 0;
        gamecompleted = true;
    }

    public void StartGame()
    {
        minigameCanvas.SetActive(true);
        gamecompleted = false;
    }

    public bool GameFinished()
    {
        return gamecompleted;
    }
}
