
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADManager : MonoBehaviour
{
    
    public static ADManager instance;
    private void Awake()
    {
        instance = this;
        PokiUnitySDK.Instance.init();
    }
    public void ShowMidGameAD()
    {
        Time.timeScale = 0;
        PokiUnitySDK.Instance.commercialBreakCallBack = MidGameADCompleted;
        PokiUnitySDK.Instance.commercialBreak();
    }
    void MidGameADCompleted()
    {

        Time.timeScale = 1;
    }
}
