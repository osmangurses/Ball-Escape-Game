using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButton : MonoBehaviour
{
    public GameObject referenceObject;
    public Sprite pressingSprite;
    public Sprite notPressingSprite;
    public float delayTime = 5f;

    private SpriteRenderer spriteRenderer;
    private HashSet<Collider2D> collidersInTrigger = new HashSet<Collider2D>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        referenceObject.SetActive(true);
    }

    void Update()
    {
        if (collidersInTrigger.Count > 0)
        {
            spriteRenderer.sprite = pressingSprite;
            CancelInvoke(nameof(OpenReferenceObjectAfterDelay));
            referenceObject.SetActive(false);
            
        }
        else
        {
            spriteRenderer.sprite = notPressingSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioPlayer.instance.StopAudio(AudioName.lazer);
        collidersInTrigger.Add(other);
        if (collidersInTrigger.Count == 1) 
        {
            AudioPlayer.instance.PlayAudio(AudioName.lazer_button);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collidersInTrigger.Remove(other);

        if (collidersInTrigger.Count == 0)
        {
            AudioPlayer.instance.PlayAudio(AudioName.lazer_button);
            Invoke(nameof(OpenReferenceObjectAfterDelay),delayTime);
        }
    }

    private void OpenReferenceObjectAfterDelay()
    {
        referenceObject.SetActive(true);
        AudioPlayer.instance.PlayAudio(AudioName.lazer);
    }
}
