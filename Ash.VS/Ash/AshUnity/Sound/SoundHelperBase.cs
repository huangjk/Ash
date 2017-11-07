//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://Ash.cn/
// Feedback: mailto:jiangyin@Ash.cn
//------------------------------------------------------------

using Ash.Sound;
using UnityEngine;

namespace UnityAsh.Runtime
{
    /// <summary>
    /// 声音辅助器基类。
    /// </summary>
    public abstract class SoundHelperBase : MonoBehaviour, ISoundHelper
    {
        /// <summary>
        /// 释放声音资源。
        /// </summary>
        /// <param name="soundAsset">要释放的声音资源。</param>
        public abstract void ReleaseSoundAsset(object soundAsset);
    }
}
