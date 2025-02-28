using UnityEngine;

public class Robot : MonoBehaviour
{
    private Animator animator;

    // Animation state name
    public string animationStateName = "Working";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        // Generate a random normalized time between 0 and 1
        float randomCycleOffset = Random.Range(0f, 1f);

        // Play the animation at the random cycle offset
        animator.Play(animationStateName, 0, randomCycleOffset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
