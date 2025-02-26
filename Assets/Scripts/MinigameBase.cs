using UnityEngine;
using UnityEngine.Events;

public class MinigameBase : MonoBehaviour
{
    public UnityEvent OnMinigameCompleted; // Event to notify when the minigame is completed

    protected void CompleteMinigame()
    {
        OnMinigameCompleted?.Invoke();
    }
}