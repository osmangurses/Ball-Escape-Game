using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLockManager : MonoBehaviour
{
    public int key_count = 0;
    [SerializeField] Image key_count_image;
    [SerializeField] Sprite[] number_sprites;

    public void IncraseKey()
    {
        key_count++;
        key_count_image.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => 
        {
            key_count_image.sprite=number_sprites[key_count];
            key_count_image.transform.DOScale(Vector3.one,0.2f);
        
        });
    
    }
    public void DecraseKey()
    {
        key_count--;

        key_count_image.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            key_count_image.sprite = number_sprites[key_count];
            key_count_image.transform.DOScale(Vector3.one, 0.2f);

        });

    }
}
