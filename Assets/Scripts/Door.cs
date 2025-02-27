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
                InteractionHintManager.instance.HideHint();
                PlayerPrefs.SetInt("Day", _dayToStart);
                PlayerPrefs.Save();

                SceneManager.LoadScene(_levelToLoad);
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
