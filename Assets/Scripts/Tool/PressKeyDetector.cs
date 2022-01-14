using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tool
{
    public class SingleKeyPressedEvent : IEvent
    {
        public KeyCode PressedKey { get; }

        public SingleKeyPressedEvent(KeyCode pressedKey)
        {
            PressedKey = pressedKey;
        }
    }

    public class MultipleKeyPressedEvent : IEvent
    {
        public IReadOnlyCollection<KeyCode> PressedKey { get; }

        public MultipleKeyPressedEvent(HashSet<KeyCode> pressedKey)
        {
            PressedKey = pressedKey;
        }
    }

    public class KeyPressInfo
    {
        public KeyCode PressedKey { get; }

        public float PressedTime { get; }

        /// <summary>
        /// 성공적으로 사용되어 이젠 필요없어진 경우 false로 설정
        /// </summary>
        public bool IsAlive { get; set; }

        public KeyPressInfo(KeyCode key, float time)
        {
            PressedKey = key;
            PressedTime = time;
            IsAlive = true;
        }
    }

    public class PressKeyDetector : MonoBehaviour
    {
        // 여기 있는 키들만 입력으로 탐지한다.
        private static readonly List<KeyCode> KeyPool = new List<KeyCode>();

        // 한번 누른 키가 몇 초동안 살아있도록 할지 (동시에 다른 키를 여럿 누를 때 용이하도록)
        private const float KeyPressLifetime = 0.15f;

        private readonly Dictionary<KeyCode, KeyPressInfo> pressedKey = new Dictionary<KeyCode, KeyPressInfo>();

        public static void Init()
        {
            var go = new GameObject();
            DontDestroyOnLoad(go);

            go.name = "PressKeyDetector";
            go.AddComponent<PressKeyDetector>();

            for (var i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
            {
                KeyPool.Add(i);
            }

            for (var i = KeyCode.A; i <= KeyCode.Z; i++)
            {
                KeyPool.Add(i);
            }

            KeyPool.Add(KeyCode.Space);
        }

        private readonly List<KeyCode> toRemove = new List<KeyCode>();

        private void Update()
        {
            var newlyPressedKey = KeyCode.None;

            var now = Time.time;

            // 등장하는 키 중에서만 확인을 한다.
            foreach (var candidate in KeyPool)
            {
                if (Input.GetKeyDown(candidate))
                {
                    pressedKey[candidate] = new KeyPressInfo(candidate, now);
                    newlyPressedKey = candidate;
                }
            }

            if (newlyPressedKey == KeyCode.None) return;

            // 싱글 키 눌림 이벤트는 한 프레임에 무조건 하나씩만 보냄
            MessageSystem.Instance.Publish(new SingleKeyPressedEvent(newlyPressedKey));

            // 눌린 지 오래 지나거나 죽었으면
            foreach (var kv in pressedKey)
            {
                var info = kv.Value;

                if (now - info.PressedTime < KeyPressLifetime && info.IsAlive) continue;
                toRemove.Add(kv.Key);
            }

            foreach (var k in toRemove)
            {
                pressedKey.Remove(k);
            }

            if (pressedKey.Count > 1)
            {
                MessageSystem.Instance.Publish(new MultipleKeyPressedEvent(new HashSet<KeyCode>(pressedKey.Values.Select(x => x.PressedKey))));
            }

            toRemove.Clear();
        }
    }
}
