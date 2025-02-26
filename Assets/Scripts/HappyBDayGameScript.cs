
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BirthdayMinigameController : MonoBehaviour {

    public GameObject minigameCanvas; // Reference to the Canvas
    public Button[] buttons; // Array of buttons for the minigame
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform

    private static List<string> CORRECT_BUTTON_ORDER = new List<string> { "C", "D", "E", "F" };
    private List<string> lastPressedButtons = new List<string>();

    private const string C_NOTE = "ButtonNoteC";
    private const string E_NOTE = "ButtonNoteE";
    private const string D_NOTE = "ButtonNoteD";
    private const string F_NOTE = "ButtonNoteF";

    void Start() {
        // Initialize buttons and set up their click listeners
        foreach (Button button in buttons) {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
        minigameCanvas.SetActive(false); // Hide the minigame canvas initially
        minigameCanvas.SetActive(true);
    }

    void Update() {
        // Show the minigame canvas when the player presses 'E'
        if (Input.GetKeyDown(KeyCode.E)) {
            minigameCanvas.SetActive(true);
        }
    }

    void OnButtonClick(Button button) {
        Debug.Log(button.name);

        switch (button.name) {
            case C_NOTE:
                NewNotePressed("C");
                break;
            case E_NOTE:
                NewNotePressed("E");
                break;
            case D_NOTE:
                NewNotePressed("D");
                break;
            case F_NOTE:
                NewNotePressed("F");
                break;
            default:
                Debug.LogWarning("Invalid button: " + button.name);
                return;
        }
    }

    void NewNotePressed(string note) {
        if (lastPressedButtons.Count >= CORRECT_BUTTON_ORDER.Count) {
            Debug.Log("First Removed");
            lastPressedButtons.RemoveAt(0);
        }
        lastPressedButtons.Add(note);

        if (CORRECT_BUTTON_ORDER.Count == lastPressedButtons.Count && CORRECT_BUTTON_ORDER.SequenceEqual(lastPressedButtons)) {
            // YOU WIN!!!
            Debug.Log("You Won!!!");

            minigameCanvas.SetActive(false);
        }
    }
}
