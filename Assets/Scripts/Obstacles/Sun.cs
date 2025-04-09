using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public enum ShootMode
    {
        Normal,
        Burst,
        Area
    }

    [Header("References")]
    public Transform player;
    public GameObject ammoPrefab;
    public Transform firePoint;

    [Header("Shooting Settings")]
    public ShootMode shootMode = ShootMode.Normal;
    public float shootInterval = 2f;
    public float projectileSpeed = 5f;
    public float spawnOffset = 1.0f;

    [Header("Burst Mode Settings")]
    public int burstCount = 3;
    public float burstDelay = 0.2f;

    [Header("Area Mode Settings")]
    public int areaCount = 5;
    public float spreadAngle = 90f;

    private float lastShootTime;
    private bool isBursting = false;

    void Start()
    {
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

        if (firePoint == null)
        {
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.parent = transform;
            firePointObj.transform.localPosition = Vector3.zero;
            firePoint = firePointObj.transform;
        }

        lastShootTime = Time.time;
    }

    void Update()
    {
        if (ammoPrefab == null)
            return;

        if (isBursting)
            return;

        if (Time.time >= lastShootTime + shootInterval)
        {
            switch (shootMode)
            {
                case ShootMode.Normal:
                    ShootAtPlayer();
                    break;
                case ShootMode.Burst:
                    StartCoroutine(BurstShoot());
                    break;
                case ShootMode.Area:
                    AreaShoot();
                    break;
            }

            lastShootTime = Time.time;
        }
    }

    void ShootAtPlayer()
    {
        if (player == null)
            return;

        Vector2 direction = (player.position - transform.position).normalized;

        ShootAmmo(direction);
    }

    IEnumerator BurstShoot()
    {
        if (player == null)
        {
            isBursting = false;
            yield break;
        }

        isBursting = true;

        for (int i = 0; i < burstCount; i++)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            ShootAmmo(direction);

            yield return new WaitForSeconds(burstDelay);
        }

        isBursting = false;
    }

    void AreaShoot()
    {
        float startAngle = 270f - (spreadAngle / 2);

        float angleStep = spreadAngle / (areaCount - 1);

        for (int i = 0; i < areaCount; i++)
        {
            float angle = startAngle + (angleStep * i);

            float radian = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            ShootAmmo(direction);
        }
    }

    void ShootAmmo(Vector2 direction)
    {
        Vector2 spawnPosition = (Vector2)transform.position + (direction * spawnOffset);

        GameObject ammoObj = Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);

        SunAmmo ammoScript = ammoObj.GetComponent<SunAmmo>();
        if (ammoScript != null)
        {
            ammoScript.Initialize(direction, projectileSpeed);

            Physics2D.IgnoreCollision(ammoObj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            Debug.LogWarning("SunAmmo component'i bulunamadý!");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(firePoint.position, 0.2f);
        }

        if (shootMode == ShootMode.Area)
        {
            float startAngle = 270f - (spreadAngle / 2);
            float angleStep = spreadAngle / (areaCount - 1);

            for (int i = 0; i < areaCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                float radian = angle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, direction * 3f);
            }
        }
    }
}