using System.Collections;
using Tool;
using UnityEngine;

public class BossSlimeObjectController : EarthObjectController
{
    private int hitCount;

    protected override float InvincibleSecond => 3;

    public override ObjectSide Side => ObjectSide.Enemy;

    protected override bool AttackEnabled => true;

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
            Holder.Earth.KillEarthObject(Holder);
        }
    }
}
