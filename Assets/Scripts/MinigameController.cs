using UnityEngine;
using UnityEngine.Events;

public class MinigameController : MonoBehaviour
{
    public GameObject[] minigamePrefabs; // Array of minigame prefabs
    public UnityEvent OnMinigameCompleted; // Event to notify when the minigame is completed

    private GameObject currentMinigame;

    public void TriggerMinigame(int minigameIndex)
    {
        if (minigameIndex >= 0 && minigameIndex < minigamePrefabs.Length)
        {
            currentMinigame = Instantiate(minigamePrefabs[minigameIndex]);
            MinigameBase minigameBase = currentMinigame.GetComponent<MinigameBase>();
            minigameBase.OnMinigameCompleted.AddListener(OnMinigameCompletedHandler);
        }
    }

    private void OnMinigameCompletedHandler()
    {
        OnMinigameCompleted?.Invoke();
        Destroy(currentMinigame);
    }
}