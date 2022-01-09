using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tool
{
    /// <summary>
    /// 로드해 와서 볼일 다 보면 Dispose 해 줘야 메모리 안 샘.
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
        /// Assets/ 아래의 애셋을 로드하는 루틴을 리턴한다.
        /// </summary>
        public IEnumerator LoadAssetAsync<T>(string prefabAddress, Action<T> onFinished) where T : UnityEngine.Object
        {
            var op = LoadAsset<T>("Assets/" + prefabAddress);

            yield return op;

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                onFinished(null);
                yield break;
            }

            onFinished(op.Result);
        }

        /// <summary>
        /// Assets/ 아래의 애셋을 로드하고 로드가 끝날 때까지 기다린다. (여러 개를 연속으로 로드할 때 쓰면 심각한 퍼포먼스 저하가 있을 수도 있음)
        /// </summary>
        public static void LoadAsset<T>(string prefabAddress, Action<T> onFinished) where T : UnityEngine.Object
        {
            var op = Addressables.LoadAssetAsync<T>("Assets/" + prefabAddress);

            op.WaitForCompletion();

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                onFinished(null);
                return;
            }

            onFinished(op.Result);

            Addressables.Release(op);
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
