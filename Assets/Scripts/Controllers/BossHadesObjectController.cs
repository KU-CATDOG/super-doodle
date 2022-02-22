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

        protected override float InvincibleSecond => 3;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

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

            phase = 0;
            startTime = Time.time;
            recentMeteorTime = 0;
            hitCount = 0;
        }

        protected override void OnDetached()
        {
            MessageSystem.Instance.Unsubscribe<SingleKeyPressedEvent>(OnEvent);
        }

        protected override void UnloadResources()
        {
            // 운석도 싸그리 파괴해 준다.
            BossHadesMeteor.DestroyAll();
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            // 승리했으므로 오브젝트 모두 파괴하고 게임 결과창 씬으로 이동시키기
            hitCount++;
            if (hitCount == 2)
            {
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

            // 스페이스가 눌릴 때마다 플레이어가 움직이는 방향을 바꿈
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
            newMeteor.Init(Holder.Earth.Player, MeteorSpeed);

            var xRange = Holder.Earth.Radius + 1;
            var x = xRange * (2 * Random.value - 1);
            newMeteor.transform.localPosition = new Vector3(x, 10, -1);

            recentMeteorTime = now;
        }

        private float recentMeteorTime;

        private float MeteorCooltime => phase switch
        {
            0 => 1f,
            1 => 0.5f,
            _ => 0.25f,
        };

        private float MeteorSpeed => phase switch
        {
            0 => 4,
            1 => 8,
            _ => 12,
        };
    }
}
