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

    [SerializeField] GameObject playerObject;

    Quaternion angle = Quaternion.Euler(0, 0, 0);
    float curAngle = 0f;

    public bool moving = false;

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

        if (Input.GetKeyDown(key) && !moving)
        {
            moving = true;
            // 플레이어 이동
            curAngle -= 90f;
            angle = Quaternion.Euler(0, 0, curAngle);
            StartCoroutine(RotatePlayer(angle));

            key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
            nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);
        }
        
            //if 버튼에 도착할 경우
            //초기 위치에서 90도, 180도, 270도
            //if (Input.GetKeyDown(KeyCode.Space)) -> 해당 씬으로 이동
  
    }

    IEnumerator RotatePlayer(Quaternion angle)
    {
        Quaternion tAngle;
        for(float t=0; t<= 0.55f; t+= Time.deltaTime)
        {
            tAngle = playerObject.transform.rotation;
            playerObject.transform.rotation = Quaternion.Slerp(tAngle, angle, 0.1f);
            yield return null;
        }
        moving = false;
    }
}
