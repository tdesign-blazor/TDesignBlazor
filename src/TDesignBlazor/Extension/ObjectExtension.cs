using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="curr">当前</param>
        /// <param name="suffix">后缀</param>
        /// <returns></returns>
        public static string ToSuffix(this object curr, string suffix)
        {
            return curr is null ? suffix : curr.ToString() + suffix;
        }
        /// <summary>
        /// 添加空格后缀
        /// </summary>
        /// <param name="curr">当前</param>
        /// <param name="suffix">后缀</param>
        /// <returns></returns>
        public static string ToSpaceSuffix(this object curr, string suffix)
        {

            return curr is null ? " " + suffix : curr.ToString() + " " + suffix;
        }
    }
}
