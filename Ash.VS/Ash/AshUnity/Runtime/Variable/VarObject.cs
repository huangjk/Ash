using Ash;

namespace AshUnity
{
    /// <summary>
    /// object 变量类。
    /// </summary>
    public class VarObject : Variable<object>
    {
        /// <summary>
        /// 初始化 object 变量类的新实例。
        /// </summary>
        public VarObject()
        {

        }

        /// <summary>
        /// 初始化 object 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarObject(object value)
            : base(value)
        {

        }
    }
}
