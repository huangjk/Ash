using System;
using System.Collections.Generic;

namespace Ash
{
    /// <summary>
    /// 转换器
    /// </summary>
    public partial class Converters
    {
        internal static Converters _instance = null;

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
        public static Converters GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Converters();
            }

            return _instance;
        }

        /// <summary>
        /// 转换器字典
        /// </summary>
        private readonly Dictionary<Type,Dictionary<Type, ITypeConverter>> coverterDictionary;

        /// <summary>
        /// 构建一个转换器
        /// </summary>
        public Converters()
        {
            coverterDictionary = new Dictionary<Type, Dictionary<Type, ITypeConverter>>();
            LoadDefaultConverters();
        }

        /// <summary>
        /// 增加一个转换器
        /// </summary>
        /// <param name="converter">转换器</param>
        public void AddConverter(ITypeConverter converter)
        {
            if (converter == null)
            {
                throw new System.ArgumentNullException();
            }

            Dictionary<Type, ITypeConverter> mapping;
            if (!coverterDictionary.TryGetValue(converter.From, out mapping))
            {
                coverterDictionary[converter.From] = mapping = new Dictionary<Type, ITypeConverter>();
            }

            mapping[converter.To] = converter;
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="to">目标类型</param>
        /// <returns>目标数据</returns>
        public object Convert(object source , Type to)
        {
            if (source == null || to == null)
            {
                throw new System.ArgumentNullException();
            }

            ITypeConverter converter;
            if (!TryGetConverter(source.GetType(), to, out converter))
            {
                throw new ConverterException("Undefined Converter [" + source.GetType() + "] to [" + to +"]");
            }

            return converter.ConvertTo(source, to);
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns>目标数据</returns>
        public TTarget Convert<TTarget>(object source)
        {
            return (TTarget)Convert(source , typeof(TTarget));
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <param name="to">目标类型</param>
        /// <returns>是否成功转换</returns>
        public bool TryConvert(object source, out object target , Type to)
        {
            if (source == null || to == null)
            {
                throw new System.ArgumentNullException();
            }

            target = null;

            ITypeConverter converter;
            if (!TryGetConverter(source.GetType(), to, out converter))
            {
                return false;
            }

            try
            {
                target = converter.ConvertTo(source, to);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns>是否成功转换</returns>
        public bool TryConvert<TTarget>(object source, out TTarget target)
        {
            target = default(TTarget);
            object obj;
            if (!TryConvert(source, out obj , typeof(TTarget)))
            {
                return false;
            }
            target = (TTarget) obj; 
            return true;
        }

        /// <summary>
        /// 获取类型所需的转换器
        /// </summary>
        /// <param name="from">来源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="converter">转换器</param>
        /// <returns>是否成功获取转换器</returns>
        private bool TryGetConverter(Type from , Type to, out ITypeConverter converter)
        {
            bool status;
            Dictionary<Type, ITypeConverter> toDictionary;
            converter = null;

            do
            {
                status = coverterDictionary.TryGetValue(from, out toDictionary);
                from = from.BaseType;
            } while (!status && from != null);

            if (!status)
            {
                return false;
            }

            do
            {
                status = toDictionary.TryGetValue(to, out converter);
                to = to.BaseType;
            } while (!status && to != null);

            return status;
        }
    }
}
