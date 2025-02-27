using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 direction;

    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Bounce off walls
        if (transform.position.x > Camera.main.orthographicSize || transform.position.x < -Camera.main.orthographicSize)
        {
            direction.x = -direction.x;
        }
        if (transform.position.y > Camera.main.orthographicSize || transform.position.y < -Camera.main.orthographicSize)
        {
            direction.y = -direction.y;
        }
    }
}