using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;
    private List<AudioSource> audioSources = new List<AudioSource>();
    public List<AudioData> audios; // Audios yerine AudioData kullanýlacak

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("IsSoundOn"))
        {
            PlayerPrefs.SetInt("IsSoundOn", 1);
        }
        // Singleton yapýsý oluþturmak için, baþka bir instance varsa onu yok et
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Bu nesneyi sahneler arasýnda koru
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Baþka bir instance varsa, bu instance'ý yok et
        }

    }
    public void SetVolume(bool isOn)
    {
        if (isOn) { PlayerPrefs.SetInt("IsSoundOn", 1); }
        else { PlayerPrefs.SetInt("IsSoundOn", 0); }
    }

    public void PlayAudio(AudioName audioName)
    {
        AudioSource audioSource = CheckAudioSources();
        foreach (AudioData audioData in audios) // Audios yerine AudioData kullanýlacak
        {
            if (audioData.audioName == audioName)
            {
                audioSource.clip = audioData.clip;
                audioSource.volume = audioData.volume * PlayerPrefs.GetInt("IsSoundOn");
                audioSource.pitch = Random.Range(1 - (audioData.pitchRandomize / 5), 1 + (audioData.pitchRandomize / 5));
                audioSource.loop = audioData.loop; // Loop özelliðini ayarla
                audioSource.Play();
                return; // Ses bulundu ve çalýndý, fonksiyondan çýk
            }
        }
        Debug.LogWarning("Audio not found: " + audioName);
    }

    public void StopAudio(AudioName audioName)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying && source.clip != null && GetAudioNameByClip(source.clip) == audioName.ToString())
            {
                source.Stop(); // Oynayan sesi durdur
                return; // Ses bulundu ve durduruldu, fonksiyondan çýk
            }
        }
        Debug.LogWarning("Audio not playing or not found: " + audioName);
    }
    public void StopAllAudio() // Tüm sesleri durdur
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop(); // Çalan tüm sesleri durdur
            }
        }
        Debug.Log("All audio stopped.");
    }
    private string GetAudioNameByClip(AudioClip clip)
    {
        foreach (AudioData audioData in audios)
        {
            if (audioData.clip == clip)
            {
                return audioData.audioName.ToString();
            }
        }
        return null;
    }

    private AudioSource CheckAudioSources()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        GameObject tempSource = new GameObject("TempAudioSource");
        AudioSource newAudioSource = tempSource.AddComponent<AudioSource>();
        audioSources.Add(newAudioSource);
        return newAudioSource;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearAudioSources();
    }

    private void ClearAudioSources()
    {
        // audioSources listesini temizle
        audioSources.Clear();
    }
}

[System.Serializable]
public class AudioData
{
    [Range(0, 1)]
    public float volume = 1;

    [Range(0, 1)]
    public float pitchRandomize = 0;

    public AudioClip clip;
    public AudioName audioName;
    public bool loop; // Loop özelliði eklendi
}

public enum AudioName
{
    circular_saw,
    explosion,
    grounded,
    jump,
    key_collect,
    lazer,
    lazer_button,
    move,
    transition,
    unlock_block,
    music
}
