using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Vector2 playerInput;
    float moveSpeed;
    public float walkSpeed, runSpeed;
    GameObject playerParent;
    bool isRunningKey, isRunningStick, isRunning;
    Animator anim;

    bool jump, jumped;
    public float gravity = 9.8f;
    public float gravityMultiplier = 1f;
    public float jumpForce = 50f;

    Rigidbody rb;

    [Header("Camera Parameters")]
    Transform mainCamera;
    Transform camTransform;
    public Vector3 distance;
    [SerializeField] Transform lookAt;
    [SerializeField] Transform playerOrientation;
    public float turnSpeed = 2.0f;

    bool canSwipe;
    Vector2 touchScreenInitialPos, touchScreenTemporaryPos, touchScreenCurrentPos;
    Vector2 direction;
    float dir;
    Vector3 offset;
    public float cameraTurnSpeed = 4.0f;

    Vector3 lookAtInitialPosition;
    public float minHeight, maxHeight;

    Vector3 offsetY;
    Vector2 mousePos, mouseTemporaryPos;
    public bool isPc, isConsole, isHandheld;

    bool canJump = true; // Variable to control jumping cooldown

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        camTransform = GameObject.FindGameObjectWithTag("camTransform").transform;
        rb = GetComponent<Rigidbody>();

        lookAtInitialPosition = lookAt.localPosition;

        offset = distance;
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Running(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunningKey = true;
        }
        else if (context.canceled)
        {
            isRunningKey = false;
        }
    }

    private void Update()
    {
        MovePlayer();

        if (playerInput != Vector2.zero)
        {
            moveSpeed = walkSpeed;
        }

        if (isRunningKey || isRunningStick)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (isHandheld)
        {
            TouchScreenRotation();
        }

        if (isPc)
        {
            MouseRotation();
        }
    }

    private void LateUpdate()
    {
        offset = Quaternion.AngleAxis(dir * cameraTurnSpeed, Vector3.up) * offset;

        mainCamera.position = transform.position + offset;
        camTransform.position = transform.position + offset;

        mainCamera.LookAt(lookAt.position);
        camTransform.LookAt(new Vector3(lookAt.position.x, camTransform.position.y, lookAt.position.z));

        lookAt.localPosition = new Vector3(lookAt.localPosition.x, Mathf.Clamp(lookAt.localPosition.y, lookAtInitialPosition.y - minHeight,
        lookAtInitialPosition.y + maxHeight), lookAt.localPosition.z);

        lookAt.localPosition += offsetY * Time.deltaTime;
    }

    void MovePlayer()
    {
        Vector3 forwardDirection = camTransform.forward;
        playerOrientation.forward = forwardDirection.normalized;

        Vector3 moveVector = playerInput.x * playerOrientation.right + playerInput.y * playerOrientation.forward;
        moveVector.Normalize();

        if (isRunning && moveVector != Vector3.zero)
        {
            if (!jump)
            {
                anim.SetBool("run", true);
                anim.SetBool("walk", false);
                anim.SetBool("idle", false);
            }
            moveSpeed = runSpeed;
        }
        else if (!isRunning && moveVector != Vector3.zero)
        {
            if (!jump)
            {
                anim.SetBool("walk", true);
                anim.SetBool("run", false);
                anim.SetBool("idle", false);
            }
            moveSpeed = walkSpeed;
        }

        if (moveVector == Vector3.zero && !jump)
        {
            anim.SetBool("idle", true);
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
        }

        if (moveVector != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(camTransform.forward, moveVector, Time.deltaTime * turnSpeed);
        }

        playerParent.transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
    }

    public void SetIsRunning(bool _isRunning)
    {
        isRunningStick = _isRunning;
    }

    public void OnScreenTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            canSwipe = true;
        }

        if (context.canceled)
        {
            canSwipe = false;
        }
    }

    public void TouchScreenInitialPosition(InputAction.CallbackContext context)
    {
        touchScreenInitialPos = context.ReadValue<Vector2>();
        touchScreenCurrentPos = touchScreenInitialPos;
    }

    public void TouchScreenCurrentPosition(InputAction.CallbackContext context)
    {
        touchScreenCurrentPos = context.ReadValue<Vector2>();
    }

    public void MousePosRotation(InputAction.CallbackContext _context)
    {
        mousePos = _context.ReadValue<Vector2>();
    }

    public void JumpButton(InputAction.CallbackContext _context)
    {
        if (_context.performed && canJump) // Check if jumping is allowed
        {
            jump = true;
            Jump();
            canJump = false; // Disable jumping temporarily
            StartCoroutine(ResetJumpCooldown()); // Start coroutine to reset jump cooldown
        }
    }

    IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(0.1f); // Adjust cooldown duration as needed
        canJump = true; // Re-enable jumping after cooldown
    }

    void Jump()
    {
        if (jump && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) // Check if not already jumping
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);

            anim.SetBool("idle", false);
            anim.SetBool("walk", false);
            anim.SetTrigger("jump");

            // Set jump to false after the jump
            jump = false;
        }
    }

    void TouchScreenRotation()
    {
        if (touchScreenInitialPos.x >= Screen.width / 2 && canSwipe)
        {
            if (touchScreenCurrentPos.x >= Screen.width / 2)
            {
                direction = touchScreenCurrentPos - touchScreenTemporaryPos;
            }
            else
            {
                direction = Vector2.zero;
            }

            dir = direction.x > 0 ? 1f : (direction.x < 0 ? -1f : 0f);
            offsetY = new Vector3(0, direction.y > 0 ? 1 : (direction.y < 0 ? -1 : 0), 0);

            touchScreenTemporaryPos = touchScreenCurrentPos;
        }
        else if (!canSwipe)
        {
            dir = 0f;
            offsetY = Vector3.zero;
        }
    }

    void MouseRotation()
    {
        direction = mousePos - mouseTemporaryPos;

        dir = direction.x > 0 ? 1f : (direction.x < 0 ? -1f : 0f);
        offsetY = new Vector3(0, direction.y > 0 ? 1 : (direction.y < 0 ? -1 : 0), 0);

        mouseTemporaryPos = mousePos;
    }

    public void Jumped()
    {
        jumped = true;
    }
}
