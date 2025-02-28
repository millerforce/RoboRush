using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame() {
        SceneManager.LoadScene("BreakRoom");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame() {
        Application.Quit();
    }

}
