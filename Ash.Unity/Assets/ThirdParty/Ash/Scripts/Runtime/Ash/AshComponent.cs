using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 游戏框架组件抽象类。
    /// </summary>
    public abstract class AshComponent : MonoBehaviour
    {
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected virtual void Awake()
        {
            AshApp.RegisterComponent(this);
        }
    }
}
