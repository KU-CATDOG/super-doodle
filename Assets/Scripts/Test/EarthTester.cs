using Controllers;
using Tool;
using UnityEngine;

namespace Test
{
    public class EarthTester : MonoBehaviour
    {
        private Earth earth;

        private float lastTimeAdded;

        private void Awake()
        {
            var loadedPrefab = AssetLoader.LoadPrefab<GameObject>("Earth");

            earth = Instantiate(loadedPrefab).GetComponent<Earth>();
            var t = earth.transform;

            t.parent = transform;
            t.localPosition = Vector3.zero;

            var playerGo = new GameObject
            {
                transform =
                {
                    parent = t,
                },
            };

            var player = playerGo.AddComponent<EarthObject>();
            player.Controller = new PlayerObjectController();

            earth.AddEarthObject(player);
        }

        private void Update()
        {
            var now = Time.time;

            if (now - lastTimeAdded <= 1f) return;

            if (earth.EarthObjects.Count > 1) return;

            var mobGo = new GameObject
            {
                transform =
                {
                    parent = earth.transform,
                },
            };

            var mob = mobGo.AddComponent<EarthObject>();
            mob.Controller = new BossSlimeObjectController();

            mob.Radian = Mathf.PI * 1.5f;

            earth.AddEarthObject(mob);

            lastTimeAdded = now;
        }
    }
}
