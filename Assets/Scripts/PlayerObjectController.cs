using System.Collections.Generic;
using System.Collections;
using Tool;
using UnityEngine;

public class PlayerObjectController : EarthObjectController
{
    public float AttackRangeRadian { get; set; } = Mathf.PI / 4;

    protected override IEnumerator LoadResources()
    {
        yield return AssetLoader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Player.prefab", x =>
        {
            resource = Object.Instantiate(x.Resource);
        });
    }

    protected override void OnAttached()
    {
        MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnSingleKeyEvent);
    }

    protected override void OnDetached()
    {
    }

    protected override void UnloadResources()
    {
    }

    private void OnSingleKeyEvent(IEvent e)
    {
        if (!(e is SingleKeyPressedEvent se)) return;

        Debug.Log("Attacking with " + se.PressedKey);

        foreach (var i in GetEnemyInAttackRange())
        {
            i.OnHitByKey(se.PressedKey);
        }
    }

    private List<IEnemyController> GetEnemyInAttackRange()
    {
        var result = new List<IEnemyController>();

        foreach (var i in Holder.Earth.EarthObjects)
        {
            if (!(i.Controller is IEnemyController enemy)) continue;

            var distanceRadian = (i.Radian % (Mathf.PI * 2)) - (Holder.Radian % (Mathf.PI * 2));

            if (distanceRadian < 0 || distanceRadian > AttackRangeRadian) continue;

            result.Add(enemy);
        }

        return result;
    }
}
