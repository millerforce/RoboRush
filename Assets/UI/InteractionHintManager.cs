using UnityEngine;

public class InteractionHintManager : MonoBehaviour {
    public static InteractionHintManager instance;

    public GameObject hint;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        if (hint != null) {
            hint.SetActive(false);
        }
    }

    public void ShowHint() {
        hint.SetActive(true);
    }


    public void HideHint() {
        hint.SetActive(false);
    }

}
