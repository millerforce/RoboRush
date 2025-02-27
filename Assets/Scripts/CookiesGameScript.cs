using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CookieClickerMinigame : MinigameBase
{
    public GameObject minigameCanvas; // Reference to the Canvas
    public Button[] cookieButtons; // Array of buttons for the minigame
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform

    private int cookiesClicked = 0;

    void Start()
    {
        // Initialize buttons and set up their click listeners
        foreach (Button button in cookieButtons)
        {
            button.onClick.AddListener(() => OnCookieClick(button));
            PlaceButtonRandomly(button);
        }
        minigameCanvas.SetActive(false); // Hide the minigame canvas initially
        minigameCanvas.SetActive(true);//Todo: remove this and fix starting minigames
    }

    void OnCookieClick(Button button)
    {
        cookiesClicked++;
        button.gameObject.SetActive(false); // Hide the button when clicked

        if (cookiesClicked >= cookieButtons.Length)
        {
            MinigameCompleted();
        }
    }

    void PlaceButtonRandomly(Button button)
    {
        float x = Random.Range(0, canvasRectTransform.rect.width);
        float y = Random.Range(0, canvasRectTransform.rect.height);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(x - canvasRectTransform.rect.width / 2, y - canvasRectTransform.rect.height / 2);

        // Place the image at the same position as the button
        Image buttonImage = button.GetComponentInChildren<Image>();
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
        foreach (Button button in cookieButtons)
        {
            button.gameObject.SetActive(true);
            PlaceButtonRandomly(button);
        }

        // Invoke the completion event
        CompleteMinigame();
    }
}