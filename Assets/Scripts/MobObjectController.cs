using System.Collections;
using Tool;
using UnityEngine;
using TMPro;

public class MobObjectController : EarthObjectController, IEnemyController
{
    public KeyCode TargetKeyCode { get; private set; }

    private TextMeshPro keyText;

    protected override IEnumerator LoadResources()
    {
        yield return AssetLoader.LoadAssetAsync<GameObject>("Prefabs/EarthObjects/Mob.prefab", x =>
        {
            resource = Object.Instantiate(x.Resource);
            keyText = resource.transform.GetComponentInChildren<TextMeshPro>();
            keyText.text = TargetKeyCode.ToString();
        });
    }

    protected override void OnAttached()
    {
        TargetKeyCode = KeyGenerator.GetKeyCode();
    }

    protected override void OnDetached()
    {
    }

    protected override void UnloadResources()
    {
    }

    public bool OnHitByKey(KeyCode code)
    {
        if (TargetKeyCode != code) return false;

        // 제대로 된 키에 맞으면 죽는다.
        Holder.Earth.KillEarthObject(Holder);

        return true;
    }
}
