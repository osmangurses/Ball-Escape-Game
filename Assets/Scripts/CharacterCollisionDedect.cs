using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDedect : MonoBehaviour
{
    Rigidbody2D rb;
    CharacterLockManager lockManager;
    CharacterController characterController;
    float current_time = 0;
    float last_tp_time;
    private void Update()
    {
        current_time += Time.deltaTime;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lockManager = GetComponent<CharacterLockManager>();
        characterController = GetComponent<CharacterController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LockedBlock") && lockManager.key_count>0)
        {
            AudioPlayer.instance.PlayAudio(AudioName.unlock_block);
            lockManager.DecraseKey();
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<SpriteRenderer>().DOFade(0,0.2f);
            Destroy(collision.gameObject, 0.2f);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioPlayer.instance.StopAudio(AudioName.music);
            AudioPlayer.instance.PlayAudio(AudioName.explosion);
            rb.velocity = Vector3.zero;
            rb.gravityScale= 0;
            characterController.controls_locked = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Play();
            LevelDatas.instance.Invoke(nameof(LevelDatas.instance.RestartLevel),1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioPlayer.instance.StopAudio(AudioName.music);
            AudioPlayer.instance.PlayAudio(AudioName.explosion);
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            characterController.controls_locked = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Play();
            LevelDatas.instance.Invoke(nameof(LevelDatas.instance.RestartLevel), 1);
        }
        if (collision.gameObject.CompareTag("Key"))
        {
            AudioPlayer.instance.PlayAudio(AudioName.key_collect);
            lockManager.IncraseKey();
            collision.gameObject.GetComponent<BoxCollider2D>().enabled=false;
            collision.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0.2f);
            Destroy(collision.gameObject, 0.2f);

        }
        if (collision.CompareTag("EndHole"))
        {
            AudioPlayer.instance.StopAudio(AudioName.music);
            AudioPlayer.instance.PlayAudio(AudioName.transition);
            LevelDatas.instance.Invoke(nameof(LevelDatas.instance.next_level), 1f);
            LevelActionManager.OnLevelEnded(0);
            transform.DOScale(Vector3.zero,0.5f);
            transform.DOMove(collision.transform.position,0.2f);
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            GetComponentInChildren<TrailRenderer>().enabled = false;

            
        }
        if (collision.CompareTag("Teleporter") && current_time>last_tp_time+0.8)
        {
            AudioPlayer.instance.PlayAudio(AudioName.transition);
            last_tp_time = current_time;
            float tempgravity=rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            characterController.controls_locked = true;
            GetComponentInChildren<TrailRenderer>().emitting = false;
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                GetComponentInChildren<TrailRenderer>().emitting = true;
                transform.position = collision.gameObject.GetComponent<Teleporter>().other_point.transform.position;
                AudioPlayer.instance.PlayAudio(AudioName.transition);
                transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {
                    characterController.controls_locked = false;
                });
                rb.gravityScale = tempgravity;
                

            });
            transform.DOMove(collision.transform.position,0.2f);
            

            
        }
    }
}
