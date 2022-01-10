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
            var loadedAsset = AssetLoader.LoadAsset<GameObject>("Prefabs/Earth.prefab");

            earth = Instantiate(loadedAsset.Resource).GetComponent<Earth>();
            earth.transform.parent = transform;
            earth.transform.localPosition = Vector3.zero;

            var playerGo = new GameObject();
            playerGo.transform.parent = earth.transform;
            var player = playerGo.AddComponent<EarthObject>();
            player.Controller = new PlayerObjectController();

            earth.AddEarthObject(player);
        }

        private void Update()
        {
            var now = Time.time;

            if (now - lastTimeAdded <= 1f) return;

            if (earth.EarthObjects.Count > 10) return;

            var mobGo = new GameObject();
            mobGo.transform.parent = earth.transform;
            var mob = mobGo.AddComponent<EarthObject>();
            mob.Controller = new BossSlimeObjectController();

            mob.Radian = Mathf.PI * 1.5f;

            earth.AddEarthObject(mob);

            lastTimeAdded = now;
        }
    }
}
