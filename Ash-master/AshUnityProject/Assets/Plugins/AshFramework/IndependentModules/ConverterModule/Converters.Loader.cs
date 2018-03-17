using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ash
{
    public partial class Converters
    {
        /// <summary>
        /// 加载默认的转换器
        /// </summary>
        /// <returns>默认的转换器</returns>
        private void LoadDefaultConverters()
        {
            var loadConverters = new ITypeConverter[]
            {
                new BoolStringConverter(),
                new ByteStringConverter(),
                new CharStringConverter(),
                new DateTimeStringConverter(),
                new DecimalStringConverter(),
                new DoubleStringConverter(),
                new EnumStringConverter(),
                new Int16StringConverter(),
                new Int32StringConverter(),
                new Int64StringConverter(),
                new SByteStringConverter(),
                new SingleStringConverter(),
                new StringBoolConverter(),
                new StringByteConverter(),
                new StringCharConverter(),
                new StringDateTimeConverter(),
                new StringDecimalConverter(),
                new StringDoubleConverter(),
                new StringEnumConverter(),
                new StringInt16Converter(),
                new StringInt32Converter(),
                new StringInt64Converter(),
                new StringSByteConverter(),
                new StringSingleConverter(),
                new StringStringConverter(),
                new StringUInt16Converter(),
                new StringUInt32Converter(),
                new StringUInt64Converter(),
                new UInt16StringConverter(),
                new UInt32StringConverter(),
                new UInt64StringConverter(),
            };

            //var converters = new Converters();

            foreach (var tmp in loadConverters)
            {
                AddConverter(tmp);
            }

            //return converters;
        }
    }
}
