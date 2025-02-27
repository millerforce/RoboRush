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
    STOPPING
}

public class FactoryController : MonoBehaviour
{
    [SerializeField]
    private int _runMinutes;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

                clock.clockText.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();

                if (AllStationsDeleted())
                {
                    state = FactoryState.STOPPING;
                }
                break;

            case FactoryState.STOPPING:

                SceneManager.LoadScene("BreakRoom");

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
}
