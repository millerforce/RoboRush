using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float WalkSpeed = 1f;
    [SerializeField]
    private float _sprintSpeed = 3f;

    private Vector3 _moveDirection;

    [SerializeField]
    private Transform _startLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        float speed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : WalkSpeed;

        transform.Translate(_moveDirection * speed * Time.deltaTime);
    }

    public bool StartAnimation()
    {
        if (!transform.position.Equals(_startLocation.position))
        {

            transform.position = Vector3.MoveTowards(transform.position, _startLocation.position, WalkSpeed * Time.deltaTime);

            return true;
        }
        else
        {
            return false;
        }
    }


}
