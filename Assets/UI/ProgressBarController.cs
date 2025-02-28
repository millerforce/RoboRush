using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Image progressBarFill;  // Reference to the UI Image (fill)
    public Workstation workstation;
    private float currentProgress = 0f; // Current progress (0 - 1)

    void Update()
    {
        // Simulate progress (Replace this with actual logic)
        currentProgress = workstation.GetProgress(); // Increment over time
        currentProgress = Mathf.Clamp01(currentProgress); // Keep value between 0 and 1
        UpdateProgress(currentProgress);
    }

    public void UpdateProgress(float progress)
    {
        if (progressBarFill != null)
        {
            progressBarFill.fillAmount = progress; // Updates fill amount (0 to 1)
        }
    }
}
