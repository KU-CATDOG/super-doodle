using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedLevel : MonoBehaviour
{
    static public int index;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
