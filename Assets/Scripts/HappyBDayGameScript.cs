
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BirthdayMinigameController : MonoBehaviour, IMinigameBase 
{

    public GameObject minigameCanvas; // Reference to the Canvas
    public Button[] buttons; // Array of buttons for the minigame
    public RectTransform canvasRectTransform; // Reference to the Canvas RectTransform

    private static List<string> CORRECT_BUTTON_ORDER = new List<string> { "C", "C", "D", "C", "F", "E" };
    private static List<int> NOTE_DELAYS = new List<int> { 500, 250, 500, 500, 500, 1000 };
    private List<string> lastPressedButtons = new List<string>();

    private const string C_NOTE = "ButtonNoteC";
    private const string E_NOTE = "ButtonNoteE";
    private const string D_NOTE = "ButtonNoteD";
    private const string F_NOTE = "ButtonNoteF";

    private static float LAST_HINT_DEFAULT = -1f;
    private static int INITAL_DELAY = 5;
    private static float HINT_DEPLAY = 7.5f;
    private float lastHintTime = LAST_HINT_DEFAULT;

    public bool gamecompleted = false;

    void Start() {
        // Initialize buttons and set up their click listeners
        foreach (Button button in buttons) {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
        minigameCanvas.SetActive(false); // Hide the minigame canvas initially
        //minigameCanvas.SetActive(true);
    }

    void OnDisable() {
        lastHintTime = LAST_HINT_DEFAULT;
    }

    void Update() {
        // Show the minigame canvas when the player presses 'E'
        if (Input.GetKeyDown(KeyCode.E)) {
            minigameCanvas.SetActive(true);
        }

        if (minigameCanvas.activeInHierarchy && (lastHintTime != -1f) && (Time.time - lastHintTime >= HINT_DEPLAY)) {
            PlayHint();
        }
    }

    void PlayHint() {
        PlayRefrenceSong();
        lastHintTime = Time.time;
    }

    void ResetHint() {
        lastHintTime = Time.time;
    }

    void OnButtonClick(Button button) {
        ResetHint(); // Don't play a hint if they are in the middle of pressing buttons

        switch (button.name) {
            case "ExitButton":
                minigameCanvas.SetActive(false);
                break;
            case C_NOTE:
                AudioManager.instance.PlaySFX("note_c");
                NewNotePressed("C");
                break;
            case E_NOTE:
                AudioManager.instance.PlaySFX("note_e");
                NewNotePressed("E");
                break;
            case D_NOTE:
                AudioManager.instance.PlaySFX("note_d");
                NewNotePressed("D");
                break;
            case F_NOTE:
                AudioManager.instance.PlaySFX("note_f");
                NewNotePressed("F");
                break;
            default:
                Debug.LogWarning("Invalid button: " + button.name);
                return;
        }
    }

    async void PlayRefrenceSong() {
        for (int i = 0; i < CORRECT_BUTTON_ORDER.Count; i++) {

            string note = CORRECT_BUTTON_ORDER[i];

            switch (note) {
                case "C":
                    AudioManager.instance.PlaySFX("note_c");
                    break;
                case "E":
                    AudioManager.instance.PlaySFX("note_e");
                    break;
                case "D":
                    AudioManager.instance.PlaySFX("note_d");
                    break;
                case "F":
                    AudioManager.instance.PlaySFX("note_f");
                    break;
                default:
                    Debug.LogWarning("Unknown note: " + note);
                    return;
            }
            await Task.Delay(NOTE_DELAYS[i]);
        }
    }

    void NewNotePressed(string note) {
        if (lastPressedButtons.Count >= CORRECT_BUTTON_ORDER.Count) {
            lastPressedButtons.RemoveAt(0);
        }
        lastPressedButtons.Add(note);

        if (CORRECT_BUTTON_ORDER.Count == lastPressedButtons.Count && CORRECT_BUTTON_ORDER.SequenceEqual(lastPressedButtons)) {
            // YOU WIN!!!
            Debug.Log("You Won!!!");

            gamecompleted = true;

            minigameCanvas.SetActive(false);
        }
    }
    public void StartGame()
    {
        minigameCanvas.SetActive(true);
        Task.Delay(INITAL_DELAY);
        PlayHint();
    }
    public bool GameFinished()
    {
        return gamecompleted;
    }
}
