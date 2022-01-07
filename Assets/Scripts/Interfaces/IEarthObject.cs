public interface IEarthObject
{
    public string Name { get; }

    /// <summary>
    /// (플레이어가 적을 하나 잡는다던지 하는) 모종의 이유로 게임이 진행될 때 전체적으로 불리는 함수
    /// </summary>
    public void OnProgress();

    /// <summary>
    /// 플레이어가 이 오브젝트를 밟으면 불리는 함수
    /// </summary>
    public void OnSteppedOn();
}
