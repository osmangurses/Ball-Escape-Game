using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DestructibleBlock : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float respawnDelay = 3f;

    private SpriteRenderer spriteRenderer;
    private Collider2D blockCollider;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        blockCollider = GetComponent<Collider2D>();
        originalColor = spriteRenderer.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteRenderer.DOComplete();
            StartCoroutine(DestroyAndRespawn());
        }
    }

    private IEnumerator DestroyAndRespawn()
    {
        spriteRenderer.DOFade(0, fadeOutDuration);

        yield return new WaitForSeconds(fadeOutDuration);
        blockCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        spriteRenderer.DOFade(originalColor.a, fadeOutDuration);
        blockCollider.enabled = true;
    }
}