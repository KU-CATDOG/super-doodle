using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tool;
using TMPro;

public class Menu : MonoBehaviour
{
    KeyCode key;
    private float rotateSpeed = 0.03f;

    [Header("GameObjects")]
    [SerializeField] private Transform Earth;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshPro nextKey;

    [SerializeField] private TextMeshPro space;
    [SerializeField] GameObject playerObject;

    Quaternion angle = Quaternion.Euler(0, 0, 0);
    float curAngle = 0f;
    Vector2 start;

    void Start()
    {
        var gen = new RandomKeyGenerator();
        key = gen.GetKeyCode();
        // key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
        nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);

        start = player.position;
    }

    void Update()
    {
        Vector2 end = player.position;

        player.RotateAround(Earth.position, Vector3.back, rotateSpeed);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("InitialScene");
        }

        if (Input.GetKeyDown(key))
        {
            // 플레이어 이동
            rotateSpeed += 0.01f;

            /*curAngle -= 90f;
            angle = Quaternion.Euler(0, 0, curAngle);
            StartCoroutine(RotatePlayer(angle));*/

            key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
            nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);
        }

        curAngle = GetAngle(start, end);

        //if 버튼에 도착할 경우
        if (curAngle % 180 >= 42.5 || curAngle % 180 <= 47.5)
        {
            space.text = "Space";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // SceneManager.LoadScene("Setting");
            }
        }
        else if (curAngle % 180 >= 87.5 || curAngle % 180 <= 92.5)
        {
            space.text = "Space";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("MapSelect");
            }
        }
        else if (curAngle % 180 >= 132.5 || curAngle % 180 <= 137.5)
        {
            space.text = "Space";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // SceneManager.LoadScene("Credit");
            }
        }
            
    }

    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return 0 - (Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg);
    }

    /*IEnumerator RotatePlayer(Quaternion angle)
    {
        Quaternion tAngle;
        for(float t=0; t<= 0.55f; t+= Time.deltaTime)
        {
            tAngle = playerObject.transform.rotation;
            playerObject.transform.rotation = Quaternion.Slerp(tAngle, angle, 0.1f);
            yield return null;
        }
        moving = false;
    }*/
}
