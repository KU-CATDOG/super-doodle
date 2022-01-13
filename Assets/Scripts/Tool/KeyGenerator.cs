using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public interface IKeyGenerator
    {
        /// <summary>
        /// 이 제너레이터에서 나올 수 있는 키들의 집합
        /// </summary>

        public KeyCode GetKeyCode();
    }

    public static class KeyGenerator
    {
        public static IReadOnlyList<KeyCode> AllKeyPool { get; }

        static KeyGenerator()
        {
            var pool = new List<KeyCode>();

            for (var i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
            {
                pool.Add(i);
            }

            for (var i = KeyCode.A; i <= KeyCode.Z; i++)
            {
                pool.Add(i);
            }

            AllKeyPool = pool;
        }

        private static IKeyGenerator keyGenerator = new RandomKeyGenerator();

        public static KeyCode GetKeyCode() => keyGenerator.GetKeyCode();
    }

    public class NullKeyGenerator : IKeyGenerator
    {
        private static readonly List<KeyCode> pool = new List<KeyCode> { KeyCode.A };

        public IReadOnlyCollection<KeyCode> KeyPool => pool;

        public KeyCode GetKeyCode() => KeyCode.A;
    }

    public class RandomKeyGenerator : IKeyGenerator
    {
        public KeyCode GetKeyCode() => KeyGenerator.AllKeyPool[Random.Range(0, KeyGenerator.AllKeyPool.Count)];
    }
}
