using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    bool isOn;
    [SerializeField] GameObject on_image, off_image;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("IsSoundOn",1)==1)
        {
            isOn = true;
            on_image.SetActive(true);
            off_image.SetActive(false);
        }
        else 
        {
            isOn = false;
            on_image.SetActive(false);
            off_image.SetActive(true);
        }
        GetComponent<Button>().onClick.AddListener(ChangeState);
    }
    void ChangeState()
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("IsSoundOn",0);
            isOn = false;
            on_image.SetActive(false);
            off_image.SetActive(true);
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                audio.volume = 0;
            }
        }
        else
        {
            PlayerPrefs.SetInt("IsSoundOn", 1);
            isOn = true;
            on_image.SetActive(true);
            off_image.SetActive(false);
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                audio.volume = 1;
            }
        }

    }
}
