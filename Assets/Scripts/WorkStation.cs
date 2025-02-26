using UnityEngine;

public enum StationState
{
    WORKING,
    PAUSED,
    FINISHED
}

public class Workstation : MonoBehaviour
{
    private ParticleSystem takeoff;
    private StationState state;

    [SerializeField]
    private float _completionTime;

    private float _timeCompleted;

    private static float exitHeight = 20f;

    [SerializeField]
    private float takeoffSpeed = 5f;

    [SerializeField]
    MinigameController minigame;

    private void Start()
    {
        takeoff = GetComponentInChildren<ParticleSystem>();
        takeoff.Stop();
        state = StationState.WORKING;
        _timeCompleted = 0f;
    }

    private void Update()
    {
        switch (state)
        {
            case StationState.WORKING:

                _timeCompleted += Time.deltaTime;

                Debug.Log($"Time passed {_timeCompleted}");

                if (_timeCompleted >= _completionTime)
                {
                    state = StationState.FINISHED;
                }

                break;

            case StationState.FINISHED:

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

        if (transform.position.y < exitHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + takeoffSpeed * Time.deltaTime, transform.position.z);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
