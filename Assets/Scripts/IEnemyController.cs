using UnityEngine;

public interface IEnemyController
{
    /// <summary>
    /// 유효한 타격이었으면 true를 리턴한다.
    /// </summary>
    public bool OnHitByKey(KeyCode key);
}
