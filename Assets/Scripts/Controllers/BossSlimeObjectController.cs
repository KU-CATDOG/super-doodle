using System.Collections;
using Tool;
using UnityEngine;

namespace Controllers
{
    public class BossSlimeObjectController : EarthObjectController
    {
        private int hitCount;

        protected override float InvincibleSecond => 3;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoader.LoadPrefabAsync<GameObject>("EarthObjects/Slime", x =>
            {
                resource = Object.Instantiate(x);
            });
        }

        protected override void OnAttached()
        {
            hitCount = 0;
            Holder.MoveSpeed = Mathf.PI / 10;
        }

        protected override void OnDetached()
        {
        }

        protected override void UnloadResources()
        {
        }

        protected override void OnMeleeHit(EarthObject hitter)
        {
            // 한대 맞으면 버티고 더 빨라짐
            if (hitCount == 0)
            {
                Holder.MoveSpeed = Mathf.PI / 7.5f;
                resource.transform.localScale *= 1.3f;
                hitCount++;
            }
            else
            {
                Object.Destroy(Holder);
            }
        }
    }
}