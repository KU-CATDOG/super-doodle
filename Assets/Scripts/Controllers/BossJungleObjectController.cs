using System.Collections;
using Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class BossJungleObjectController : EarthObjectController
    {
        private int phase;

        private float startTime;

        private BossJungleBanana bananaPrefab;
        private BossJunglePineApple pineApplePrefab;
        private BossJunglePeach peachPrefab;

        protected override float InvincibleSecond => 3;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        private float lastDodgeTime = 0;

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

            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("Misc/JunglePineApple", x =>
            {
                pineApplePrefab = x.GetComponent<BossJunglePineApple>();
            });

            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("Misc/JunglePeach", x =>
            {
                peachPrefab = x.GetComponent<BossJunglePeach>();
            });
        }

        protected override void OnAttached()
        {
            Holder.MoveSpeed = Mathf.PI / 10;

            MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnEvent);

            phase = 0;
            startTime = Time.time;
            recentBananaTime = 0f;

            Holder.StartCoroutine(Oscillate());
        }

        private IEnumerator Oscillate()
        {
            yield return new WaitForSeconds(1);

            Transform obj = Holder.transform.GetChild(0);
            while (Holder.gameObject)
            {
                yield return new WaitForFixedUpdate();
                //Debug.Log(obj.position);

                float x = Time.time * 3 % 3;
                float y = x < 2 ? 0.5f * x : -1 * x + 3;

                obj.localPosition = new Vector3(obj.localPosition.x, y + 3, obj.localPosition.z);

            }
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
                GameManager.Inst.gameState = GameManager.GameState.EndGame;
                GameManager.Inst.isRecentGameWin = true;
                UnloadResources();
                Object.Destroy(Holder.gameObject);
                SceneManager.LoadScene("ResultScene");
                return;
            }
        }

        private void OnEvent(IEvent e)
        {
            if (!(e is SingleKeyPressedEvent { PressedKey: KeyCode.Space })) return;
            if (Time.time - lastDodgeTime < 1f) return;

            lastDodgeTime = Time.time;
            Holder.StartCoroutine(BossJungleBanana.DodgeBanana(0.5f));
            Holder.StartCoroutine(BossJunglePineApple.DodgePineApple(0.5f));
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
            if (phase == 1)
            {
                if (now - recentPineAppleTime < 5f) return;

                var newPineApple = Object.Instantiate(pineApplePrefab);
                newPineApple.Init(Holder.Earth.Player);
                newPineApple.transform.localPosition = Holder.Controller.ResourceWorldPos;

                recentPineAppleTime = now;
            }
            if (phase == 2)
            {
                if (now - recentPeachTime < 3f) return;

                var newPeach = Object.Instantiate(peachPrefab);
                newPeach.Init(Holder);
                newPeach.transform.localPosition = Holder.Controller.ResourceWorldPos;

                recentPeachTime = now;
            }
        }

        private float recentBananaTime;
        private float recentPineAppleTime;
        private float recentPeachTime;
    }
}
