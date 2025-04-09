using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DelayedActivate : MonoBehaviour
{
    [SerializeField] private float activationDelay = 3f;

    private List<MonoBehaviour> disabledComponents = new List<MonoBehaviour>();
    private List<Collider2D> disabledColliders = new List<Collider2D>();
    private List<Renderer> disabledRenderers = new List<Renderer>();

    void Start()
    {
        DisableAllComponents();

        StartCoroutine(ActivateAfterDelay());
    }

    private void DisableAllComponents()
    {
        MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script != this && script.enabled)
            {
                disabledComponents.Add(script);
                script.enabled = false;
            }
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.enabled)
            {
                disabledColliders.Add(collider);
                collider.enabled = false;
            }
        }

        Renderer[] renderers = GetComponents<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer.enabled)
            {
                disabledRenderers.Add(renderer);
                renderer.enabled = false;
            }
        }
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        Vector3 tempScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(tempScale, 0.1f);
        foreach (MonoBehaviour script in disabledComponents)
        {
            script.enabled = true;
        }

        foreach (Collider2D collider in disabledColliders)
        {
            collider.enabled = true;
        }

        foreach (Renderer renderer in disabledRenderers)
        {
            renderer.enabled = true;
        }
    }
}