using System.Collections;
using UnityEngine;

public abstract class EarthObjectController
{
    public EarthObject Holder { get; private set; }

    public bool IsResourceLoaded { get; private set; }

    protected GameObject resource;

    public void AttachThis(EarthObject earthObject)
    {
        Holder = earthObject;

        OnAttached();
        Holder.StartCoroutine(Load());
    }

    public void DetachThis()
    {
        OnDetached();
        UnloadResources();

        Holder = null;
        Object.Destroy(resource);
        resource = null;
    }

    public virtual void OnUpdate()
    {
    }

    protected abstract void OnAttached();

    protected abstract void OnDetached();

    private IEnumerator Load()
    {
        IsResourceLoaded = false;

        yield return LoadResources();

        var t = resource.transform;
        t.parent = Holder.transform;

        // 오브젝트는 중앙에 있지만 스프라이트 등 눈에 보이는 놈들은 지구 위에서 도는 모습으로 보여야 함
        t.localPosition = new Vector3(0, Holder.Earth.Radius / 2f + 1, 0);

        IsResourceLoaded = true;
    }

    protected abstract IEnumerator LoadResources();

    /// <summary>
    /// resource가 Destroy 되기 전에 필요한 처리가 있으면 여기서 해결
    /// </summary>
    protected abstract void UnloadResources();

    /// <summary>
    /// 땅이 제시한 키를 맞추건 틀리건 그 위에 있는 모든 오브젝트들의 이 함수가 불림
    /// </summary>
    public virtual void OnEarthKeyPressed(bool corrected)
    {
    }
}
