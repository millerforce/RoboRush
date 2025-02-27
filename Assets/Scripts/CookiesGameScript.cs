using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JetBrains.Annotations;

public class CookieClickerMinigame : MonoBehaviour, IMinigameBase
{
    public GameObject minigameCanvas; // Reference to the Canvas
    public Button[] cookieButtons; // Array of buttons for the minigame
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform

    public Texture2D[] cookieTextures; // Array of cookie textures (0: full, 1: half-eaten, 2: mostly-eaten)
    public Texture2D[] cookieTextures2; // Array of cookie textures (0: full, 1: half-eaten, 2: mostly-eaten)
    public Texture2D[] cookieTextures3; // Array of cookie textures (0: full, 1: half-eaten, 2: mostly-eaten)
    public Texture2D[] cookieTextures4; // Array of cookie textures (0: full, 1: half-eaten, 2: mostly-eaten)

    public bool gamecompleted = false;

    private int cookiesClicked = 0;
    private int[] buttonClickCounts; // Array to keep track of clicks for each button

    void Start()
    {
        buttonClickCounts = new int[cookieButtons.Length];

        // Initialize buttons and set up their click listeners
        foreach (Button button in cookieButtons)
        {
            button.onClick.AddListener(() => OnCookieClick(button));
            PlaceButtonRandomly(button);
        }
        minigameCanvas.SetActive(false); // Hide the minigame canvas initially
    }

    Texture2D[] selectRandomCookie()
    {
        int cookieType = Random.Range(0, 3);
        switch (cookieType)
        {
            case 0:
                return 
        }
    }

    void OnCookieClick(Button button)
    {
        int index = System.Array.IndexOf(cookieButtons, button);
        buttonClickCounts[index]++;

        RawImage buttonImage = button.GetComponentInChildren<RawImage>();

        if (buttonClickCounts[index] < 3)
        {
            // Change the button texture to a more eaten cookie
            buttonImage.texture = cookieTextures[buttonClickCounts[index]];
        }
        else
        {
            // Hide the button after 3 clicks
            button.gameObject.SetActive(false);
            cookiesClicked++;

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
            cookieButtons[i].GetComponentInChildren<RawImage>().texture = cookieTextures[0]; // Reset to full cookie texture
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