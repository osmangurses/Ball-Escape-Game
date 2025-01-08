using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    // Hareket hýzýný belirleyelim
    public float moveSpeed = 5f;

    // Dönme hýzý
    public float rotationSpeed = 360f; // saniyede kaç derece dönecek

    // Raycast'in uzunluðu
    public float raycastLength = 0.5f;

    // Sað ve sol için offset (mesafe)
    public float raycastOffset = 0.5f;

    // Hangi yöne doðru hareket edeceðimizi belirleyelim: 1 saða, -1 sola
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
        // Raycast'leri yönetecek fonksiyon
        RaycastChecks();

        // Hareket fonksiyonu
        MoveObject();

        // Dönme fonksiyonu
        RotateSaw();
    }

    // Sað ve sol raycast'leri kontrol eden fonksiyon
    void RaycastChecks()
    {
        // Sað raycast
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * raycastOffset, Vector2.right, raycastLength);
        // Sol raycast
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * raycastOffset, Vector2.left, raycastLength);

        // Eðer saðdaki raycast herhangi bir objeye çarparsa, sola hareket et
        if (hitRight.collider != null && hitRight.collider.gameObject.tag != "Player")
        {
            direction = -1;
        }

        // Eðer soldaki raycast herhangi bir objeye çarparsa, saða hareket et
        if (hitLeft.collider != null && hitLeft.collider.gameObject.tag != "Player")
        {
            direction = 1;
        }
    }

    // Obje hareketini saðlayan fonksiyon
    void MoveObject()
    {
        // Objemizi dünya uzayýnda saða veya sola doðru hareket ettir
        rb.velocity=Vector3.right * direction * moveSpeed +rb.velocity.y*Vector3.up;
    }

    // Testereyi döndüren fonksiyon
    void RotateSaw()
    {
        // Testereyi Z ekseninde döndür
        transform.Rotate(0, 0, -rotationSpeed * direction * Time.deltaTime);
    }

    // Raycast'i sahnede görselleþtirmek için
    private void OnDrawGizmos()
    {
        // Sað ve sol raycast çizgileri
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right * raycastOffset, transform.position + Vector3.right * (raycastOffset + raycastLength));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.left * raycastOffset, transform.position + Vector3.left * (raycastOffset + raycastLength));
    }
}
