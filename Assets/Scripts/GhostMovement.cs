using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostMovement : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private List<PlayerStates> movementStates;
    public float acceleration = 8f;
    public float maxSpeed = 5f;
    public float drag = 4f;
    

    private Rigidbody2D rb;
    Vector2 movementVector;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public Vector2 ApplyMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(inputX, inputY);
        
        // Normalize input only if it's not zero to maintain consistent speed
        if (input.magnitude > 1)
            input.Normalize();

        // Accelerate towards input direction
        if (input.magnitude > 0)        
            movementVector += input * acceleration * Time.deltaTime;
        
        else        
            // Apply drag when no input
            movementVector = Vector2.Lerp(movementVector, Vector2.zero, drag * Time.deltaTime);
        

        // Clamp velocity to max speed
        movementVector = Vector2.ClampMagnitude(movementVector, maxSpeed);

        rb.linearVelocity = movementVector;

        return movementVector;
    }

    private void Update()
    {
        // Set to Zero for states we are not supposed to be moving in
        if (!movementStates.Contains(stateMachine.GetPlayerStateEnum()) && (rb.linearVelocity != Vector2.zero || movementVector != Vector2.zero))
        {
            movementVector = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
        
    }
}