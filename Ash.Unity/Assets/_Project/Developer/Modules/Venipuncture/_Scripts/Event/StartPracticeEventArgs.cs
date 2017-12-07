using Ash.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Venipuncture
{
    /// <summary>
    /// 开始实操事件
    /// </summary>
    public class StartPracticeEventArgs : AshEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.StartPractice;
            }
        }

        public StartPracticeEventArgs(int practiceLevel, object userData)
        {
            PracticeLevel = practiceLevel;
            UserData = userData;
        }

        /// <summary>
        /// 考试级别（自由训练，模拟考试，考试）
        /// </summary>
        public int PracticeLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
