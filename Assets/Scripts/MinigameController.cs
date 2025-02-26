using UnityEngine;
using UnityEngine.Events;

public class MinigameController : MonoBehaviour
{
    public UnityEvent OnMinigameCompleted;

    void Start()
    {
        // Initialize the minigame
    }

    public void CompleteMinigame()
    {
        // Logic for completing the minigame
        OnMinigameCompleted.Invoke();
        Destroy(gameObject); // Destroy the minigame instance
    }
}