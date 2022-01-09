using System.Collections;
using Tool;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : EarthObjectController
{
    protected override IEnumerator LoadResources()
    {
        using var loader = new AssetLoader();

        yield return loader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Player.prefab", x =>
        {
            resource = Object.Instantiate(x);
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
}
