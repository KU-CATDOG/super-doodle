using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MapSelect");
        }
        else if (Input.anyKeyDown)
            SceneManager.LoadScene("Tutorial");
    }
}
