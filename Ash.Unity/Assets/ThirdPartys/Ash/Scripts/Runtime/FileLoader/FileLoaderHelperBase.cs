using Ash.FileLoader;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 资源辅助器基类。
    /// </summary>
    public abstract class FileLoaderHelperBase : MonoBehaviour, IFileLoaderHelper
    {      
        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        public abstract void Release(object objectToRelease);
    }
}
