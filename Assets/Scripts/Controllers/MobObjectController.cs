using System.Collections;
using Tool;
using UnityEngine;

namespace Controllers
{
    public class MobObjectController : EarthObjectController
    {
        protected override float InvincibleSecond => 0;

        public override ObjectSide Side => ObjectSide.Enemy;

        // 잡몹은 공격이 불가능하다.
        protected override bool AttackEnabled => false;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoader.LoadPrefabAsync<GameObject>("EarthObjects/Mob", x =>
            {
                resource = Object.Instantiate(x);
            });
        }

        protected override void OnAttached()
        {
        }

        protected override void OnDetached()
        {
        }

        protected override void UnloadResources()
        {
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            // 한대 맞으면 바로 죽음
            Object.Destroy(Holder);
        }
    }
}
