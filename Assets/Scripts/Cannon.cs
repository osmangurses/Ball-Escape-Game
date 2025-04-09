using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Cannon : MonoBehaviour
{
    public Transform collectedTransform;
    public Rigidbody2D collectedRB;
    public List<string> collectableTags;
    public float rotationSpeed = 50f;
    public float fireForce = 10f;
    public float maxRotation = 90f;
    public float minRotation = -90f;
    public Transform firePoint;
    public Transform rotatePoint;
    public InputAction fireAction;
    public bool isCollected;
    public bool rotateClockwise = true;

    private void Awake()
    {
        fireAction.Enable();
    }

    private void OnEnable()
    {
        fireAction.started += ctx => FireCollected();
    }

    private void OnDisable()
    {
        fireAction.started -= ctx => FireCollected();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collectableTags.Contains(collision.gameObject.tag) &&
            collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb) &&
            !isCollected)
        {
            collectedRB = rb;
            collectedTransform = collision.transform;

            collectedTransform.DOScale(Vector3.zero, 0.3f).OnComplete(() => {
                collectedTransform.gameObject.SetActive(false);
            });

            isCollected = true;
        }
    }

    void FireCollected()
    {
        Debug.Log("Fire");
        if (isCollected)
        {
            collectedTransform.gameObject.SetActive(true);
            collectedTransform.position = firePoint.position;

            collectedTransform.DOComplete();
            collectedTransform.DOScale(Vector3.one, 0f).OnComplete(() => {
                if (collectedRB != null)
                {
                    collectedRB.isKinematic = false;
                    Vector2 fireDirection = rotatePoint.up;
                    Debug.DrawRay(firePoint.position, fireDirection * 2f, Color.red, 1f);
                    collectedRB.AddForce(fireDirection * fireForce, ForceMode2D.Impulse);

                    CharacterController playerController = collectedTransform.GetComponent<CharacterController>();
                    if (playerController != null)
                    {
                        playerController.LaunchByCannon();
                    }

                    collectedRB = null;
                    collectedTransform = null;
                    isCollected = false;
                }
            });
        }
    }

    private void Update()
    {
        RotateFirePoint();

        if (Input.GetKeyDown(KeyCode.M))
        {
            FireCollected();
        }
    }

    private void RotateFirePoint()
    {
        float currentRotation = rotatePoint.localRotation.eulerAngles.z;

        if (currentRotation > 180f)
            currentRotation -= 360f;

        if (currentRotation >= maxRotation)
            rotateClockwise = false;
        else if (currentRotation <= minRotation)
            rotateClockwise = true;

        float direction = rotateClockwise ? 1f : -1f;
        rotatePoint.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }
}