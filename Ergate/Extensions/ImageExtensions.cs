using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Adds the image prefix.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns></returns>
        public static string AddImagePrefix(this string imageUrl)
        {
            if (string.IsNullOrEmpty(RequestInfo.ServerUrl))
            {
                throw new MsgException("必须在startup中先初始化RequestInfo.ServerUrl这个参数");
            }
            if (!string.IsNullOrEmpty(imageUrl))
            {
                if (!imageUrl.StartsWith($"http"))
                {
                    return $"{RequestInfo.ServerUrl}{imageUrl}";
                }
            }
            return imageUrl.SafeToString();
        }

        /// <summary>
        /// Removes the image prefix.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns></returns>
        public static string RemoveImagePrefix(this string imageUrl)
        {
            if (string.IsNullOrEmpty(RequestInfo.ServerUrl))
            {
                throw new MsgException("必须在startup中先初始化RequestInfo.ServerUrl这个参数");
            }
            imageUrl = imageUrl.SafeToString();
            imageUrl = imageUrl.Replace(RequestInfo.ServerUrl, "");
            return imageUrl;
        }
    }
}
