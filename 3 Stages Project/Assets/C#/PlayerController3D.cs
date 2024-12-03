using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterController3D : MonoBehaviour
{
    [Header("Player Settings")]
    public float movementSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;  // Rotation speed for smooth turning
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController characterController;
    private Vector3 velocity;

    // Input values
    private Vector2 movementInput;
    private bool jumpInput;

    // Animator
    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        UpdateAnimation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && characterController.isGrounded)
        {
            jumpInput = true;
        }
    }

    private void HandleMovement()
    {
        // Get the camera's forward and right vectors to orient movement
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;  // Remove any vertical component (keep only horizontal direction)
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Determine movement direction based on input and camera orientation
        Vector3 moveDirection = (forward * movementInput.y + right * movementInput.x).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // Rotate the character smoothly towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the character based on the direction
            float speed = (movementInput.y > 0.1f || movementInput.x != 0) ? movementSpeed : 0f;
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (characterController.isGrounded && jumpInput)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpInput = false;
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        // Calculate speed for the Blend Tree
        float speed = movementInput.magnitude;

        // Update the 'Speed' parameter in the Animator (for Blend Tree)
        animator.SetFloat("Speed", speed);

        // Set jumping state
        if (!characterController.isGrounded)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }
}
