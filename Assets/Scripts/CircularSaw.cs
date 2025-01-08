using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    // Hareket h�z�n� belirleyelim
    public float moveSpeed = 5f;

    // D�nme h�z�
    public float rotationSpeed = 360f; // saniyede ka� derece d�necek

    // Raycast'in uzunlu�u
    public float raycastLength = 0.5f;

    // Sa� ve sol i�in offset (mesafe)
    public float raycastOffset = 0.5f;

    // Hangi y�ne do�ru hareket edece�imizi belirleyelim: 1 sa�a, -1 sola
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
        // Raycast'leri y�netecek fonksiyon
        RaycastChecks();

        // Hareket fonksiyonu
        MoveObject();

        // D�nme fonksiyonu
        RotateSaw();
    }

    // Sa� ve sol raycast'leri kontrol eden fonksiyon
    void RaycastChecks()
    {
        // Sa� raycast
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * raycastOffset, Vector2.right, raycastLength);
        // Sol raycast
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * raycastOffset, Vector2.left, raycastLength);

        // E�er sa�daki raycast herhangi bir objeye �arparsa, sola hareket et
        if (hitRight.collider != null && hitRight.collider.gameObject.tag != "Player")
        {
            direction = -1;
        }

        // E�er soldaki raycast herhangi bir objeye �arparsa, sa�a hareket et
        if (hitLeft.collider != null && hitLeft.collider.gameObject.tag != "Player")
        {
            direction = 1;
        }
    }

    // Obje hareketini sa�layan fonksiyon
    void MoveObject()
    {
        // Objemizi d�nya uzay�nda sa�a veya sola do�ru hareket ettir
        rb.velocity=Vector3.right * direction * moveSpeed +rb.velocity.y*Vector3.up;
    }

    // Testereyi d�nd�ren fonksiyon
    void RotateSaw()
    {
        // Testereyi Z ekseninde d�nd�r
        transform.Rotate(0, 0, -rotationSpeed * direction * Time.deltaTime);
    }

    // Raycast'i sahnede g�rselle�tirmek i�in
    private void OnDrawGizmos()
    {
        // Sa� ve sol raycast �izgileri
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right * raycastOffset, transform.position + Vector3.right * (raycastOffset + raycastLength));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.left * raycastOffset, transform.position + Vector3.left * (raycastOffset + raycastLength));
    }
}
