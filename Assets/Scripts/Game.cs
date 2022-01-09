using System;
using UnityEngine;
using Tool;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    /// <summary>
    /// MonoBehaviour가 아닌 놈들이 Update에 불렸으면 하는 함수가 있으면 여기 등록
    /// </summary>
    public event Action OnUpdated;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OnUpdated?.Invoke();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
