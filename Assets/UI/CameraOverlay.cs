using UnityEngine;

public class CameraOverlay : MonoBehaviour {

    [SerializeField]
    private GameObject recOverlay;

    private static float LAST_BLINK_DEFAULT = -1f;
    private static float BLINK_DELAY = 1.5f;
    private float lastBlinkUpdate = LAST_BLINK_DEFAULT;
    private bool recOverlayShowwing = true;

    void Start() {
        lastBlinkUpdate = Time.time;
    }

    void Update() {
        if (Time.time - lastBlinkUpdate >= BLINK_DELAY) {
            recOverlay.SetActive(!recOverlayShowwing);
            lastBlinkUpdate = Time.time;
            recOverlayShowwing = !recOverlayShowwing;
        }
    }
}
