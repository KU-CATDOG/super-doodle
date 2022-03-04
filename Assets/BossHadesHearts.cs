using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHadesHearts : MonoBehaviour
{
    private Transform[] heartObjects;

    private void Start()
    {
        heartObjects = transform.GetComponentsInChildren<Transform>();
        // 0 1 2
    }

    public void SetHealth(int hp)
    {
        int i = 0;
        for (i = 0; i < hp; ++i)
        {
            heartObjects[i].gameObject.SetActive(true);
        }
        for (; i < heartObjects.Length; ++i)
        {
            heartObjects[i].gameObject.SetActive(false);
        }
    }
}
