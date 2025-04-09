using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("Jumper Settings")]
    public float jumpForce = 10f;      
    public Vector2 jumpDirection = Vector2.up; 
    public bool resetPlayerVelocity = true; 

    [Header("Animation")]
    public string jumpAnimationName = "Jump"; 
    public float animationDelay = 0.1f;      

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Jumper objesinde Animator bileþeni bulunamadý!");
        }

        jumpDirection.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayJumpAnimation();

            StartCoroutine(JumpPlayerWithDelay(collision.gameObject));
        }
    }

    private void PlayJumpAnimation()
    {
        if (animator != null)
        {
            animator.Play(jumpAnimationName);
            Debug.Log("Jumper animasyonu oynatýlýyor: " + jumpAnimationName);
        }
    }

    private IEnumerator JumpPlayerWithDelay(GameObject player)
    {
        yield return new WaitForSeconds(animationDelay);

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        AudioPlayer.instance.PlayAudio(AudioName.jump);

        if (playerRb != null)
        {
            if (resetPlayerVelocity)
            {
                playerRb.velocity = Vector2.zero;
            }

            playerRb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Player zýplatýldý! Kuvvet: " + (jumpDirection * jumpForce));

            Animator playerAnimator = player.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                try
                {
                    playerAnimator.Play("Jump");
                    Debug.Log("Player Jump animasyonu çalýþtýrýldý");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Player Jump animasyonu çalýþtýrýlamadý: " + e.Message);
                }
            }
        }
        else
        {
            Debug.LogError("Player objesinde Rigidbody2D bileþeni bulunamadý!");
        }
    }
}