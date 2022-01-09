using System.Collections;
using Tool;
using UnityEngine;
using TMPro;

public class MobObjectController : EarthObjectController, IEnemyController
{
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

    public void OnHit()
    {
        // 제대로 된 키에 맞으면 죽는다.
        Holder.Earth.KillEarthObject(Holder);
    }
}
