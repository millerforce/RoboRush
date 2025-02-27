using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CookieClickerMinigame : MonoBehaviour, IMinigameBase
{
    public GameObject minigameCanvas; // Reference to the Canvas
    public Button[] cookieButtons; // Array of buttons for the minigame
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform
    public CookieType[] cookieTypes; // Array of different cookie types

    public bool gamecompleted = false;

    private int cookiesClicked = 0;
    private int[] buttonClickCounts; // Array to keep track of clicks for each button
    private int[] buttonCookieTypes; // Array to keep track of the cookie type for each button

    void Start()
    {
        buttonClickCounts = new int[cookieButtons.Length];
        buttonCookieTypes = new int[cookieButtons.Length];

        // Initialize buttons and set up their click listeners
        for (int i = 0; i < cookieButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda expression
            cookieButtons[i].onClick.AddListener(() => OnCookieClick(index));
            PlaceButtonRandomly(cookieButtons[i]);
            buttonCookieTypes[i] = Random.Range(0, cookieTypes.Length); // Assign a random cookie type to each button
            cookieButtons[i].GetComponentInChildren<RawImage>().texture = cookieTypes[buttonCookieTypes[i]].cookieStages[0]; // Set initial texture
        }
        minigameCanvas.SetActive(false); // Hide the minigame canvas initially
    }

    void OnCookieClick(int index)
    {
        buttonClickCounts[index]++;

        RawImage buttonImage = cookieButtons[index].GetComponentInChildren<RawImage>();

        if (buttonClickCounts[index] < 3)
        {
            // Change the button texture to a more eaten cookie
            buttonImage.texture = cookieTypes[buttonCookieTypes[index]].cookieStages[buttonClickCounts[index]];
            Debug.Log($"Button {index} clicked {buttonClickCounts[index]} times. Texture changed.");
        }
        else
        {
            // Hide the button after 3 clicks
            cookieButtons[index].gameObject.SetActive(false);
            cookiesClicked++;
            Debug.Log($"Button {index} clicked 3 times. Button hidden.");

            if (cookiesClicked >= cookieButtons.Length)
            {
                MinigameCompleted();
            }
        }
    }

    void PlaceButtonRandomly(Button button)
    {
        float x = Random.Range(0, canvasRectTransform.rect.width);
        float y = Random.Range(0, canvasRectTransform.rect.height);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(x - canvasRectTransform.rect.width / 2, y - canvasRectTransform.rect.height / 2);

        // Place the image at the same position as the button
        RawImage buttonImage = button.GetComponentInChildren<RawImage>();
        if (buttonImage != null)
        {
            buttonImage.rectTransform.anchoredPosition = new Vector2(x - canvasRectTransform.rect.width / 2, y - canvasRectTransform.rect.height / 2);
        }
    }

    void MinigameCompleted()
    {
        // Logic for when the minigame is completed
        minigameCanvas.SetActive(false);
        cookiesClicked = 0;

        // Reset buttons for next time
        for (int i = 0; i < cookieButtons.Length; i++)
        {
            cookieButtons[i].gameObject.SetActive(true);
            buttonClickCounts[i] = 0;
            buttonCookieTypes[i] = Random.Range(0, cookieTypes.Length); // Assign a random cookie type to each button
            cookieButtons[i].GetComponentInChildren<RawImage>().texture = cookieTypes[buttonCookieTypes[i]].cookieStages[0]; // Reset to full cookie texture
            PlaceButtonRandomly(cookieButtons[i]);
        }

        // Invoke the completion event
        gamecompleted = true;
    }

    public void StartGame()
    {
        minigameCanvas.SetActive(true);
    }

    public bool GameFinished()
    {
        return gamecompleted;
    }
}