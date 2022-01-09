using System;
using UnityEngine;
using Tool;

namespace Test
{
    public class EarthTester : MonoBehaviour
    {
        private void Awake()
        {
            AssetLoader.LoadAsset<GameObject>("Prefabs/Earth.prefab", x =>
            {
                var earth = Instantiate(x);
                earth.transform.parent = transform;
                earth.transform.localPosition = Vector3.zero;

                var playerGo = new GameObject();
                playerGo.transform.parent = earth.transform;
                var player = playerGo.AddComponent<EarthObject>();
                player.Controller = new PlayerObjectController();
            });
        }
    }
}
