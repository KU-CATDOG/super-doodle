using System.Collections;
using Tool;
using UnityEngine;

public class MobObjectController : EnemyController
{
    protected override float InvincibleSecond => 0;

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

    protected override void OnHit()
    {
        // 한대 맞으면 바로 죽음
        Holder.Earth.KillEarthObject(Holder);
    }
}
