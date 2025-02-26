using UnityEngine;

public class OptionsMenu : MonoBehaviour {


    public GameObject optionsMenu;

    public void OpenOptions() {
        optionsMenu.SetActive(true);
    }

    public void CloseOptions() {
        optionsMenu.SetActive(false);
    }
}
