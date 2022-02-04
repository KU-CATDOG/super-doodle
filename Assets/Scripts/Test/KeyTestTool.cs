using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyTestTool : MonoBehaviour
{
    [SerializeField]
    private Text pressedKeyText;

    void Update()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                pressedKeyText.text = kcode.ToString();
        }
    }
}
