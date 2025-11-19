using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostMovement : MonoBehaviour
{
    public float acceleration = 8f;
    public float maxSpeed = 5f;
    public float drag = 4f;
    [SerializeField] private PlayerStateMachine stateMachine;

    private Vector2 velocity;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Ensure no gravity
    }

    void Update()
    {
        var currentState = (PlayerStates)stateMachine.GetCurrentState();
        if (currentState == PlayerStates.Locomotion || currentState == PlayerStates.Idle)
        {
            // Get input
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector2 input = new Vector2(inputX, inputY);

            // Normalize input only if it's not zero to maintain consistent speed
            if (input.magnitude > 1)
            {
                input.Normalize();
            }

            // Accelerate towards input direction
            if (input.magnitude > 0)
            {
                velocity += input * acceleration * Time.deltaTime;
            }
            else
            {
                // Apply drag when no input
                velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);
            }

            // Clamp velocity to max speed
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            velocity = Vector2.zero;
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = velocity;
    }
}