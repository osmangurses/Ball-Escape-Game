using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float rotationSpeed = 360f;

    public float raycastLength = 0.5f;

    public float raycastOffset = 0.5f;

    private int direction = 1;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        AudioPlayer.instance.PlayAudio(AudioName.circular_saw);
    }
    void Update()
    {
        RaycastChecks();

        MoveObject();

        RotateSaw();
    }

    void RaycastChecks()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * raycastOffset, Vector2.right, raycastLength);
        
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * raycastOffset, Vector2.left, raycastLength);

        if (hitRight.collider != null && hitRight.collider.gameObject.tag != "Player")
        {
            direction = -1;
        }

        if (hitLeft.collider != null && hitLeft.collider.gameObject.tag != "Player")
        {
            direction = 1;
        }
    }

    void MoveObject()
    {
        rb.velocity=Vector3.right * direction * moveSpeed +rb.velocity.y*Vector3.up;
    }

    void RotateSaw()
    {
        transform.Rotate(0, 0, -rotationSpeed * direction * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right * raycastOffset, transform.position + Vector3.right * (raycastOffset + raycastLength));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.left * raycastOffset, transform.position + Vector3.left * (raycastOffset + raycastLength));
    }
}
