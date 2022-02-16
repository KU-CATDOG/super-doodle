using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tool;
using TMPro;

public class Menu : MonoBehaviour
{
    IEvent e;
    KeyCode key;
    private float rotateSpeed = 1f;

    [Header("GameObjects")]
    [SerializeField] private Transform Earth;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshPro nextKey;

    void Start()
    {
        var gen = new RandomKeyGenerator();
        key = gen.GetKeyCode();
        // key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
        nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("InitialScene");
        }

        if (Input.GetKeyDown(key))
        {
                // 플레이어 이동
                player.RotateAround(Earth.position, Vector3.back, 30f);
                key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
                nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);
        }
        
            //if 버튼에 도착할 경우
            //초기 위치에서 90도, 180도, 270도
            //if (Input.GetKeyDown(KeyCode.Space)) -> 해당 씬으로 이동
  
    }
}
