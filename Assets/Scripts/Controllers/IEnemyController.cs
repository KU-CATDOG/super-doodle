using UnityEngine;

public interface IEnemyController
{
    /// <summary>
    /// 플레이어에게 부딪혔을 때 불리는 함수
    /// </summary>
    public void OnHit();
}
