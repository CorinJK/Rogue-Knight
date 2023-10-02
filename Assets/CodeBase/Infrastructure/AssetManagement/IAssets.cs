using CodeBase.Infrastructure.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        Task<GameObject> Instantiate(string path);
        Task<GameObject> Instantiate(string path, Vector3 at);

        void CleanUp();
        void Initialize();

        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
    }
}