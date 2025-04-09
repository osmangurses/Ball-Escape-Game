using UnityEngine;
public class ResetButtonHandler : MonoBehaviour
{
    private LevelDatas levelDatas;

    void Start()
    {
        levelDatas = FindObjectOfType<LevelDatas>();
    }

    public void OnResetButtonClick()
    {
        if (levelDatas != null)
        {
            levelDatas.ResetGame();
        }
    }
}
