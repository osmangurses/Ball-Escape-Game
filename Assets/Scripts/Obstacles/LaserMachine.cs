using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachine : MonoBehaviour
{
    public SpriteRenderer lazerSprite; 
    public float raycastDistance = 10f;
    public Vector2 raycastOffset = new Vector2(0f, 0.33f); 

    private RaycastHit2D hit;
    private void Start()
    {
        AudioPlayer.instance.PlayAudio(AudioName.lazer);
    }
    void Update()
    {
        Vector2 direction = transform.up; 

        int layerMask = ~(1 << LayerMask.NameToLayer("Laser")); 

        Vector2 raycastOrigin = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)raycastOffset);

        hit = Physics2D.Raycast(raycastOrigin, direction, raycastDistance, layerMask);

        if (hit.collider != null)
        {
            DrawLazer(transform.position, hit.point);
        }
        else
        {
            DrawLazer(transform.position, raycastOrigin + direction * raycastDistance);
        }
    }

    void DrawLazer(Vector2 start, Vector2 end)
    {
        Vector2 difference = end - start;
        float distance = difference.magnitude;

        lazerSprite.transform.position = (start + end) / 2;

        lazerSprite.transform.up = difference.normalized;

        lazerSprite.transform.localScale = new Vector3(lazerSprite.transform.localScale.x, distance, lazerSprite.transform.localScale.z);

    }

    void OnDrawGizmos()
    {
        Vector2 direction = transform.up;

        Vector2 raycastOrigin = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)raycastOffset);

        if (hit.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(raycastOrigin, hit.point); 
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(raycastOrigin, raycastOrigin + direction * raycastDistance);
        }
    }
}
