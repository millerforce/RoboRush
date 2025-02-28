using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Threading.Tasks;

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
    Clock clock;

    private float _timeRemaining;

    [SerializeField]
    Transform startLocation;

    [SerializeField]
    PlayerController player;

    private FactoryState state;

    private GameObject[] foundStations;
    private List<Workstation> stations = new();
    private bool inEndlessMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int day = PlayerPrefs.GetInt("Day");
        Debug.Log($"Entering day: {day}");

        SetDifficultyByDay(day);

        inEndlessMode = PlayerPrefs.GetInt("Endless") == 1;

        _timeRemaining = _runMinutes * 60;
        state = FactoryState.STARTING;

        Invoke(nameof(FindStations), 0.5f);
        StartCoroutine(
                DisplayTheDay());
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
                _timeRemaining -= Time.deltaTime;

                TimeSpan timeSpan = TimeSpan.FromSeconds(_timeRemaining);

                string outMinutes;
                string outSeconds;

                int minutes = timeSpan.Minutes;
                if (minutes <= 0)
                {
                    outMinutes = "0";
                }
                else
                {
                    outMinutes = minutes.ToString();
                }

                int seconds = timeSpan.Seconds;
                if (seconds < 10)
                {
                    outSeconds = "0" + seconds;
                }
                else
                {
                    outSeconds = seconds.ToString();
                }
                clock.clockText.text = outMinutes + ":" + outSeconds;

                if (AllStationsDeleted())
                {
                    if (inEndlessMode)
                    {
                        state = FactoryState.NEXTDAY;
                    }
                    else
                    {
                        state = FactoryState.FAILED;
                    }
                }

                IsAnyMinigameRunning();

                if (_timeRemaining <= 0)
                {
                    //Display failure message maybe?

                    state = FactoryState.FAILED;
                }

                break;

            case FactoryState.FAILED:

                PlayerPrefs.SetInt("Day", 1);
                PlayerPrefs.Save();

                int highest = PlayerPrefs.GetInt("HighestDay", 1);
                int dayg = PlayerPrefs.GetInt("Day", 1);
                if (highest > dayg)
                {
                    PlayerPrefs.SetInt("HighestDay", highest);
                    PlayerPrefs.Save();
                }

                SceneManager.LoadScene("BreakRoom");

                break;

            case FactoryState.NEXTDAY:
                int day = PlayerPrefs.GetInt("Day", 1);
                PlayerPrefs.SetInt("Day", ++day);
                PlayerPrefs.Save();

                SceneManager.LoadScene("Endless");

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
            rectTransform.anchoredPosition = new Vector2(30 + i * 55, -5);
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
        _runMinutes = _defaultRuntime - day * 0.05f;
    }
}
