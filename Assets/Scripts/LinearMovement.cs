using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 pointA;           
    public Vector3 pointB;           
    public float speed = 2.0f;      
    public bool startAtPointA = true;

    [Header("Looping Settings")]
    public bool smoothTransition = true;

    private float journeyLength;     
    private float startTime;         
    private Vector3 startPoint;       
    private Vector3 endPoint;         
    private bool movingToB = true;    

    void Start()
    {
        if (startAtPointA)
        {
            transform.position = pointA;
            startPoint = pointA;
            endPoint = pointB;
        }
        else
        {
            transform.position = pointB;
            startPoint = pointB;
            endPoint = pointA;
            movingToB = false;
        }

        journeyLength = Vector3.Distance(startPoint, endPoint);
        startTime = Time.time;
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        if (smoothTransition)
        {
            fractionOfJourney = Mathf.SmoothStep(0, 1, fractionOfJourney);
        }

        transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);

        if (fractionOfJourney >= 1.0f)
        {
            movingToB = !movingToB;

            if (movingToB)
            {
                startPoint = pointA;
                endPoint = pointB;
            }
            else
            {
                startPoint = pointB;
                endPoint = pointA;
            }

            startTime = Time.time;
            journeyLength = Vector3.Distance(startPoint, endPoint);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointA, 0.3f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pointB, 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointA, pointB);
    }
}