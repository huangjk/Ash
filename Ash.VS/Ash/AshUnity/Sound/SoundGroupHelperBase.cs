//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://Ash.cn/
// Feedback: mailto:jiangyin@Ash.cn
//------------------------------------------------------------

using Ash.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityAsh.Runtime
{
    /// <summary>
    /// 声音组辅助器基类。
    /// </summary>
    public abstract class SoundGroupHelperBase : MonoBehaviour, ISoundGroupHelper
    {
        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;

        /// <summary>
        /// 获取或设置声音组辅助器所在的混音组。
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get
            {
                return m_AudioMixerGroup;
            }
            set
            {
                m_AudioMixerGroup = value;
            }
        }
    }
}
