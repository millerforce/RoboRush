using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private TMP_Text timerText;

    private float _timerLength;

    private float _timeRemaining;

    [SerializeField]
    UnityEvent timerFinished;

    public void SetTimer(float timerMinutes)
    {
        _timeRemaining = timerMinutes * 60;
    }

    private void Start()
    {
        timerText = GetComponentInChildren<TMP_Text>();
    }

    public void Update()
    {
        _timeRemaining -= Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(_timeRemaining);

        timerText.text = timeSpan.ToString("mm':'ss");

        if (timeSpan.Minutes == 0 && timeSpan.Seconds == 0)
        {
            timerFinished.Invoke();
        }
    }

}
