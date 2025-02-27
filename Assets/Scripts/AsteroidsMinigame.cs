using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class AsteroidMinigame : MonoBehaviour
{
    [SerializeField] public float speed = 20f;
    [SerializeField] int maxAsteroids;
    [SerializeField] int minAsteroids;
    [SerializeField] public int maxLevel; // At what level will the max difficulty of the game happen
    [SerializeField] public Texture asteroidTexture; // This should be a reference to the PNG texture

    public RectTransform spawnPlatform; // The area from which asteroids will spawn
    public Canvas canvas; // A reference to the canvas where the RawImage will be instantiated

    class Asteroid
    {
        public RawImage image;
        public Vector3 position;
        public Vector2 direction;

        public Asteroid(Texture texture, Vector3 pos, Vector2 dir, Canvas canvas)
        {
            // Create a new GameObject and attach a RawImage component
            GameObject asteroidObject = new GameObject("Asteroid");
            asteroidObject.transform.SetParent(canvas.transform);

            // Add RawImage component and set its texture
            image = asteroidObject.AddComponent<RawImage>();
            image.texture = texture; // Directly assign the Texture to the RawImage's texture

            // Set position and direction
            position = pos;
            direction = dir;

            // Optional: Adjust size and position
            image.rectTransform.sizeDelta = new Vector2(50, 50); // Size of the asteroid
            image.rectTransform.anchoredPosition = pos; // Position relative to the canvas
        }
    }

    List<Asteroid> asteroids = new List<Asteroid>();

    int determineAmountOfAsteroids(int day)
    {
        return Mathf.FloorToInt(Mathf.Lerp(minAsteroids, maxAsteroids, day / (float)maxLevel));
    }

    // Function to get a random spawn position along the edge of the spawn platform
    Vector3 getRandomSpawn()
    {
        RectTransform panelRectTransform = spawnPlatform.GetComponent<RectTransform>(); // Reference the spawning panel

        // Get panel dimensions
        float panelWidth = panelRectTransform.rect.width;
        float panelHeight = panelRectTransform.rect.height;

        // Safe padding to prevent overlap
        float padding = 0f;

        // Randomly choose an edge (0 = top, 1 = bottom, 2 = left, 3 = right)
        int edge = UnityEngine.Random.Range(0, 4);

        Vector3 spawnPosition = Vector3.zero;

        // Choose random position along the selected edge
        switch (edge)
        {
            case 0: // Top edge
                float topX = UnityEngine.Random.Range(-panelWidth / 2 + padding, panelWidth / 2 - padding);
                spawnPosition = new Vector3(topX, panelHeight / 2 - padding, 0);
                break;

            case 1: // Bottom edge
                float bottomX = UnityEngine.Random.Range(-panelWidth / 2 + padding, panelWidth / 2 - padding);
                spawnPosition = new Vector3(bottomX, -panelHeight / 2 + padding, 0);
                break;

            case 2: // Left edge
                float leftY = UnityEngine.Random.Range(-panelHeight / 2 + padding, panelHeight / 2 - padding);
                spawnPosition = new Vector3(-panelWidth / 2 + padding, leftY, 0);
                break;

            case 3: // Right edge
                float rightY = UnityEngine.Random.Range(-panelHeight / 2 + padding, panelHeight / 2 - padding);
                spawnPosition = new Vector3(panelWidth / 2 - padding, rightY, 0);
                break;
        }

        return spawnPosition; // Return local position directly
    }

    // Function to get the movement direction based on the spawn position
    Vector2 getDirectionFromSpawn(Vector3 spawnPosition)
    {
        RectTransform panelRectTransform = spawnPlatform.GetComponent<RectTransform>(); // Reference the spawning panel

        // Get panel dimensions
        float panelWidth = panelRectTransform.rect.width;
        float panelHeight = panelRectTransform.rect.height;

        Vector2 direction = Vector2.zero;

        // Determine which edge the spawn position is on, and calculate the direction
        if (Mathf.Approximately(spawnPosition.y, panelHeight / 2)) // Top edge
        {
            direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), -1f); // Move down with random horizontal direction
        }
        else if (Mathf.Approximately(spawnPosition.y, -panelHeight / 2)) // Bottom edge
        {
            direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), 1f); // Move up with random horizontal direction
        }
        else if (Mathf.Approximately(spawnPosition.x, -panelWidth / 2)) // Left edge
        {
            direction = new Vector2(1f, UnityEngine.Random.Range(-1f, 1f)); // Move right with random vertical direction
        }
        else if (Mathf.Approximately(spawnPosition.x, panelWidth / 2)) // Right edge
        {
            direction = new Vector2(-1f, UnityEngine.Random.Range(-1f, 1f)); // Move left with random vertical direction
        }

        return direction;
    }

    void spawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Get random spawn position and direction
            Vector3 spawnPosition = getRandomSpawn();
            Vector2 direction = getDirectionFromSpawn(spawnPosition);

            // Create an Asteroid object
            Asteroid asteroid = new Asteroid(asteroidTexture, spawnPosition, direction, this.canvas);
            asteroids.Add(asteroid);
        }
    }

    void Start()
    {
        int day = PlayerPrefs.GetInt("Day");
        if (day < 1) day = 1;
        Debug.Log($"amount of Asteroids: {determineAmountOfAsteroids(day)}");
        spawnAsteroids(determineAmountOfAsteroids(day));
    }

    void Update()
    {
        foreach (Asteroid asteroid in asteroids)
        {
            // Move asteroid
            asteroid.position += (Vector3)asteroid.direction * speed * Time.deltaTime;

            // Apply new position to the UI element
            asteroid.image.rectTransform.position = asteroid.position;
        }
    }

}
