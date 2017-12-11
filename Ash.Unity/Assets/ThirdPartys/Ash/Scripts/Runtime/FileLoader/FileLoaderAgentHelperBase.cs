using Ash.FileLoader;
using System;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 加载资源代理辅助器基类。
    /// </summary>
    public abstract class FileLoaderAgentHelperBase :  MonoBehaviour, IFileLoaderAgentHelper
    {
        public abstract event EventHandler<FileLoaderAgentHelperCompleteEventArgs> FileLoaderAgentHelperComplete;
        public abstract event EventHandler<FileLoaderAgentHelperErrorEventArgs> FileLoaderAgentHelperError;
        public abstract event EventHandler<FileLoaderAgentHelperUpdateEventArgs> FileLoaderAgentHelperUpdate;

        public abstract void ReadFile(string fileUri, Type type);

        public abstract void Reset();
    }
}
