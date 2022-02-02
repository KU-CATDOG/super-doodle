using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    /// <summary>
    /// 땅에 공급해 줄 키를 생성하는 기능을 가진 인터페이스
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// 이 제너레이터에서 나올 수 있는 키들의 집합
        /// </summary>
        public IReadOnlyCollection<KeyCode> CandidatePool { get; }

        public KeyCode GetKeyCode();
    }

    public static class EarthKeyGenerator
    {
        public static IKeyGenerator KeyGenerator { get; set; } = new RandomKeyGenerator();
    }

    public class NullKeyGenerator : IKeyGenerator
    {
        public IReadOnlyCollection<KeyCode> CandidatePool => new HashSet<KeyCode> { KeyCode.A };

        public KeyCode GetKeyCode() => KeyCode.A;
    }

    public class RandomKeyGenerator : IKeyGenerator
    {
        private readonly List<KeyCode> pool = new List<KeyCode>();

        public IReadOnlyCollection<KeyCode> CandidatePool => pool;

        public RandomKeyGenerator()
        {
            for (var i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
            {
                pool.Add(i);
            }

            for (var i = KeyCode.A; i <= KeyCode.Z; i++)
            {
                pool.Add(i);
            }

            for (var i = KeyCode.Insert; i <= KeyCode.F12; i++)
            {
                pool.Add(i);
            }
        }

        public KeyCode GetKeyCode() => pool[Random.Range(0, pool.Count)];
    }
}
