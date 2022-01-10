using System.Collections;
using UnityEngine;
using Tool;

public class BossSlimeObjectController : EnemyController
{
    private int hitCount;

    protected override float InvincibleSecond => 3;

    protected override IEnumerator LoadResources()
    {
        yield return AssetLoader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Slime.prefab", x =>
        {
            resource = Object.Instantiate(x.Resource);
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

    protected override void OnHit()
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
            Holder.Earth.KillEarthObject(Holder);
        }
    }
}
