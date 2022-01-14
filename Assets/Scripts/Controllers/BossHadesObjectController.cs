using System.Collections;
using Tool;
using UnityEngine;

namespace Controllers
{
    public class BossHadesObjectController : EarthObjectController
    {
        protected override float InvincibleSecond => 3;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoader.LoadPrefabAsync<GameObject>("EarthObjects/Hades", x =>
            {
                resource = Object.Instantiate(x);
            });
        }

        protected override void OnAttached()
        {
            Holder.MoveSpeed = Mathf.PI / 10;

            MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnEvent);
        }

        protected override void OnDetached()
        {
            MessageSystem.Instance.Unsubscribe<SingleKeyPressedEvent>(OnEvent);
        }

        protected override void UnloadResources()
        {
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            Object.Destroy(Holder);
        }

        private void OnEvent(IEvent e)
        {
            if (!(e is SingleKeyPressedEvent { PressedKey: KeyCode.Space })) return;

            var player = Holder.Earth.Player;

            if (player == null) return;

            // 스페이스가 눌릴 때마다 플레이어가 움직이는 방향을 바꿈
            player.MoveSpeed = -player.MoveSpeed;
        }
    }
}
