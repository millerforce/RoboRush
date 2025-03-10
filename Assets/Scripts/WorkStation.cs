using System.Collections.Generic;
using UnityEngine;

public enum StationState
{
    WORKING,//Working on task
    BROKEN,
    PLAYINGMINIGAME,
    FINISHED
}

public class Workstation : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem takeoff;

    [SerializeField]
    private GameObject alert;

    private StationState state;

    private float _completionTime;//The amount of time the station needs to be in the working state to finish its task

    private float _cooldownTime = 15f;//The time interval the station can break down on

    private float _timeCompleted;//The amount of time the workstation has been working on its task

    private float _breakdownChance = 0.05f;//The probability each frame outside the cooldown that the work station will breakdown

    private float _timeSinceBreakdown = 0;//Tracking how long it has been since the last breakdown

    private bool _canBreak;

    private float _timeSinceAttemptedBreakdown;//The time since we last tried to enter the broke state

    private float _attemptBreakDownInterval = 2f;

    private static readonly float exitHeight = 10f;

    private static readonly float takeoffSpeed = 6f;

    private List<IMinigameBase> minigames = new();

    private IMinigameBase activeMinigame;

    private const string brokenAnimation = "isBroken";

    public bool playingMinigame = false;

    public bool allowedToPlayMinigame = true;

    [SerializeField]
    Animator stationAnimator;

    [SerializeField]
    Animator robotAnimator;

    [SerializeField]
    ParticleSystem stationEffect;

    private string[] brokeSFX = { "bot_aaah_low", "bot_aaah", "bot_ack", "bot_off", "bot_sad", "bot_waoa" };

    private void Start()
    {
        alert.SetActive(false);

        int day = PlayerPrefs.GetInt("Day");
        SetDifficultyByDay(day);

        TryAnimation(stationAnimator, brokenAnimation, false);
        TryAnimation(robotAnimator, brokenAnimation, false);

        TrySetParticles(true);

        takeoff.Stop();

        ChanceStartState();

        _timeCompleted = 0f;
        _canBreak = false;
        _timeSinceBreakdown = 0f;

        MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script is IMinigameBase)
            {
                minigames.Add(script as IMinigameBase);
            }
        }

        RandomizeMinigame();
    }

    private void Update()
    {
        switch (state)
        {
            case StationState.WORKING:
                alert.SetActive(false);

                _timeCompleted += Time.deltaTime;
                _timeSinceBreakdown += Time.deltaTime;

                if (_timeCompleted >= _completionTime)
                {
                    AudioManager.instance.PlaySFX("blast_off");
                    state = StationState.FINISHED;
                }

                if (_timeSinceBreakdown > _cooldownTime && !_canBreak)//if cooldown is over and we arent currently trying to break
                {
                    _canBreak = true;
                }

                if (_canBreak)
                {
                    if (_timeSinceAttemptedBreakdown >= _attemptBreakDownInterval)
                    {
                        int randValue = Random.Range(0, 1);

                        if (randValue <= _breakdownChance)
                        {
                            TransitionToBroken();
                        }
                    }
                    _timeSinceAttemptedBreakdown += Time.deltaTime;
                }

                break;

            case StationState.BROKEN:

                //Add logic for showing exclamation point
                alert.SetActive(true);
                

                break;

            case StationState.PLAYINGMINIGAME:
                //alert.SetActive(false);

                if (!activeMinigame.IsRunning())
                {
                    playingMinigame = false;
                    state = StationState.BROKEN;
                }

                if (activeMinigame.GameFinished())
                {
                    playingMinigame = false;
                    Debug.Log("Station has been fixed");
                    state = StationState.WORKING;
                    _timeSinceBreakdown = 0f;//reset cooldown timer
                    TryAnimation(stationAnimator, brokenAnimation, false);
                    TryAnimation(robotAnimator, brokenAnimation, false);
                    TrySetParticles(true);

                    RandomizeMinigame();
                }
                break;

            case StationState.FINISHED:
                alert.SetActive(false);
                //This is where they take off
                FinishTask();

                break;
        }
    }

    public void FinishTask()
    {
        if (!takeoff.isPlaying)
        {
            takeoff.Play();
        }

        if (transform.position.y <= exitHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + takeoffSpeed * Time.deltaTime, transform.position.z);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!allowedToPlayMinigame)
        {
            return;
        }
        if (other.isTrigger || !GetComponent<Collider>().isTrigger)
        {
            return;
        }
        if (!other.CompareTag("Player") || state != StationState.BROKEN)
        {
            return;
        }
        Debug.Log("Player in active minigame trigger");
        if (Input.GetKey(KeyCode.E))
        {
            playingMinigame = true;
            activeMinigame.StartGame();
            state = StationState.PLAYINGMINIGAME;
        }
    }

    private void TryAnimation(Animator animator, string animation, bool flag)
    {
        if (animator != null)
        {
            animator.SetBool(animation, flag);
        }
    }

    void SetDifficultyByDay(int day)
    {
        float newTime = 2 - day * 0.05f;
        float dayLength;
        if (newTime < 1f)
        {
            dayLength = 1f;
        }
        else
        {
            dayLength = newTime;
        }
        _completionTime = dayLength * 0.5f * 60;

        _breakdownChance += day * 0.02f;

        float cooldownMin = _cooldownTime - (5 + (day * 0.05f));
        float cooldownMax = _cooldownTime + (5 + (day * 0.05f));

        _cooldownTime = Random.Range(cooldownMin, cooldownMax);
    }

    void TrySetParticles(bool flag)
    {
        if (stationEffect != null)
        {
            if (flag)
            {
                if (!stationEffect.isPlaying)
                {
                    stationEffect.Play();
                }
            }
            else
            {
                if (!stationEffect.isStopped)
                {
                    stationEffect.Stop();
                }
            }
        }
    }


    public float GetProgress()
    {
        return _timeCompleted / _completionTime;
    }

    void ChanceStartState()
    {
        float brokenChance = 0.30f;

        float rand = Random.Range(0f, 1f);

        if (rand <= brokenChance)
        {
            TransitionToBroken();
        }
        else
        {
            state = StationState.WORKING;
        }
    }

    void PlayRandomSound()
    {
        int rand = Random.Range(0, 6);

        AudioManager.instance.PlaySFX(brokeSFX[rand]);
    }

    void TransitionToBroken()
    {
        Debug.Log("Station has broke down");
        TryAnimation(stationAnimator, brokenAnimation, true);
        TryAnimation(robotAnimator, brokenAnimation, true);
        TrySetParticles(false);

        state = StationState.BROKEN;
        _timeSinceAttemptedBreakdown = 0f;
        _canBreak = false;
        PlayRandomSound();
    }

    void RandomizeMinigame()
    {
        int rand;
        if (minigames.Count > 0)
        {
            rand = Random.Range(0, minigames.Count);
            activeMinigame = minigames[rand];
        }
    }
}
