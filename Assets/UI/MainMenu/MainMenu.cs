using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame() {
        SceneManager.LoadScene("BreakRoom");
    }

    public void ExitGame() {
        Application.Quit();
    }


}
