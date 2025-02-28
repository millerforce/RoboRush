using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

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

    private bool isAMinigameRunning = false;

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
        foreach(Workstation station in stations)
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
    }

    bool IsAnyMinigameRunning()
    {
        foreach(Workstation workstation in stations)
        {
            if (workstation.playingMinigame)
            {
                foreach(Workstation station in stations)
                {
                    if (!station.playingMinigame)
                    {
                        station.allowedToPlayMinigame = false;
                    }
                }
                return true;
            }
        }

        foreach(Workstation station in stations)
        {
            station.allowedToPlayMinigame = true;
        }

        return false;
    }

    void SetDifficultyByDay(int day)
    {
        _runMinutes = _defaultRuntime - day * 0.05f;
    }
}
