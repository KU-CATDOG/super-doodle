using System.Collections;
using Tool;
using UnityEngine;

namespace Controllers
{
    public class BossJungleController : EarthObjectController
    {
        private int phase;

        private float startTime;

        private BossJungleBanana bananaPrefab;

        protected override float InvincibleSecond => 3;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("EarthObjects/Jungle", x =>
            {
                resource = Object.Instantiate(x);
            });

            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("Misc/JungleBanana", x =>
            {
                bananaPrefab = x.GetComponent<BossJungleBanana>();
            });
        }

        protected override void OnAttached()
        {
            Holder.MoveSpeed = Mathf.PI / 10;

            MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnEvent);

            phase = 0;
            startTime = Time.time;
            recentBananaTime = 0f;
        }

        protected override void OnDetached()
        {
        }

        protected override void UnloadResources()
        {
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            phase++;

            if (phase == 3)
            {
                // 승리
            }
        }

        private void OnEvent(IEvent e)
        {
            if (!(e is SingleKeyPressedEvent { PressedKey: KeyCode.Space })) return;

            Holder.StartCoroutine(BossJungleBanana.DodgeBanana());
        }

        public override void OnUpdate()
        {
            var now = Time.time - startTime;

            if (phase == 0)
            {
                if (now - recentBananaTime < 5f) return;

                var newBanana = Object.Instantiate(bananaPrefab);
                newBanana.Init(Holder.Earth.Player);
                newBanana.transform.localPosition = Holder.Controller.ResourceWorldPos;

                recentBananaTime = now;
            }
        }

        private float recentBananaTime;
    }
}
