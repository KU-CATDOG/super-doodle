using System.Collections.Generic;
using System.Collections;
using Tool;
using UnityEngine;

public class PlayerObjectController : EarthObjectController
{
    private readonly List<EnemyController> toHitCache = new List<EnemyController>();

    protected override IEnumerator LoadResources()
    {
        yield return AssetLoader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Player.prefab", x =>
        {
            resource = Object.Instantiate(x.Resource);
        });
    }

    protected override void OnAttached()
    {
        Holder.MoveSpeed = 0;
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
            if (!(i.Controller is EnemyController enemy)) continue;

            // 히트박스의 범위
            var distanceRadian = Mod(i.Radian, Mathf.PI * 2) - Mod(Holder.Radian, Mathf.PI * 2);

            // 히트박스 안에 없으면 지워 줘야 함
            if (Mathf.Abs(distanceRadian) > 0.1f) continue;

            // 안에 있으면 때릴 놈 목록에 추가함
            toHitCache.Add(enemy);
        }

        var now = Time.time;

        foreach (var i in toHitCache)
        {
            i.AttackThis();
        }

        toHitCache.Clear();
    }

    private float Mod(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r + m : r;
    }

    public override void OnEarthKeyPressed(bool corrected)
    {
        if (corrected)
        {
            Holder.MoveSpeed = Mathf.Min(Holder.MoveSpeed + 0.2f, Mathf.PI / 2);
        }
        else
        {
            Holder.MoveSpeed = Mathf.Max(Holder.MoveSpeed - 0.2f, 0);
        }
    }
}
