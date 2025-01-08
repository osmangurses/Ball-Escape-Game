using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed;
    public float jump_force;
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer sr;

    public float groundCheckOffset = 0.1f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    bool levelEnded = false;
    public bool controls_locked = false;
    bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        LevelActionManager.LevelEnded += (int levelIndex) => levelEnded = true;
    }
    private void OnDisable()
    {

        LevelActionManager.LevelEnded -= (int levelIndex) => levelEnded = true;
    }
    private void Update()
    {
        if (!levelEnded && !controls_locked)
        {
            float h_input = ControlManager.instance.x_input;
            if (ControlManager.instance.GetJump() && isGrounded)
            {
                AudioPlayer.instance.PlayAudio(AudioName.jump);
                rb.AddForce(Vector3.up * jump_force);
            }
            if (h_input > 0 && sr.flipX)
            {
                sr.flipX = false;
            }
            else if (h_input < 0 && !sr.flipX) { sr.flipX = true; }
            rb.velocity = Vector3.up * rb.velocity.y + h_input * speed * Vector3.right;

            isGrounded = IsGrounded();
            if (isGrounded && rb.velocity.x != 0)
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

    // Raycast ile zemine temas kontrolü
    private bool IsGrounded()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - groundCheckOffset);

        // Sadece groundLayer (Block layer'ý) ile çarpýþmayý kontrol ediyoruz
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        if (rb.velocity.y<-5 && hit.collider != null)
        {
        }
        // Raycast bir nesneye çarptýysa ve bu nesne "Block" layer'ýnda ise zeminde kabul edilir
        return hit.collider != null;
    }

    // Zemine temas eden raycast'in çizilmesini saðlar (Görsel Debug için)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - groundCheckOffset);
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * groundCheckDistance);
    }
}