using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private string _levelToLoad = "BreakRoom";

    [SerializeField]
    private int _dayToStart = 1;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                if (_levelToLoad.Equals("Endless"))
                {
                    PlayerPrefs.SetInt("Endless", _levelToLoad.Equals("Endless") ? 1 : 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("Day", _dayToStart);
                    PlayerPrefs.Save();
                }

                int highest = PlayerPrefs.GetInt("HighestDay", 1);
                int day = PlayerPrefs.GetInt("Day", 1);
                if (highest <= day)
                {
                    PlayerPrefs.SetInt("HighestDay", day);
                    PlayerPrefs.Save();
                }

                InteractionHintManager.instance.HideHint();
                
                StartCoroutine(GameObject.FindFirstObjectByType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, _levelToLoad));
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InteractionHintManager.instance.ShowHint();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InteractionHintManager.instance.HideHint();
        }
    }
}
