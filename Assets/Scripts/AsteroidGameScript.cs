using UnityEngine;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
    public GameObject minigameCanvas; // Reference to the Canvas
    public Button[] buttons; // Array of buttons for the minigame
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform

    private int buttonsClicked = 0;

    void Start()
    {
        // Initialize buttons and set up their click listeners
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
            PlaceButtonRandomly(button);
        }
        minigameCanvas.SetActive(false); // Hide the minigame canvas initially
        minigameCanvas.SetActive(true);
    }

    void Update()
    {
        // Show the minigame canvas when the player presses 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            minigameCanvas.SetActive(true);
        }
    }

    void OnButtonClick(Button button)
    {
        buttonsClicked++;
        button.gameObject.SetActive(false); // Hide the button when clicked

        if (buttonsClicked >= buttons.Length)
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
        buttonsClicked = 0;

        // Reset buttons for next time
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
            PlaceButtonRandomly(button);
        }
    }
}
