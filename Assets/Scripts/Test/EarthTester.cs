using System;
using UnityEngine;
using Tool;

namespace Test
{
    public class EarthTester : MonoBehaviour
    {
        private void Awake()
        {
            using var loader = new AssetLoader();

            loader.LoadPrefab<GameObject>("Earth.prefab", x =>
            {
                var go = Instantiate(x);
                go.transform.parent = transform;
                go.transform.localPosition = Vector3.zero;
            });
        }
    }
}
