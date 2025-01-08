using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public static class LevelActionManager
{
    public static event Action<int> LevelEnded;
    public static void OnLevelEnded(int levelIndex)
    {
        LevelEnded?.Invoke(levelIndex);
    }
}
