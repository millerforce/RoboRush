using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum PlayerState 
{ 
    RUNNING,
    IDLE,
    WALKING,
    INTERACTING,
    STARTING
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float WalkSpeed = 2f;

    [SerializeField]
    public float SprintSpeed = 3f;

    [SerializeField]
    private Transform _startLocation;

    private Animator animator;

    private Vector3 _moveDirection;

    private PlayerState state;

    public float rotationSpeed = 5f;

    public bool isPlayingMinigame = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveDirection = Vector2.zero;
        animator = gameObject.GetComponent<Animator>();
        state = PlayerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        if (!MovementNearZero(_moveDirection) && state != PlayerState.STARTING)
        {
            animator.SetBool("isInteracting", false);

            transform.rotation = Quaternion.LookRotation(_moveDirection);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                state = PlayerState.RUNNING;
            }
            else
            {
                state = PlayerState.WALKING;
            }
        }
        else if (state != PlayerState.INTERACTING && state != PlayerState.STARTING)
        {
            state = PlayerState.IDLE;
        }

        if (state == PlayerState.IDLE)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isInteracting", false);
        }
        else if (state == PlayerState.WALKING)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isInteracting", false);

            transform.Translate(WalkSpeed * Time.deltaTime * _moveDirection, Space.World);
        }
        else if (state == PlayerState.RUNNING)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isInteracting", false);

            transform.Translate(SprintSpeed * Time.deltaTime * _moveDirection, Space.World);
        }
        else if (state == PlayerState.INTERACTING)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);

            animator.SetBool("isInteracting", true);
        }
    }

    public bool StartAnimation()
    {
        if (!transform.position.Equals(_startLocation.position))
        {
            state = PlayerState.STARTING;
            animator.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, _startLocation.position, WalkSpeed * Time.deltaTime);

            return true;
        }
        else
        {
            state = PlayerState.IDLE;
            animator.SetBool("isWalking", false);
            return false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
         
            if (Input.GetKey(KeyCode.E))
            {
                state = PlayerState.INTERACTING;
            }
        }
        
    }

    private bool MovementNearZero(Vector3 movement)
    {
        return (movement.magnitude <= 0.6);
    }
}
