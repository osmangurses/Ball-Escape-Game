using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class BeeEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;          
    private SpriteRenderer spriteRenderer; 

    [Header("Movement Settings")]
    public float moveSpeed = 3f;     

    [Header("Detection Settings")]
    public float detectionRange = 10f; 
    public LayerMask detectionMask;    

    [Header("Death Settings")]
    public float fadeOutDuration = 1.0f;
    public float destroyDelay = 0.2f;    

    private bool canSeePlayer = false;
    private Vector2 directionToPlayer;
    private RaycastHit2D hit;
    private bool isDead = false;   

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer bileþeni bulunamadý!");
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player bulunamadý! Player objesine 'Player' tag'i atadýðýnýzdan emin olun.");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Death");
            }

            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Death");
            }

            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Bee öldü!");

        AudioPlayer.instance.PlayAudio(AudioName.explosion);
        moveSpeed = 0;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.DOFade(0, fadeOutDuration).SetEase(Ease.InQuad).OnComplete(() => {
                Destroy(gameObject, destroyDelay);
            });
        }
        else
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    void Update()
    {
        if (isDead) return;

        if (player == null)
            return;

        directionToPlayer = (player.position - transform.position).normalized;

        hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, detectionMask);


        if (hit.collider != null)
        {

            if (hit.collider.transform == player)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }

        if (canSeePlayer)
        {

            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);

            }

            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void OnDrawGizmos()
    {
        if (player == null)
            return;

        Vector2 direction = player ? (player.position - transform.position).normalized : Vector2.right;
        float distance = player ? Vector2.Distance(transform.position, player.position) : detectionRange;

        Gizmos.color = canSeePlayer ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * Mathf.Min(distance, detectionRange)));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (hit.collider != null)
        {
            Gizmos.color = hit.collider.transform == player ? Color.green : Color.blue;
            Gizmos.DrawSphere(hit.point, 0.2f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.gray;
            Gizmos.DrawRay(transform.position, Vector2.right * detectionRange);
        }
    }
}