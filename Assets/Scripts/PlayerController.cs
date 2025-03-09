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
    public float SprintSpeed = 4f;

    [SerializeField]
    private Transform _startLocation;

    [SerializeField]
    private Collider playerCollider;

    private Rigidbody rigidbody;

    private Animator animator;

    private Vector3 _moveDirection;

    private PlayerState state;

    public float rotationSpeed = 5f;

    public bool isPlayingMinigame = false;

    private RigidbodyConstraints baseConstraints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveDirection = Vector2.zero;
        animator = gameObject.GetComponent<Animator>();
        state = PlayerState.IDLE;
        rigidbody = GetComponent<Rigidbody>();
        baseConstraints = rigidbody.constraints;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        if (!MovementNearZero(_moveDirection) && state != PlayerState.STARTING)
        { 
            transform.rotation = Quaternion.LookRotation(_moveDirection);

            IdleToMoving(Input.GetKey(KeyCode.LeftShift));
        }
        else if (state != PlayerState.INTERACTING && state != PlayerState.STARTING && state != PlayerState.IDLE)
        {
            MovingToIdle();
        }

        if (state == PlayerState.WALKING)
        {
            transform.Translate(WalkSpeed * Time.deltaTime * _moveDirection, Space.World);
        }
        else if (state == PlayerState.RUNNING)
        {
            transform.Translate(SprintSpeed * Time.deltaTime * _moveDirection, Space.World);
        }
        else if (state == PlayerState.IDLE || state == PlayerState.INTERACTING)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public bool StartAnimation()
    { 
        if (!transform.position.Equals(_startLocation.position))
        {
            playerCollider.enabled = false;
            state = PlayerState.STARTING;
            animator.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, _startLocation.position, WalkSpeed * Time.deltaTime);

            return true;
        }
        else
        {
            state = PlayerState.IDLE;
            animator.SetBool("isWalking", false);
            playerCollider.enabled = true;
            return false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
         
            if (Input.GetKey(KeyCode.E))
            {
                ToInteracting();
            }
        }
        
    }

    private bool MovementNearZero(Vector3 movement)
    {
        return (movement.magnitude <= 0.6);
    }

    private void MovingToIdle()
    {
        state = PlayerState.IDLE;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isInteracting", false);
    }

    private void IdleToMoving(bool isRunning)
    {
        rigidbody.constraints = baseConstraints;
        if (isRunning)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            state = PlayerState.RUNNING;
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            state = PlayerState.WALKING; 
        }
            animator.SetBool("isInteracting", false);
    }

    private void ToInteracting()
    {
        state = PlayerState.INTERACTING;
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);

        animator.SetBool("isInteracting", true);
    }
}
