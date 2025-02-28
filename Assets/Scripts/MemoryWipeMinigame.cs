using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;

public class MemoryWipeMinigame : MonoBehaviour, IMinigameBase
{
    public GameObject minigameCanvas; // Reference to the Canvas
    private bool gamecompleted = false;

    public TextMeshProUGUI hintTextField;
    public TextMeshProUGUI badAttemptField;
    public TMP_InputField inputField;

    public string passcode;

    [SerializeField] int maxLength;
    [SerializeField] int minLength;
    [SerializeField] int maxLevel;

    public string[] passwordList =
    {
        "password",
        "qwerty",
        "abc123",
        "iamhuman",
        "teamleaderconnor",
        "1234",
        "12345"
    };

    private bool isActive = false;

    void Start()
    {
        int day = PlayerPrefs.GetInt("Day");
        generatePassword(determinePasswordLength(day));

        badAttemptField.gameObject.SetActive(false);
        minigameCanvas.SetActive(false); // Hide the minigame initially
        isActive = false;

        inputField.onValueChanged.AddListener(onInputChanged);
        inputField.onEndEdit.AddListener(guessPassword);
    }
    void Update()
    {

    }

    int determinePasswordLength(int day)
    {
        return Mathf.FloorToInt(Mathf.Lerp(minLength, maxLength, day / maxLevel));
    }

    void generatePassword(int size)
    {
        //get random number, assign passcode to passwordExamples[random]
        //int passwordIndex = UnityEngine.Random.Range(0, passwordList.Length);
        //passcode = passwordList[passwordIndex];

        string abc = "abcdefghjkmnopqrstuvwxyz";

        for (int i = 0; i < size; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, abc.Length);

            if (UnityEngine.Random.Range(0, 2) == 0) passcode += abc[randIndex];
            else passcode += Char.ToUpper(abc[randIndex]);
        }

        hintTextField.text = "HINT: " + passcode;
    }

    void onInputChanged(string inputText)
    {
        badAttemptField.gameObject.SetActive(false);
    }

    void guessPassword(string input)
    {
        if (input == passcode)
        {
            MinigameCompleted();
        } else
        {
            badAttemptField.gameObject.SetActive(true);
            inputField.Select();
            inputField.ActivateInputField();
        }

        inputField.text = "";
    }

    void MinigameCompleted()
    {
        minigameCanvas.SetActive(false);
        isActive = false;
        gamecompleted = true;

        int day = PlayerPrefs.GetInt("Day");
        generatePassword(determinePasswordLength(day));
    }

    public void StartGame()
    {
        minigameCanvas.SetActive(true);
        isActive = true;
        gamecompleted = false;

        inputField.Select();
        inputField.ActivateInputField();
    }

    public bool GameFinished()
    {
        return gamecompleted;
    }
    public bool IsRunning()
    {
        return isActive;
    }
}
