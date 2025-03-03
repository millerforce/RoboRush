using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FactoryState
{
    STARTING,
    RUNNING,
    FAILED,
    NEXTDAY
}

public class FactoryController : MonoBehaviour
{
    [SerializeField]
    private float _runMinutes;

    private const float _defaultRuntime = 2f;

    [SerializeField]
    private Timer timer;

    public GameObject progBarPrefab;

    private float _timeRemaining;

    [SerializeField]
    Transform startLocation;

    [SerializeField]
    PlayerController player;

    private FactoryState state;

    private GameObject[] foundStations;
    private List<Workstation> stations = new();

    public Material[] alertMaterials;

    Color[] alertColors = { new Color(1, 0, 0), new Color(0.9924106f, 0, 1), new Color(0.66044f, 0, 1), new Color(0.04705951f, 0, 1), new Color(0, 0.7338469f, 1), new Color(0, 1, 0.6611695f), new Color(0, 1, 0), new Color(1, 0.9447687f, 0), new Color(1, 0.6415883f, 0), new Color(0.2578616f, 0.1661031f, 0.1224437f) };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int day = PlayerPrefs.GetInt("Day");
        Debug.Log($"Entering day: {day}");

        SetDifficultyByDay(day);

        timer.SetTimer(_runMinutes);

        SetHighScore();

        _timeRemaining = _runMinutes * 60;
        state = FactoryState.STARTING;

        Invoke(nameof(FindStations), 0.5f);
        StartCoroutine(DisplayTheDay());
    }

    IEnumerator DisplayTheDay()
    {
        yield return new WaitForSeconds(1);
        
        StartCoroutine(DayDisplay.instance.ShowDayCanvas());
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case FactoryState.STARTING:
                if (!player.StartAnimation())
                {
                    state = FactoryState.RUNNING;
                }
                break;

            case FactoryState.RUNNING:

                if (AllStationsDeleted())
                {
                    StartNextDay();
                }

                IsAnyMinigameRunning();

                break;
        }
    }
    bool AllStationsDeleted()
    {
        foreach (Workstation station in stations)
        {
            if (station != null)
                return false;
        }
        return true;
    }

    public void TimerFinished()//Timer manages itself and calls this method when completed
    {
        Debug.Log("Day failed returning to breakroom");
        ResetDay();

        SceneManager.LoadScene("BreakRoom");
    }

    void FindStations()
    {
        foundStations = GameObject.FindGameObjectsWithTag("Workstation");

        foreach (GameObject obj in foundStations)
        {

            if (obj.TryGetComponent<Workstation>(out var station))
            {
                stations.Add(station);
            }
        }

        for (int i = 0; i < stations.Count; i++)
        {

            GameObject progressBarObj = Instantiate(progBarPrefab, new Vector3(2, 5, 10), Quaternion.identity);
            progressBarObj.GetComponent<ProgressBarController>().workstation = stations[i];
            // Find the child object
            GameObject progressBarHolder = progressBarObj.transform.Find("ProgressBarHolder").gameObject;

            RectTransform rectTransform = progressBarObj.transform.Find("ProgressBarHolder").GetComponent<RectTransform>();
            // Set the global position
            rectTransform.anchoredPosition = new Vector2(30 + i * 55, -10);
            Transform alertChild = stations[i].transform.Find("alert");
            Renderer childRenderer = alertChild.GetComponent<Renderer>();
            childRenderer.material = alertMaterials[i];

            GameObject progressBarBkgFill = progressBarHolder.transform.Find("BackgroundImage")?.gameObject;
            if (progressBarBkgFill != null)
            {
                GameObject progressBarFill = progressBarBkgFill.transform.Find("FillImage")?.gameObject;
                if (progressBarFill != null)
                {
                    Image fillImage = progressBarFill.GetComponent<Image>();
                    if (fillImage != null)
                    {
                        fillImage.color = alertColors[i];
                        Debug.Log("pleaste");
                    }
                }
            }

        }
    }
    void IsAnyMinigameRunning()
    {
        foreach (Workstation workstation in stations)
        {
            if (workstation.playingMinigame)
            {
                foreach (Workstation station in stations)
                {
                    if (!station.playingMinigame)
                    {
                        station.allowedToPlayMinigame = false;
                    }
                }
                return;
            }
        }

        foreach (Workstation station in stations)
        {
            station.allowedToPlayMinigame = true;
        }
    }

    void SetDifficultyByDay(int day)
    {
        float newTime = _defaultRuntime - day * 0.05f;

        if (newTime < 1f)
        {
            _runMinutes = 1f;
        }
        else
        {
            _runMinutes = newTime;
        }
        
    }

    public static void SetHighScore()
    {
        int highest = PlayerPrefs.GetInt("HighestDay", 1);
        int day = PlayerPrefs.GetInt("Day", 1);
        if (day > highest)
        {
            PlayerPrefs.SetInt("HighestDay", day);
            PlayerPrefs.Save();
        }
    }
    public void StartNextDay()
    {
        IncrementDay();

        SceneManager.LoadScene("Endless");
    }

    public static void IncrementDay() 
    {
        int day = PlayerPrefs.GetInt("Day", 1);
        PlayerPrefs.SetInt("Day", ++day);
        PlayerPrefs.Save();
    }

    public static void ResetDay()
    {
        PlayerPrefs.SetInt("Day", 1);
        PlayerPrefs.Save();
    }
}
