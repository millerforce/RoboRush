using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AsteroidMinigame : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField] int maxAsteroids;
    [SerializeField] int minAsteroids;
    [SerializeField] public int maxLevel; // At what level will the max difficulty of the game happen
    [SerializeField] public Texture asteroidTexture; // This should be a reference to the PNG texture

    public RectTransform spawnPlatform; // The area from which asteroids will spawn

    class Asteroid
    {
        public RawImage image;
        public Vector3 position;
        public Vector2 direction;

        public Asteroid(RawImage img, Vector3 pos, Vector2 dir)
        {
            image = img;
            position = pos;
            direction = dir;
        }
    }

    List<Asteroid> asteroids = new List<Asteroid>();

    int determineAmountOfAsteroids(int day)
    {
        return Mathf.FloorToInt(Mathf.Lerp(minAsteroids, maxAsteroids, day / (float)maxLevel));
    }

    void spawnAsteroids(int amount)
    {
        
    }

    void Start()
    {
        int day = PlayerPrefs.GetInt("Day");
        spawnAsteroids(determineAmountOfAsteroids(day));
    }

    void Update()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            Asteroid asteroid = asteroids[i];
            asteroid.position += (Vector3)asteroid.direction * speed * Time.deltaTime;
        }
    }
}
