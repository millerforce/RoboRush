using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private string _levelToLoad = "BreakRoom";

    private void OnTriggerStay(Collider other)
    {
        print("door trigger entered");
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                SceneManager.LoadScene(_levelToLoad);
            }
        }
    }
}
