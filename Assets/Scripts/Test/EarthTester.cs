using Controllers;
using Tool;
using UnityEngine;

namespace Test
{
    public enum Boss
    {
        BossSlime,
        BossHades,
        BossJungle,
    }

    public class EarthTester : MonoBehaviour
    {
        public Boss currentBoss;

        private Earth earth;

        private float lastTimeAdded;

        private GameObject speedBar;

        private void Awake()
        {
            GameManager.Inst.gameState = GameManager.GameState.InGame;
            GameManager.Inst.currentBoss = currentBoss;

            var loadedPrefab = AssetLoaderManager.Inst.LoadPrefab<GameObject>("Earth");

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

            speedBar = GameObject.Find("SpeedBar");
            if (speedBar != null) speedBar.GetComponent<SpeedBar>().Initialize();

        }

        private void Update()
        {
            var now = Time.time;

            if (now - lastTimeAdded <= 1f) return;

            if (earth.ObjectCount > 1) return;

            if (GameManager.Inst.gameState != GameManager.GameState.InGame) return;

            var mobGo = new GameObject
            {
                transform =
                {
                    parent = earth.transform,
                },
            };

            var mob = mobGo.AddComponent<EarthObject>();
            mob.Controller = SelectBoss(currentBoss);
            mob.Radian = Mathf.PI * 1.5f;

            lastTimeAdded = now;
        }

        private EarthObjectController SelectBoss(Boss bossSelected) => bossSelected switch
        {
            Boss.BossSlime => new BossSlimeObjectController(),
            Boss.BossHades => new BossHadesObjectController(),
            Boss.BossJungle => new BossJungleObjectController(),
            _ => new BossSlimeObjectController(),
        };
    }
}
