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
            if (phase == 1 || phase == 2)
            {
                Holder.StartCoroutine(ChangePhase(phase));
            }
            else if (phase == 3)
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

        private IEnumerator ChangePhase(int currentPhase)
        {
            var jungleTr = Holder.GetComponentInChildren<SpriteController>().transform;

            float timer = 0;
            while (timer < 0.5f)
            {
                jungleTr.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0), timer * 2);

                timer += Time.deltaTime;
                yield return null;
            }
            switch (currentPhase)
            {
                case 1:
                    jungleTr.GetComponent<SpriteController>().SetSprite(1);
                    break;
                case 2:
                default:
                    jungleTr.localScale *= 1.2f;
                    break;
            }
            while (timer < 1f)
            {
                jungleTr.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 360, 0), timer * 2 - 1);

                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void OnEvent(IEvent e)
        {
            if (!(e is SingleKeyPressedEvent { PressedKey: KeyCode.Space })) return;
            if (Time.time - lastDodgeTime < 1f) return;

            lastDodgeTime = Time.time;
            Holder.StartCoroutine(BossJungleBanana.DodgeBanana(0.5f));
            Holder.StartCoroutine(BossJunglePineApple.DodgePineApple(0.5f));
            var player = Holder.Earth.Player;
            Holder.StartCoroutine(DodgePlayer(player));
            Debug.Log("Dodge");
        }

        private IEnumerator DodgePlayer(EarthObject player)
        {
            float timer = 0f;
            Transform playerModel = player.transform.GetChild(0);
            while (timer < 0.25f)
            {
                playerModel.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0), timer * 4);
                timer += Time.deltaTime;
                yield return null;
            }
            while (timer < 0.5f)
            {
                playerModel.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 359, 0), timer * 4 - 1);
                timer += Time.deltaTime;
                yield return null;
            }
            playerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
