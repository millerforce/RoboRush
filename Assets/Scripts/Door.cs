using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private string _levelToLoad;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E)) 
            {

                FactoryController.SetHighScore();

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
