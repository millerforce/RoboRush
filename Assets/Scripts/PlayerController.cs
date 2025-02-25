using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float WalkSpeed = 2f;
    [SerializeField]
    private float _sprintSpeed = 3f;

    
    private Animator animator;

    private Vector3 _moveDirection;

    [SerializeField]
    private Transform _startLocation;

    public float rotationSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveDirection = Vector2.zero;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        float speed;
        bool isSprinting;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = _sprintSpeed;
            isSprinting = true;
        }
        else
        {
            speed = WalkSpeed;
            isSprinting = false;
        }

        animator.SetBool("isRunning", isSprinting);
        animator.SetBool("isWalking", !isSprinting);

        transform.Translate(speed * Time.deltaTime * _moveDirection, Space.World);

        if (_moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_moveDirection);
        }
        
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
