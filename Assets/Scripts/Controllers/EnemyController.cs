using UnityEngine;

public abstract class EnemyController : EarthObjectController
{
    /// <summary>
    /// 한대 맞고 몇 초동안 무적인지
    /// </summary>
    protected abstract float InvincibleSecond { get; }

    /// <summary>
    /// 가장 최근에 맞은 게 언제인지
    /// </summary>
    private float recentHitTime = float.MinValue;

    /// <summary>
    /// 플레이어에게 부딪혔을 때 불리는 함수
    /// </summary>
    public void AttackThis()
    {
        var now = Time.time;

        if (now - recentHitTime < InvincibleSecond) return;

        recentHitTime = Time.time;
        OnHit();
    }

    protected abstract void OnHit();
}
