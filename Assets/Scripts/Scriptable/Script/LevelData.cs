using UnityEngine;

[CreateAssetMenu(menuName = "New Level", fileName = "Level_")]
public class LevelData : ScriptableObject
{
    public GameObject level_objects;
    public int level_number;
}
