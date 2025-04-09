using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SunAmmo : MonoBehaviour
{
    private Vector2 direction;
    private float speed;

    public void Initialize(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Start()
    {
        gameObject.tag = "Obstacle";

        if (GetComponent<CapsuleCollider2D>() == null)
        {
            gameObject.AddComponent<CapsuleCollider2D>();
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        transform.DOScale(Vector3.one,0.05f);
        rb.isKinematic = true;
        rb.gravityScale = 0;
    }

    void Update()
    {
        Vector3 movement = -transform.up * speed * Time.deltaTime;
        transform.position += movement;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Sun>() != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sun>() != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}