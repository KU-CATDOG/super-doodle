using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BossJungleController : EarthObjectController
    {
        protected override float InvincibleSecond => 3;

        public override ObjectSide Side => ObjectSide.Enemy;

        protected override bool AttackEnabled => true;

        protected override IEnumerator LoadResources()
        {
            yield return AssetLoaderManager.Inst.LoadPrefabAsync<GameObject>("EarthObjects/Jungle", x =>
            {
                resource = Object.Instantiate(x);
            });
        }

        protected override void OnAttached()
        {
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
        }
    }
}
