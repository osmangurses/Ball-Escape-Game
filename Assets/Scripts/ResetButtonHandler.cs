using UnityEngine;
public class ResetButtonHandler : MonoBehaviour
{
    private LevelDatas levelDatas;

    void Start()
    {
        // Sahnedeki LevelDatas objesini bulma
        levelDatas = FindObjectOfType<LevelDatas>();
    }

    public void OnResetButtonClick()
    {
        if (levelDatas != null)
        {
            levelDatas.ResetGame();  // LevelDatas i�indeki ResetGame fonksiyonunu �a��r�r
        }
    }
}
