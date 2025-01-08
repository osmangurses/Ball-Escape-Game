using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDatas : MonoBehaviour
{
    public static LevelDatas instance;
    public int current_level_index=0;
    public LevelData[] level_datas;
    public Button restart_button;
    GameObject instantiated_level_objects=null;
    float ad_timer;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("FHLevel"))
        {
            PlayerPrefs.SetInt("FHLevel", 0);
            PlayerPrefs.Save();
        }
        current_level_index = PlayerPrefs.GetInt("FHLevel");
        RestartLevel();
        restart_button.onClick.AddListener(RestartLevel);
    }
    private void Update()
    {
        ad_timer += Time.deltaTime;
    }
    public void RestartLevel()
    {
        AudioPlayer.instance.StopAllAudio();
        if (instantiated_level_objects != null)
        {
            Destroy(instantiated_level_objects);
        }
        instantiated_level_objects = Instantiate(level_datas[current_level_index].level_objects);
        if (ad_timer>180)
        {
            ADManager.instance.ShowMidGameAD();
            ad_timer = 0;
        }
        AudioPlayer.instance.PlayAudio(AudioName.music);
    }
    public void next_level()
    {
        AudioPlayer.instance.StopAllAudio();
        if (instantiated_level_objects != null)
        {
            Destroy(instantiated_level_objects);
        }
        current_level_index++;
        instantiated_level_objects = Instantiate(level_datas[current_level_index].level_objects);
        if (ad_timer > 180)
        {
            ADManager.instance.ShowMidGameAD();
            ad_timer = 0;
        }
        AudioPlayer.instance.PlayAudio(AudioName.music);
        PlayerPrefs.SetInt("FHLevel", current_level_index);
        PlayerPrefs.Save();
    }
    public void ResetGame()
    {
        current_level_index = 0;
        RestartLevel();

    }
}
