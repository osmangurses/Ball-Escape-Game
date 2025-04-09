using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public float speed;
    public float jump_force;
    private Rigidbody2D rb;
    [HideInInspector] public bool isGrounded;
    private SpriteRenderer sr;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    bool levelEnded = false;
    public bool controls_locked = false;
    bool isMoving = false;

    private bool isLaunchedByCannon = false;

    public Transform groundCheckOrigin;
    public Vector3 raycastDirection = new Vector3(1, 0, 0);

    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector2 moveInput;
    private bool jumpPressed;

    public PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput component bulunamad�! L�tfen objeye bir PlayerInput component ekleyin.");
            }
        }

        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
        }
    }

    private void OnEnable()
    {
        LevelActionManager.LevelEnded += (int levelIndex) => levelEnded = true;
        if (moveAction != null) moveAction.Enable();
        if (jumpAction != null) jumpAction.Enable();
    }

    private void OnDisable()
    {
        LevelActionManager.LevelEnded -= (int levelIndex) => levelEnded = true;
        if (moveAction != null) moveAction.Disable();
        if (jumpAction != null) jumpAction.Disable();
    }

    private void Start()
    {
        if (groundCheckOrigin == null)
        {
            Debug.LogWarning("Ground Check Origin atanmam��! L�tfen Inspector'dan bir referans objesi atay�n.");
        }
    }

    private void Update()
    {

        isGrounded = IsGrounded();
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }

        if (jumpAction != null)
        {
            jumpPressed = jumpAction.WasPressedThisFrame();
        }

        if (!levelEnded && !controls_locked)
        {
            float h_input = moveInput.x;

            if (jumpPressed && isGrounded)
            {
                AudioPlayer.instance.PlayAudio(AudioName.jump);
                rb.AddForce(transform.up * jump_force);
            }

            if (h_input > 0 && sr.flipX)
            {
                sr.flipX = false;
            }
            else if (h_input < 0 && !sr.flipX) { sr.flipX = true; }
            rb.linearVelocity = Vector3.up * rb.linearVelocity.y + h_input * speed * Vector3.right;

            if (isGrounded && rb.linearVelocity.x != 0)
            {
                if (!isMoving)
                {
                    isMoving = true;
                }
            }
            else
            {
                isMoving = false;
            }
        }
    }

    public void LaunchByCannon()
    {
        isLaunchedByCannon = true;
        controls_locked = true;
    }

    private bool IsGrounded()
    {
        if (groundCheckOrigin == null)
            return false;

        Vector2 rayOrigin = groundCheckOrigin.position;

        Vector2 rayDirection;
        if (!sr.flipX)
        {
            rayDirection = new Vector2(1, 0);
        }
        else 
        {
            rayDirection = new Vector2(-1, 0);
        }

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, groundCheckDistance, groundLayer);

        if (rb.linearVelocity.y < -5 && hit.collider != null)
        {
            
        }


        if (isLaunchedByCannon && controls_locked && hit.collider != null)
        {
            isLaunchedByCannon = false;
            controls_locked = false;

        }
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (groundCheckOrigin == null)
            return;

        Gizmos.color = Color.red;

        Vector3 rayOrigin = groundCheckOrigin.position;

        Vector3 rayEndRight = rayOrigin + Vector3.right * groundCheckDistance;
        Vector3 rayEndLeft = rayOrigin + Vector3.left * groundCheckDistance;
        Gizmos.DrawLine(rayOrigin, rayEndRight);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rayOrigin, rayEndLeft);
    }
}