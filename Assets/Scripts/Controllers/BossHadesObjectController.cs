using System.Collections;
using Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class BossHadesObjectController : EarthObjectController
    {
        private int phase;

        private int hitCount;

        private float startTime;

        private BossHadesMeteor meteorPrefab;

        protected override float InvincibleSecond => 0.2f;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        private BossHadesHearts heartController;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("EarthObjects/Hades", x =>
            {
                resource = Object.Instantiate(x);
            });

            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("Misc/HadesMeteor", x =>
            {
                meteorPrefab = x.GetComponent<BossHadesMeteor>();
            });
        }

        protected override void OnAttached()
        {
            Holder.MoveSpeed = Mathf.PI / 10;

            MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnEvent);

            heartController = GameObject.FindGameObjectWithTag("Heart")?.GetComponent<BossHadesHearts>();

            phase = 0;
            startTime = Time.time;
            recentMeteorTime = 0;
            hitCount = 0;

            Holder.StartCoroutine(Fly());
        }

        private IEnumerator Fly()
        {
            yield return new WaitForSeconds(1f);

            Transform obj = Holder.transform.GetChild(0);

            obj.localPosition = new Vector3(obj.localPosition.x, 3.6f, obj.localPosition.z);

            while (Holder.gameObject)
            {
                yield return new WaitForFixedUpdate();

                obj.localPosition = new Vector3(obj.localPosition.x, Mathf.PingPong(Time.time, 0.2f) + 3.5f, obj.localPosition.z);
            }
        }

        protected override void OnDetached()
        {
            MessageSystem.Instance.Unsubscribe<SingleKeyPressedEvent>(OnEvent);
        }

        protected override void UnloadResources()
        {
            // ????????? ????????? ????????? ??????.
            BossHadesMeteor.DestroyAll();
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            // ?????????????????? ???????????? ?????? ???????????? ?????? ????????? ????????? ???????????????
            hitCount++;
            if (hitCount == 2)
            {
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

            var player = Holder.Earth.Player;

            if (player == null) return;

            // ??????????????? ?????? ????????? ??????????????? ???????????? ????????? ??????
            Holder.StartCoroutine(FlipPlayer(player.transform, player.MoveSpeed > 0));
            player.MoveSpeed = -player.MoveSpeed;
        }

        public override void OnUpdate()
        {
            var now = Time.time - startTime;

            if (phase == 0 && now >= 10f)
            {
                phase = 1;
                Holder.MoveSpeed = Mathf.PI / 7.5f;
            }

            if (phase == 1 && now >= 20f)
            {
                phase = 2;
            }

            if (now - recentMeteorTime < MeteorCooltime) return;

            var newMeteor = Object.Instantiate(meteorPrefab);
            newMeteor.Init(Holder.Earth.Player, MeteorSpeed, MeteorSize, heartController);

            var xRange = Holder.Earth.Radius + 1;
            var x = xRange * (2 * Random.value - 1);
            newMeteor.transform.localPosition = new Vector3(x, 10, -1);

            recentMeteorTime = now;
        }

        private float recentMeteorTime;

        private float MeteorCooltime => phase switch
        {
            0 => 2f,
            1 => 1f,
            _ => 0.5f,
        };

        private float MeteorSpeed => phase switch
        {
            0 => 2,
            1 => 4,
            _ => 6,
        };

        private float MeteorSize => phase switch
        {
            0 => 1f,
            _ => 0.5f
        };

        private IEnumerator FlipPlayer(Transform player, bool watchLeft)
        {
            float timer = 0;
            float duration = 0.2f;
            Transform childTransforms = player.GetChild(0);
            while (timer < duration)
            {
                childTransforms.localRotation = Quaternion.Euler
                    (
                    childTransforms.localRotation.x,
                    Mathf.Lerp(childTransforms.localRotation.y, watchLeft ? 180 : 360, timer / duration),
                    childTransforms.localRotation.z
                    );
                timer += Time.deltaTime;
                yield return null;
            }
            childTransforms.localRotation = Quaternion.Euler
                    (
                    childTransforms.localRotation.x,
                    watchLeft ? 180 : 0,
                    childTransforms.localRotation.z
                    );
        }
    }
}
