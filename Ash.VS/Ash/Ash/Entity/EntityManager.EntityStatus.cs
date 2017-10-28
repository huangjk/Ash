//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://Ash.cn/
// Feedback: mailto:jiangyin@Ash.cn
//------------------------------------------------------------

namespace Ash.Entity
{
    internal partial class EntityManager
    {
        /// <summary>
        /// 实体状态。
        /// </summary>
        private enum EntityStatus
        {
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillHide,
            Hidden,
            WillRecycle,
            Recycled,
        }
    }
}
