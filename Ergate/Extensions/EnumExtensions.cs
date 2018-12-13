using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ergate
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetMessage<T>(this T code) where T : Enum
        {
            var codeDictionary = new Dictionary<string, string>();

            if (codeDictionary.Count == 0)
            {

                var fields = code.GetType().GetFields();

                foreach (var f in fields)
                {
                    if (f.Name != "value__")
                    {
                        object[] objs = f.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
                        if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
                        {
                            codeDictionary.Add(f.GetValue(null).ToString(), code.ToString());
                        }
                        else
                        {
                            var desc = (DescriptionAttribute)objs[0];
                            codeDictionary.Add(f.GetValue(null).ToString(), desc.Description);
                        }
                    }
                }
            }
            return codeDictionary[code.ToString()];
        }
    }
}
