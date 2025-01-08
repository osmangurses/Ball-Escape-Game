using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachine : MonoBehaviour
{
    public SpriteRenderer lazerSprite; // Lazerin sprite'ýný gösterecek SpriteRenderer
    public float raycastDistance = 10f;
    public Vector2 raycastOffset = new Vector2(0f, 0.33f); // Raycast için offset ayarý

    // Raycast hit noktasý
    private RaycastHit2D hit;
    private void Start()
    {
        AudioPlayer.instance.PlayAudio(AudioName.lazer);
    }
    void Update()
    {
        Vector2 direction = transform.up; // Objeye göre yukarý yön

        // Yukarýya doðru raycast at, lazer katmaný dýþýndaki tüm katmanlara çarpar
        int layerMask = ~(1 << LayerMask.NameToLayer("Laser")); // "Laser" katmaný dýþýndaki katmanlar

        // Raycast baþlangýç noktasýný makinenin dönüþüyle birlikte hesapla
        Vector2 raycastOrigin = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)raycastOffset);

        hit = Physics2D.Raycast(raycastOrigin, direction, raycastDistance, layerMask);

        if (hit.collider != null)
        {
            // Çarptýðý objeye kadar lazer çiz
            DrawLazer(transform.position, hit.point);
        }
        else
        {
            // Çarpmazsa maksimum mesafeye kadar lazer çiz
            DrawLazer(transform.position, raycastOrigin + direction * raycastDistance);
        }
    }

    // Lazer sprite'ýný çizmek için fonksiyon
    void DrawLazer(Vector2 start, Vector2 end)
    {
        Vector2 difference = end - start;
        float distance = difference.magnitude;

        // Lazer sprite'ýnýn ortasýný baþlangýç ile bitiþin ortasýna yerleþtir
        lazerSprite.transform.position = (start + end) / 2;

        // Lazer sprite'ýnýn yönünü ayarlamak için "up" vektörünü fark vektörüne hizala
        lazerSprite.transform.up = difference.normalized;

        // Lazer sprite'ýný scale kullanarak uzat
        lazerSprite.transform.localScale = new Vector3(lazerSprite.transform.localScale.x, distance, lazerSprite.transform.localScale.z);

    }

    // Raycast'i Scene'de görmek için Gizmos ile çizdir
    void OnDrawGizmos()
    {
        Vector2 direction = transform.up; // Objeye göre yukarý yön

        // Offset ile raycast baþlangýç noktasý
        Vector2 raycastOrigin = (Vector2)transform.position + (Vector2)(transform.rotation * (Vector3)raycastOffset);

        // Eðer Update fonksiyonunda raycast hit bilgisi varsa, onu çizdir
        if (hit.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(raycastOrigin, hit.point); // Temas noktasýna kadar çi
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(raycastOrigin, raycastOrigin + direction * raycastDistance); // Maksimum mesafeye kadar çiz
        }
    }
}
