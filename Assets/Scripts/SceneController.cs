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
            starting = StartAnimation();
        }
        else
        {
            _timeRemaining -= Time.deltaTime;

            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeRemaining);

            clock.clockText.text = timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString();
        }
            
    }

    bool StartAnimation()
    {
        if (!player.transform.position.Equals(startLocation.position)) {

            player.transform.position = Vector3.MoveTowards(player.transform.position, startLocation.position, player.WalkSpeed * Time.deltaTime);

            return true;
        }
        else
        {
            return false;
        }
    }
}
