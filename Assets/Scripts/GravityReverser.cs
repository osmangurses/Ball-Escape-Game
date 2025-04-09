using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GravityReverser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = -playerRigidbody.gravityScale;
                collision.gameObject.transform.eulerAngles += Vector3.forward * 180;
            }
        }
    }
}