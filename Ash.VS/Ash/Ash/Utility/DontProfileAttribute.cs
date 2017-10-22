 

using System;

namespace  Ash
{
    /// <summary>
    /// 避免被注入性能采样代码的标记。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class DontProfileAttribute : Attribute
    {

    }
}
