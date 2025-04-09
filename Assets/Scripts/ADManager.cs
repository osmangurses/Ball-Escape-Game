
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADManager : MonoBehaviour
{
    
    public static ADManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void ShowMidGameAD()
    {
    }
    void MidGameADCompleted()
    {
    }
}
