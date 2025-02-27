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
        print("door trigger entered");
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                PlayerPrefs.SetInt("Day", _dayToStart);
                PlayerPrefs.Save();

                SceneManager.LoadScene(_levelToLoad);
            }
        }
    }
}
