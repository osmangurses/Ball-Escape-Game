using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachine : MonoBehaviour
{
    public SpriteRenderer lazerSprite; // Lazerin sprite'�n� g�sterecek SpriteRenderer
    public float raycastDistance = 10f;
    public Vector2 raycastOffset = new Vector2(0f, 0.33f); // Raycast i�in offset ayar�

    // Raycast hit noktas�
    private RaycastHit2D hit;
    private void Start()
    {
        AudioPlayer.instance.PlayAudio(AudioName.lazer);
    }
    void Update()
    {
        Vector2 direction = transform.up; // Objeye g�re yukar� y�n

        // Yukar�ya do�ru raycast at, lazer katman� d���ndaki t�m katmanlara �arpar
        int layerMask = ~(1 << LayerMask.NameToLayer("Laser")); // "Laser" katman� d���ndaki katmanlar

        // Raycast ba�lang�� noktas�n� makinenin d�n���yle birlikte hesapla
        Vector2 raycastOrigin = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)raycastOffset);

        hit = Physics2D.Raycast(raycastOrigin, direction, raycastDistance, layerMask);

        if (hit.collider != null)
        {
            // �arpt��� objeye kadar lazer �iz
            DrawLazer(transform.position, hit.point);
        }
        else
        {
            // �arpmazsa maksimum mesafeye kadar lazer �iz
            DrawLazer(transform.position, raycastOrigin + direction * raycastDistance);
        }
    }

    // Lazer sprite'�n� �izmek i�in fonksiyon
    void DrawLazer(Vector2 start, Vector2 end)
    {
        Vector2 difference = end - start;
        float distance = difference.magnitude;

        // Lazer sprite'�n�n ortas�n� ba�lang�� ile biti�in ortas�na yerle�tir
        lazerSprite.transform.position = (start + end) / 2;

        // Lazer sprite'�n�n y�n�n� ayarlamak i�in "up" vekt�r�n� fark vekt�r�ne hizala
        lazerSprite.transform.up = difference.normalized;

        // Lazer sprite'�n� scale kullanarak uzat
        lazerSprite.transform.localScale = new Vector3(lazerSprite.transform.localScale.x, distance, lazerSprite.transform.localScale.z);

    }

    // Raycast'i Scene'de g�rmek i�in Gizmos ile �izdir
    void OnDrawGizmos()
    {
        Vector2 direction = transform.up; // Objeye g�re yukar� y�n

        // Offset ile raycast ba�lang�� noktas�
        Vector2 raycastOrigin = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)raycastOffset);

        // E�er Update fonksiyonunda raycast hit bilgisi varsa, onu �izdir
        if (hit.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(raycastOrigin, hit.point); // Temas noktas�na kadar �i
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(raycastOrigin, raycastOrigin + direction * raycastDistance); // Maksimum mesafeye kadar �iz
        }
    }
}
