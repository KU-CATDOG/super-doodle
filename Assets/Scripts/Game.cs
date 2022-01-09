using System;
using UnityEngine;
using Tool;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        PressKeyDetector.Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
