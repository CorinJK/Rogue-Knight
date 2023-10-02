using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssets
    {
        // Словарь кеша завершенных операций
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        // Словарь handle между началом загрузки и ее окончанием
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            // Если словарь есть, вернуть результат
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete
                (Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete
                (Addressables.LoadAssetAsync<T>(address), address);
        }

        // Загрузка префабов 
        public Task<GameObject> Instantiate(string address) => 
            Addressables.InstantiateAsync(address).Task;

        // Перегрузка: Загрузка префабов + позиция
        public Task<GameObject> Instantiate(string address, Vector3 at) => 
            Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        // Чистка handle
        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);

            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle => _completedCache[cacheKey] = completeHandle;     // Если операция завершилась, записать в кэш

            AddHandle(cacheKey, handle);                              // Для каждого запроса сохранить ассинхронную оперцию, пока та не завершилась

            return await handle.Task;                                                             // Выйти, дождавшись завершения загрузки
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }
            resourceHandles.Add(handle);
        }
    }
}