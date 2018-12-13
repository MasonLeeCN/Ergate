using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ergate
{
    /// <summary>
    /// 返回状态码
    /// </summary>
    public enum CodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 200,

        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        Unknown_error = 501,

        /// <summary>
        /// 服务暂不可用
        /// </summary>
        [Description("服务暂不可用")]
        Service_temporarily_unavailable = 503,

        /// <summary>
        /// 无权限访问该用户数据
        /// </summary>
        [Description("无权限访问该用户数据")]
        No_permission_to_access_user_data = 401,

        /// <summary>
        /// 请求参数无效
        /// </summary>
        [Description("请求参数无效")]
        Invalid_parameter = 416,

        /// <summary>
        /// 请求参数过多
        /// </summary>
        [Description("请求参数过多")]
        Too_many_parameters = 413,

        /// <summary>
        /// 无效的Token
        /// </summary>
        [Description("无效的Token")]
        Access_token_invalid_or_no_longer_valid = 406,

        /// <summary>
        /// Token过期
        /// </summary>
        [Description("Token过期")]
        Access_token_expired = 403,

        /// <summary>
        /// 系统内部错误
        /// </summary>
        [Description("系统内部错误")]
        System_inner_error = 500
    }
}
