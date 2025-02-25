using System;
using TMPro;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private int _runMinutes;

    [SerializeField]
    Clock clock;

    private float _timeRemaining;

    [SerializeField]
    Transform startLocation;

    [SerializeField]
    PlayerController player;

    bool starting;
    void Start()
    {
        _timeRemaining = _runMinutes * 60;
        starting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (starting)
        {
            starting = player.StartAnimation();
        }
        else
        {
            _timeRemaining -= Time.deltaTime;

            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeRemaining);

            clock.clockText.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();
        }
            
    }


}
