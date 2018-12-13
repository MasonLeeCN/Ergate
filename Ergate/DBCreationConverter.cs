using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ergate
{
    /// <summary>
    /// 对DBNull的转换处理，此处只写了转换成JSON字符串的处理，JSON字符串转对象的未处理
    /// </summary>        
    public class DBCreationConverter : DefaultContractResolver
    {
        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </returns>
        protected override IList<JsonProperty> CreateProperties(
            Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p =>
                    {
                        var jp = base.CreateProperty(p, memberSerialization);
                        jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                        return jp;
                    }).ToList();
        }

        /// <summary>
        /// json 小寫
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Resolved name of the property.
        /// </returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NullToEmptyStringValueProvider : IValueProvider
    {
        PropertyInfo _MemberInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullToEmptyStringValueProvider"/> class.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="target">The target to get the value from.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target);
            if (_MemberInfo.PropertyType == typeof(object) && result == null) result = "";
            if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
            if (_MemberInfo.PropertyType == typeof(IEnumerable<>) && result == null) result = new List<object>();
            if (_MemberInfo.PropertyType == typeof(DateTime) && result == null && (DateTime)result == DateTime.MinValue) result = "";

            if (_MemberInfo.PropertyType == typeof(MyDate))
            {
                var value = ((MyDate)result).Value;
                if (value != DateTime.MinValue)
                {
                    result = value.ToString("yyyy/MM/dd");
                }
                else
                {
                    result = "";
                }
            }

            if (_MemberInfo.PropertyType == typeof(MyDate2))
            {
                var value = ((MyDate2)result).Value;
                if (value != DateTime.MinValue)
                {
                    result = value.ToString("yyyy-MM-dd");
                }
                else
                {
                    result = "";
                }
            }


            return result;

        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="target">The target to set the value on.</param>
        /// <param name="value">The value to set on the target.</param>
        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }
    }

    /// <summary>
    /// 自定义Date类,用于返回(2018/1/1)字符串
    /// </summary>
    public class MyDate
    {
        public MyDate(string datestr)
        {
            this.Value = datestr.SafeToDateTime();
        }
        public MyDate(DateTime dt)
        {
            this.Value = dt;
        }
        public DateTime Value { get; set; }


        public static implicit operator MyDate(string m)
        {
            return new MyDate(m.SafeToDateTime());
        }

        public static implicit operator MyDate(DateTime m)
        {
            return new MyDate(m.SafeToDateTime());
        }

        public static implicit operator MyDate(DateTime? m)
        {
            return new MyDate(m.SafeToDateTime());
        }
    }

    /// <summary>
    /// 自定义Date类,用于返回(2018-1-1)字符串
    /// </summary>
    public class MyDate2
    {
        public MyDate2(string datestr)
        {
            this.Value = datestr.SafeToDateTime();
        }
        public MyDate2(DateTime dt)
        {
            this.Value = dt;
        }
        public DateTime Value { get; set; }


        public static implicit operator MyDate2(string m)
        {
            return new MyDate2(m.SafeToDateTime());
        }

        public static implicit operator MyDate2(DateTime m)
        {
            return new MyDate2(m.SafeToDateTime());
        }

        public static implicit operator MyDate2(DateTime? m)
        {
            return new MyDate2(m.SafeToDateTime());
        }
    }
}
