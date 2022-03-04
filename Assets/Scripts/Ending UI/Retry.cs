using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Tool;

public class Retry : MonoBehaviour
{
    KeyCode key;

    [SerializeField]
    TextMeshPro retryText;

    private void Start()
    {
        key = EarthKeyGenerator.KeyGenerator.GetKeyCode();
        retryText.text = "Press " + EarthKeyGenerator.KeyGenerator.KeyCodeToString(key) + " To Retry";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.ButtonPress);
            SceneManager.LoadScene("MapSelect");
        }
        else if (Input.GetKeyDown(key))
        {
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.ButtonPress);
            SceneManager.LoadScene(GameManager.Inst.currentBoss.ToString());
        }
    }
}
