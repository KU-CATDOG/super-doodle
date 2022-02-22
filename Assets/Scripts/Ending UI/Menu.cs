using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tool;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] GameObject settings;
    [SerializeField] Button save;

    bool inSetting = false;

    Quaternion angle = Quaternion.Euler(0, 0, 0);
    float curAngle = 0f;
<<<<<<< HEAD
    Vector2 start;
=======



    public bool moving = false;
>>>>>>> 955120fa5be81336b026e8bd71976269695a32fa

    void Start()
    {
        var gen = new RandomKeyGenerator();
        key = gen.GetKeyCode();
        // key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
        nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);

<<<<<<< HEAD
        start = player.position;
=======
        GameManager.Inst.gameState = GameManager.GameState.Menu;

>>>>>>> 955120fa5be81336b026e8bd71976269695a32fa
    }

    void Update()
    {
        Vector2 end = player.position;

        player.RotateAround(Earth.position, Vector3.back, rotateSpeed);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("InitialScene");
        }

<<<<<<< HEAD
        if (Input.GetKeyDown(key))
=======
        if (Input.GetKeyDown(key) && !moving && !inSetting)
>>>>>>> 955120fa5be81336b026e8bd71976269695a32fa
        {
            // 플레이어 이동
            rotateSpeed += 0.01f;

            /*curAngle -= 90f;
            angle = Quaternion.Euler(0, 0, curAngle);
            StartCoroutine(RotatePlayer(angle));*/

            key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
            nextKey.text = EarthKeyGenerator.KeyGenerator.KeyCodeToString(key);
        }

<<<<<<< HEAD
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
            
=======
        //if 버튼에 도착할 경우
        //초기 위치에서 90도, 180도, 270도
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.rotation == Quaternion.Euler(0, 0, -90))
            {
                settings.SetActive(true);
                inSetting = true;
            }
        }


>>>>>>> 955120fa5be81336b026e8bd71976269695a32fa
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
<<<<<<< HEAD
    }*/
=======
    }

    #region KeySettings
    public void ToggleAnKey(bool boolean)
    {
        GameManager.Inst.an = boolean;
    }
    public void ToggleFnKey(bool boolean)
    {
        GameManager.Inst.fn = boolean;
    }
    public void ToggleControlKey(bool boolean)
    {
        GameManager.Inst.cn = boolean;
    }
    public void ToggleKeypadKey(bool boolean)
    {
        GameManager.Inst.keypad = boolean;
    }
    #endregion

    public void CloseSettings()
    {
        settings.SetActive(false);
        inSetting = false;
    }
>>>>>>> 955120fa5be81336b026e8bd71976269695a32fa
}
