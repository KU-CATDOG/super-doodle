using System.Collections;
using UnityEngine;

public enum ObjectSide
{
    Player,
    Enemy,
}

public abstract class EarthObjectController
{
    public EarthObject Holder { get; private set; }

    public bool IsResourceLoaded { get; private set; }

    public abstract ObjectSide Side { get; }

    /// <summary>
    /// 공격이 가능한 상태여도 한 대 맞아서 무적 시간인 도중에는 다른 오브젝트를 때릴 수 없음
    /// </summary>
    public bool CanAttackOtherObject => AttackEnabled && Time.time - recentHitTime > InvincibleSecond;

    protected abstract bool AttackEnabled { get; }

    /// <summary>
    /// 한대 맞고 몇 초동안 무적인지
    /// </summary>
    protected abstract float InvincibleSecond { get; }

    /// <summary>
    /// 가장 최근에 맞은 게 언제인지
    /// </summary>
    private float recentHitTime = float.MinValue;

    protected GameObject resource;

    /// <summary>
    /// 지구를 돌고 있는 물체의 월드 포지션
    /// </summary>
    public Vector3 ResourceWorldPos => resource.transform.position;

    public void AttachThis(EarthObject earthObject)
    {
        Holder = earthObject;
        Holder.Earth.RegisterEarthObjectController(this);

        OnAttached();
        Holder.StartCoroutine(Load());
    }

    public void DetachThis()
    {
        Holder.Earth.UnregisterEarthObjectController(this);

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
        t.localPosition = new Vector3(0, Holder.Earth.Radius + 1, 0);

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

    protected virtual void OnMeleeAttack()
    {

    }

    public void MeleeAttackThis(EarthObject hitter)
    {
        var now = Time.time;

        if (now - recentHitTime < InvincibleSecond) return;

        recentHitTime = Time.time;
        OnMeleeAttack();
        OnMeleeHit(hitter);
    }

    protected abstract void OnMeleeHit(EarthObject hitter);
}
