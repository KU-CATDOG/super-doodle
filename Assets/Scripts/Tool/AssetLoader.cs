using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tool
{
    /// <summary>
    /// 로드해 와서 볼일 다 보면 Dispose 해 줘야 메모리 안 샘. using var = 로 스코프 내에서 사용해도 된다.
    /// </summary>
    public class AssetLoader : IDisposable
    {
        private readonly HashSet<AsyncOperationHandle> ops = new HashSet<AsyncOperationHandle>();

        private AsyncOperationHandle<T> LoadAsset<T>(string address) where T : UnityEngine.Object
        {
            var op = Addressables.LoadAssetAsync<T>(address);
            ops.Add(op);

            return op;
        }

        /// <summary>
        /// Assets/Prefab/ 아래의 프리팹을 로드한다.
        /// </summary>
        public void LoadPrefab<T>(string prefabAddress, Action<T> onFinished) where T : UnityEngine.Object
        {
            var op = LoadAsset<T>("Assets/Prefabs/" + prefabAddress);
            op.Completed += x =>
            {
                if (x.Status != AsyncOperationStatus.Succeeded)
                {
                    onFinished(null);
                    return;
                }

                onFinished(x.Result);
            };
        }

        /// <summary>
        /// Assets/Prefab/ 아래의 프리팹을 로드하고 로드가 끝날 때까지 기다린다.
        /// </summary>
        public void LoadPrefabSync<T>(string prefabAddress, Action<T> onFinished) where T : UnityEngine.Object
        {
            var op = LoadAsset<T>("Assets/Prefabs/" + prefabAddress);
            op.Completed += x =>
            {
                if (x.Status != AsyncOperationStatus.Succeeded)
                {
                    onFinished(null);
                    return;
                }

                onFinished(x.Result);
            };

            op.WaitForCompletion();
        }

        public void Dispose()
        {
            foreach (var op in ops)
            {
                Addressables.Release(op);
            }
        }
    }
}
