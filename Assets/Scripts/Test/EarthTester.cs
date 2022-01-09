using System;
using UnityEngine;
using Tool;

namespace Test
{
    public class EarthTester : MonoBehaviour
    {
        private void Awake()
        {
            var loadedAsset = AssetLoader.LoadAsset<GameObject>("Prefabs/Earth.prefab");

            var earth = Instantiate(loadedAsset.Resource);
            earth.transform.parent = transform;
            earth.transform.localPosition = Vector3.zero;

            var playerGo = new GameObject();
            playerGo.transform.parent = earth.transform;
            var player = playerGo.AddComponent<EarthObject>();
            player.Controller = new PlayerObjectController();
        }
    }
}
