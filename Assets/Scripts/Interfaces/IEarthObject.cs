public interface IEarthObject
{
    public string Name { get; }

    /// <summary>
    /// 플레이어가 이 오브젝트를 밟으면 불리는 함수
    /// </summary>
    public void OnSteppedOn();
}
