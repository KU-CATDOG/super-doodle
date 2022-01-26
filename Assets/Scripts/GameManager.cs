using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _inst;

    public float tempTimer = 0;

    public static GameManager Inst
    {
        get
        {
            if (_inst)
            {
                return _inst;
            }
            else
            {
                var managerGameObject = new GameObject();
                managerGameObject.name = "GameManager_Singleton";
                DontDestroyOnLoad(managerGameObject);
                _inst = managerGameObject.AddComponent<GameManager>();
                return _inst;
            }
        }
    }
}
