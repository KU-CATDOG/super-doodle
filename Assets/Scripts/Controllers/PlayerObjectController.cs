using System.Collections;
using System.Collections.Generic;
using Tool;
using UnityEngine;

public class PlayerObjectController : EarthObjectController
{
    private readonly List<EarthObject> toHitCache = new List<EarthObject>();

    public override ObjectSide Side => ObjectSide.Player;

    protected override bool AttackEnabled => true;

    protected override float InvincibleSecond => 3;

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

    protected override void OnMeleeHit(EarthObject hitter)
    {
        Debug.Log("Player Hit!");
    }
}
