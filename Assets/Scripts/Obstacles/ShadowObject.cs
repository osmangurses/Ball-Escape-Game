using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShadowObject : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;

    [Header("Shadow Settings")]
    public float delay = 0.5f;
    public int positionHistorySize = 100;
    public float scaleAnimDuration = 0.5f;
    public Ease scaleEase = Ease.OutBack;

    private List<TransformData> positionHistory = new List<TransformData>();
    private bool isInitialized = false;

    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float timestamp;

        public TransformData(Vector3 pos, Quaternion rot, float time)
        {
            position = pos;
            rotation = rot;
            timestamp = time;
        }
    }

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is missing! Please assign a target object.");
            return;
        }

        transform.localScale = Vector3.zero;

        StartCoroutine(RecordPositionsForDelay());
    }

    IEnumerator RecordPositionsForDelay()
    {
        float startTime = Time.time;

        while (Time.time < startTime + delay)
        {
            positionHistory.Add(new TransformData(
                targetObject.position,
                targetObject.rotation,
                Time.time
            ));

            yield return null;
        }

        isInitialized = true;
        transform.DOScale(Vector3.one, scaleAnimDuration)
            .SetEase(scaleEase);
    }

    void Update()
    {
        positionHistory.Add(new TransformData(
            targetObject.position,
            targetObject.rotation,
            Time.time
        ));

        if (positionHistory.Count > positionHistorySize)
        {
            positionHistory.RemoveAt(0);
        }

        if (!isInitialized) return;

        ApplyDelayedMovement();
    }

    void ApplyDelayedMovement()
    {
        float targetTime = Time.time - delay;

        TransformData beforeData = null;
        TransformData afterData = null;

        for (int i = 0; i < positionHistory.Count - 1; i++)
        {
            if (positionHistory[i].timestamp <= targetTime && positionHistory[i + 1].timestamp >= targetTime)
            {
                beforeData = positionHistory[i];
                afterData = positionHistory[i + 1];
                break;
            }
        }

        if (beforeData != null && afterData != null)
        {
            float t = Mathf.InverseLerp(beforeData.timestamp, afterData.timestamp, targetTime);

            transform.position = Vector3.Lerp(beforeData.position, afterData.position, t);
            transform.rotation = Quaternion.Slerp(beforeData.rotation, afterData.rotation, t);
        }
        else if (positionHistory.Count > 0)
        {
            TransformData oldestData = positionHistory[0];
            transform.position = oldestData.position;
            transform.rotation = oldestData.rotation;
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}