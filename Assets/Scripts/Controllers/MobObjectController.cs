using System.Collections;
using Tool;
using UnityEngine;

public class MobObjectController : EarthObjectController
{
    protected override float InvincibleSecond => 0;

    public override ObjectSide Side => ObjectSide.Enemy;

    protected override bool AttackEnabled => false;

    protected override IEnumerator LoadResources()
    {
        yield return AssetLoader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Mob.prefab", x =>
        {
            resource = Object.Instantiate(x.Resource);
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
        Holder.Earth.KillEarthObject(Holder);
    }
}
