using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed = 1f;

    private Vector3 _moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.z = Input.GetAxis("Vertical");

        transform.Translate(_moveDirection * _walkSpeed * Time.deltaTime);
    }
}
