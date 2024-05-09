using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;

    public GameObject camHolder;

    public Animator anim;

    public float speed;
    public float sensitivity;
    public float maxForce;
    public float jumpForce;
    public float sprintSpeed;
    public float crouchSpeed;

    private float lookRotation;
    

    public bool grounded;

    private bool isSprinting;
    private bool isCrouching;
    
    private Vector2 move;
    private Vector2 look;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        anim.SetBool("isCrouching", isCrouching);
    }


    void FixedUpdate()
    {
        Move();
        
    }

    void LateUpdate()
    {
        Look();
    }

    private void Jump()
    {
        Vector3 jumpForces = rb.velocity;

        if (grounded)
        {
            jumpForces.y = jumpForce;
        }

        rb.velocity = jumpForces;

    }

    


    public void SetGrounded(bool state)
    {
        grounded = state;
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * look.x * sensitivity);

        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    private void Move()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed); // conditional operator statement, isSprinting (true)? (if yes) sprintSpeed : (if no) speed;
        // using it twice with brackets, this lets you nest it as far as I can tell

        targetVelocity = transform.TransformDirection(targetVelocity);


        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

    }

    
    

    


    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context) 
    {
        isSprinting = context.ReadValueAsButton();
    }
    
    public void OnCrouch(InputAction.CallbackContext context) 
    {
        isCrouching = context.ReadValueAsButton();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    


}
