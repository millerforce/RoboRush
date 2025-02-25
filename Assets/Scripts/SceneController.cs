using System;
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private int _runMinutes;

    [SerializeField]
    Clock clock;

    private float _timeRemaining;

    void Start()
    {
        _timeRemaining = _runMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {

        _timeRemaining -= Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(_timeRemaining);

        clock.clockText.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();
    }
}
