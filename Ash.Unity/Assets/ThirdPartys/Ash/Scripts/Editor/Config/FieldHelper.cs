using System;
using UnityEngine;

namespace AshUnityEditor.Config
{
    /// <summary>
    /// 域的类
    /// </summary>
    public class FieldHelper
    {
        /// <summary>
        /// 得到当前域的类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FieldType GetCurrentFieldType(System.Type value)
        {
            if (value.IsGenericType)
            {
                System.Type t = value.GetGenericArguments()[0];

                if (t.IsEnum)
                {
                    return FieldType.GEN_ENUM;
                }
                else if (t == typeof(Bounds))
                {
                    return FieldType.GEN_BOUNDS;
                }
                else if (t == typeof(Color))
                {
                    return FieldType.GEN_COLOR;
                }
                else if (t == typeof(AnimationCurve))
                {
                    return FieldType.GEN_ANIMATIONCURVE;
                }
                else if (t == typeof(string))
                {
                    return FieldType.GEN_STRING;
                }
                else if (t == typeof(float))
                {
                    return FieldType.GEN_FLOAT;
                }
                else if (t == typeof(int))
                {
                    return FieldType.GEN_INT;
                }
                else if (t == typeof(bool))
                {
                    return FieldType.GEN_BOOL;
                }
                else if (t == typeof(Vector2))
                {
                    return FieldType.GEN_VECTOR2;
                }
                else if (t == typeof(Vector3))
                {
                    return FieldType.GEN_VECTOR3;
                }
                else if (t == typeof(Vector4))
                {
                    return FieldType.GEN_VECTOR4;
                }

                return FieldType.GEN_STRING;
            }

            if (value.IsEnum)
            {
                return FieldType.ENUM;
            }
            else if (value == typeof(Bounds))
            {
                return FieldType.BOUNDS;
            }
            else if (value == typeof(Color))
            {
                return FieldType.COLOR;
            }
            else if (value == typeof(AnimationCurve))
            {
                return FieldType.ANIMATIONCURVE;
            }
            else if (value == typeof(string))
            {
                return FieldType.STRING;
            }
            else if (value == typeof(float))
            {
                return FieldType.FLOAT;
            }
            else if (value == typeof(int))
            {
                return FieldType.INT;
            }
            else if (value == typeof(bool))
            {
                return FieldType.BOOL;
            }
            else if (value == typeof(Vector2))
            {
                return FieldType.VECTOR2;
            }
            else if (value == typeof(Vector3))
            {
                return FieldType.VECTOR3;
            }
            else if (value == typeof(Vector4))
            {
                return FieldType.VECTOR4;
            }

            return FieldType.STRING;
        }

        /// <summary>
        /// 得到类型绘制的宽度
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public static int GetTypeWide(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.INT:
                    return 50;
                case FieldType.STRING:
                    return 100;
                case FieldType.FLOAT:
                    return 80;
                case FieldType.BOOL:
                    return 40;
                case FieldType.VECTOR2:
                    return 80;
                case FieldType.VECTOR3:
                    return 100;
                case FieldType.VECTOR4:
                    return 100;
                case FieldType.COLOR:
                    return 100;
                case FieldType.CURVE:
                    return 100;
                case FieldType.BOUNDS:
                    return 170;
                case FieldType.GEN_INT:
                    return 90;
                case FieldType.GEN_STRING:
                    return 90;
                case FieldType.GEN_FLOAT:
                    return 90;
                case FieldType.GEN_BOOL:
                    return 70;
                case FieldType.GEN_VECTOR2:
                    return 100;
                case FieldType.GEN_VECTOR3:
                    return 110;
                case FieldType.GEN_VECTOR4:
                    return 120;
                case FieldType.GEN_COLOR:
                    return 110;
                case FieldType.GEN_CURVE:
                    return 110;
                case FieldType.GEN_BOUNDS:
                    return 190;
                case FieldType.ANIMATIONCURVE:
                    return 100;
                case FieldType.GEN_ANIMATIONCURVE:
                    return 120;
                case FieldType.ENUM:
                    return 60;
                case FieldType.GEN_ENUM:
                    return 80;
                default:
                    return 100;
            }
        }
    }
}
