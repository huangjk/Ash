using Ash.Core;
using UnityEngine;

namespace Ash.Runtime
{
    /// <summary>
    /// 游戏框架组件抽象类。
    /// </summary>
    public abstract class AshUnityComponent : MonoBehaviour
    {
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected virtual void Awake()
        {
            if (AshUnityEntry.Instance == null)
            {
                DestroyImmediate(this as Component);
                throw new AshException("AppEngine.Instance isnt Exist");
            }
            else
            {
                AshUnityEntry.Instance.RegisterAshComponent(this);
            }
        }
    }
}