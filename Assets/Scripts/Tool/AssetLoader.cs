using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tool
{
    public static class AssetLoader
    {
        /// <summary>
        /// Assets/ 아래의 애셋을 로드하는 루틴을 리턴한다.
        /// </summary>
        public static IEnumerator LoadAssetAsync<T>(string prefabAddress, Action<LoadedAsset<T>> onFinished) where T : UnityEngine.Object
        {
            var op = Addressables.LoadAssetAsync<T>("Assets/" + prefabAddress);

            yield return op;

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                onFinished(null);
                yield break;
            }

            onFinished(new LoadedAsset<T>(op));
        }

        /// <summary>
        /// Assets/ 아래의 애셋을 로드하고 로드가 끝날 때까지 기다린다. (여러 개를 연속으로 로드할 때 쓰면 심각한 퍼포먼스 저하가 있을 수도 있음)
        /// </summary>
        public static LoadedAsset<T> LoadAsset<T>(string prefabAddress) where T : UnityEngine.Object
        {
            var op = Addressables.LoadAssetAsync<T>("Assets/" + prefabAddress);

            op.WaitForCompletion();

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                return null;
            }

            return new LoadedAsset<T>(op);
        }
    }

    /// <summary>
    /// 이걸 참조하는 인스턴스가 전부 Destroy되거나 하면 이것도 Release 해줘야 함
    /// </summary>
    public class LoadedAsset<T> where T : UnityEngine.Object
    {
        private readonly T resource;

        private readonly AsyncOperationHandle<T> resourceHandle;

        private bool isReleased;

        public T Resource => isReleased ? null : resource;

        public LoadedAsset(AsyncOperationHandle<T> handle)
        {
            resourceHandle = handle;
            resource = handle.Result;
        }

        public void Release()
        {
            isReleased = true;
            Addressables.Release(resourceHandle);
        }
    }
}
