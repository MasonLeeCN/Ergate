using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ergate
{
    /// <summary>
    /// Checker 的摘要说明
    /// false 表示验证失败,true表示验证成功
    /// </summary>
    public class Checker
    {
        public static bool UserName(string value)
        {
            var pattern = @"^[a-zA-Z0-9_-]{4,16}$";
            return Test(pattern, value);
        }

        public static bool Int_Zheng(string value)
        {
            var pattern = @"^\d +$";
            return Test(pattern, value);
        }

        public static bool Int_Fu(string value)
        {
            var pattern = @"^-\d+$";
            return Test(pattern, value);
        }

        public static bool Int(string value)
        {
            var pattern = @"^-?\d+$";
            return Test(pattern, value);
        }
        public static bool Email(string value)
        {
            var pattern = @"^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$";
            return Test(pattern, value);
        }
        public static bool Mobile(string value)
        {
            var pattern = @"^1[34578]\d{9}$";
            return Test(pattern, value);
        }
        public static bool IdCard(string value)
        {
            var pattern = @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$";
            return Test(pattern, value);
        }
        public static bool Url(string value)
        {
            var pattern = @"^((https?|ftp|file):\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
            return Test(pattern, value);
        }
        public static bool Ipv4(string value)
        {
            var pattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            return Test(pattern, value);
        }
        public static bool Rgb(string value)
        {
            var pattern = @"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";
            return Test(pattern, value);
        }
        public static bool Date(string value)
        {
            var pattern = @"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$";
            return Test(pattern, value);
        }
        public static bool QQ(string value)
        {
            var pattern = @"^[1-9][0-9]{4,10}$";
            return Test(pattern, value);
        }
        public static bool Wechat(string value)
        {
            var pattern = @"^[a-zA-Z]([-_a-zA-Z0-9]{5,19})+$";
            return Test(pattern, value);
        }
        public static bool Chepai(string value)
        {
            var pattern = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4";
            return Test(pattern, value);
        }
        public static bool Chinese(string value)
        {
            var pattern = @"[\u4E00-\u9FA5]";
            return Test(pattern, value);
        }
        public bool Length(string value, int len)
        {
            if (value.SafeToString().Length == len)
                return true;
            return false;
        }
        private static bool Test(string pattern, string value)
        {
            var reg = new Regex(pattern);
            return reg.IsMatch(value);
        }
    }
}