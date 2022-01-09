using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public interface IEvent
    {
    }

    public class MessageSystem : MonoBehaviour
    {
        private static MessageSystem instance;

        public static MessageSystem Instance
        {
            get
            {
                if (instance != null) return instance;

                var go = new GameObject();
                DontDestroyOnLoad(go);

                go.name = "MessageSystem";
                instance = go.AddComponent<MessageSystem>();
                return instance;
            }
        }

        private readonly Dictionary<Type, HashSet<Action<IEvent>>> callbacks = new Dictionary<Type, HashSet<Action<IEvent>>>();
        private readonly HashSet<IEvent> publishedThisFrame = new HashSet<IEvent>();

        private bool isCurrentlyHandlingEvent = false;

        // Update()에서 콜백들에 foreach를 도는 중에 내부에서 구독/취소 요청이 들어오면 Enumerator가 깨져 버릴 수 있으므로
        // 이벤트를 처리하는 도중에는 여기 모아 뒀다가 끝나고 일괄적으로 넣는다.
        private readonly Dictionary<Type, HashSet<Action<IEvent>>> subscribeBuffer = new Dictionary<Type, HashSet<Action<IEvent>>>();

        private readonly Dictionary<Type, HashSet<Action<IEvent>>> unsubscribeBuffer = new Dictionary<Type, HashSet<Action<IEvent>>>();

        public void Publish<T>(T e) where T : IEvent
        {
            publishedThisFrame.Add(e);
        }

        public void Subscribe<T>(Action<IEvent> callback) where T : IEvent
        {
            var type = typeof(T);

            var dict = isCurrentlyHandlingEvent ? subscribeBuffer : callbacks;

            if (!dict.TryGetValue(type, out var set))
            {
                set = new HashSet<Action<IEvent>>();
                dict[type] = set;
            }

            set.Add(callback);
        }

        public void Unsubscribe<T>(Action<IEvent> callback) where T : IEvent
        {
            var type = typeof(T);

            if (isCurrentlyHandlingEvent)
            {
                if (!unsubscribeBuffer.TryGetValue(type, out var set))
                {
                    set = new HashSet<Action<IEvent>>();
                    unsubscribeBuffer[type] = set;
                }

                set.Add(callback);
            }
            else
            {
                if (!callbacks.TryGetValue(type, out var set)) return;
                set.Remove(callback);
            }
        }

        private void Update()
        {
            isCurrentlyHandlingEvent = true;

            foreach (var e in publishedThisFrame)
            {
                if (!callbacks.TryGetValue(e.GetType(), out var set)) continue;

                foreach (var cb in set)
                {
                    cb(e);
                }
            }

            publishedThisFrame.Clear();

            isCurrentlyHandlingEvent = false;

            foreach (var kv in subscribeBuffer)
            {
                if (!callbacks.TryGetValue(kv.Key, out var set))
                {
                    callbacks[kv.Key] = kv.Value;
                    continue;
                }

                foreach (var cb in kv.Value)
                {
                    set.Add(cb);
                }
            }

            foreach (var kv in unsubscribeBuffer)
            {
                if (!callbacks.TryGetValue(kv.Key, out var set)) continue;

                foreach (var cb in kv.Value)
                {
                    set.Remove(cb);
                }
            }
        }
    }
}
