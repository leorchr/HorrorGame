using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public CharacterController controller;

    public float speed = 5f;
    public float gravity = -10f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;
    public float moveSmoothTime = 0.2f;
    bool isGrounded;

    Vector2 movement;
    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;
    }

    void Update()
    {
        float x = movement.x;
        float z = movement.y;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        currentMoveVelocity = Vector3.SmoothDamp(
            currentMoveVelocity,
            move * speed,
            ref moveDampVelocity,
            moveSmoothTime);
        controller.Move(currentMoveVelocity * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
