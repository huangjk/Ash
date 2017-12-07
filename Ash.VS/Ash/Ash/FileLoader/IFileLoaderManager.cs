using Ash.ObjectPool;

namespace Ash.FileLoader
{
    public interface IFileLoaderManager
    {
        float AssetAutoReleaseInterval { get; set; }
        int AssetCapacity { get; set; }
        float AssetExpireTime { get; set; }
        int AssetPriority { get; set; }
        int LoadFreeAgentCount { get; }
        int LoadTotalAgentCount { get; }
        int LoadWaitingTaskCount { get; }
        int LoadWorkingAgentCount { get; }

        void AddLoadResourceAgentHelper(IFileLoaderAgentHelper fileLoaderAgentHelper);
        void LoadFileByteData(string fileUrl, LoadFileCallbacks loadFileCallbacks);
        void LoadFileByteData(string fileUrl, LoadFileCallbacks loadFileCallbacks, object userData);
        void LoadFile<T>(string fileUrl, LoadFileCallbacks loadFileCallbacks);
        void LoadFile<T>(string fileUrl, LoadFileCallbacks loadFileCallbacks, object userData);
        void SetResourceHelper(IFileLoaderHelper fileLoaderHelper);
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);
        void UnloadFile(object asset);
    }
}