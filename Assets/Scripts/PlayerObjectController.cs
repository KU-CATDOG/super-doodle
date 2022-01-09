using System.Collections.Generic;
using System.Collections;
using Tool;
using UnityEngine;

public class PlayerObjectController : EarthObjectController
{
    private readonly List<IEnemyController> toHitCache = new List<IEnemyController>();

    protected override IEnumerator LoadResources()
    {
        yield return AssetLoader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Player.prefab", x =>
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

    public override void OnUpdate()
    {
        foreach (var i in Holder.Earth.EarthObjects)
        {
            if (!(i.Controller is IEnemyController enemy)) continue;

            var distanceRadian = Mod(i.Radian, Mathf.PI * 2) - Mod(Holder.Radian, Mathf.PI * 2);

            if (Mathf.Abs(distanceRadian) > 0.1f) continue;

            toHitCache.Add(enemy);
        }

        foreach (var i in toHitCache)
        {
            i.OnHit();
        }

        toHitCache.Clear();
    }

    private float Mod(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r + m : r;
    }
}
