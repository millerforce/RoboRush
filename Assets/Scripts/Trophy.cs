using TMPro;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    public Camera trophyCamera;  // Assign in Inspector
    private Camera mainCamera;   // Auto-detect at runtime

    private bool isPlayerInside = false;
    private bool isViewingTrophy = false;

    private GameObject player;
    private Renderer[] playerRenderers;  // To store all of the player's Renderer components
    private MonoBehaviour playerMovementScript;  // Assuming the player has a movement script

    public TextMeshProUGUI totalWins;

    private void Start()
    {
        // Auto-find the Main Camera
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure it's tagged as 'MainCamera'.");
        }

        trophyCamera.enabled = false;  // Start with the trophy camera off

        totalWins.text = PlayerPrefs.GetInt("HighestDay", 0).ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionHintManager.instance.ShowHint();

            isPlayerInside = true;
            player = other.gameObject;
            playerRenderers = player.GetComponentsInChildren<Renderer>();  // Get all renderer components (e.g., body, weapons, etc.)
            playerMovementScript = player.GetComponent<MonoBehaviour>();  // Replace with the actual movement script

            if (playerRenderers.Length == 0)
            {
                Debug.LogError("Player has no Renderer components!");
            }
            if (playerMovementScript == null)
            {
                Debug.LogError("Player movement script not found!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionHintManager.instance.HideHint();

            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            ToggleCamera();
        }
    }

    private void ToggleCamera()
    {
        isViewingTrophy = !isViewingTrophy;

        if (isViewingTrophy)
        {
            Debug.Log("Switching to Trophy Camera");
            mainCamera.enabled = false;
            trophyCamera.enabled = true;

            InteractionHintManager.instance.HideHint();

            // Disable player movement and hide the player
            if (playerRenderers.Length > 0)
            {
                foreach (var renderer in playerRenderers)
                {
                    renderer.enabled = false;  // Hide the player by disabling the renderer
                }
            }
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false;  // Disable movement
            }
        }
        else
        {
            Debug.Log("Switching back to Main Camera");
            trophyCamera.enabled = false;
            mainCamera.enabled = true;

            InteractionHintManager.instance.ShowHint();

            // Enable player movement and show the player
            if (playerRenderers.Length > 0)
            {
                foreach (var renderer in playerRenderers)
                {
                    renderer.enabled = true;  // Show the player by enabling the renderer
                }
            }
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true;  // Enable movement
            }
        }
    }
}
