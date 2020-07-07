using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintDemo.Common
{
    public static partial class StringHelper
    {

        /// <summary>
        /// input ="ABCD",preffixToRemove="AB", return "CD"。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="preffixToRemove"></param>
        /// <returns></returns>
        public static string TrimStart(this string input, string preffixToRemove)
        {
            if (input.IsNullOrEmpty() || preffixToRemove.IsNullOrEmpty())
            {
                return input;
            }

            if (!input.StartsWith(preffixToRemove))
            {
                return input;
            }

            return input.Substring(preffixToRemove.Length, input.Length - preffixToRemove.Length);
        }

        /// <summary>
        /// input ="ABCD",suffixToRemove="CD", return "AB"。 不区分大小写
        /// </summary>
        /// <param name="input"></param>
        /// <param name="suffixToRemove"></param>
        /// <returns></returns>
        public static string TrimEnd(this string input, string suffixToRemove)
        {
            if (input.IsNullOrEmpty() || suffixToRemove.IsNullOrEmpty())
            {
                return input;
            }

            if (!input.EndsWith(suffixToRemove, StringComparison.OrdinalIgnoreCase))
            {
                return input;
            }

            return input.Substring(0, input.Length - suffixToRemove.Length);
        }

        /// <summary>
        /// Trim start && end
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        public static string Trim(this string input, string trimStr)
        {
            if (trimStr.IsNullOrEmpty())
            {
                return input.SafeTrim();
            }
            return input.TrimStart(trimStr).TrimEnd(trimStr);
        }

        /// <summary>
        /// Allows null-safe trimming of string. 安全Trim，允许为null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SafeTrim(this string input)
        {
            if (input == null)
            {
                return null;
            }
            return input.Trim();
        }

        /// <summary>
        /// 安全 Replace，允许为 null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SafeReplace(this string input, string old_Value, string new_Value)
        {
            if (input.IsNullOrWhiteSpace() || old_Value.IsNullOrWhiteSpace())
            {
                return input;
            }

            input = input.Replace(old_Value, new_Value);

            return input;
        }

        #region Remove Last StrSplit（去除尾部多余的分隔符）

        public static string RemoveLastStrSplit(string FormatString, string strSplit)
        {
            if (FormatString.LastIndexOf(strSplit) != -1)
            {
                if (FormatString.EndsWith(strSplit))
                {
                    FormatString = FormatString.Remove(FormatString.Length - strSplit.Length);
                }
            }

            return FormatString;
        }

        public static void RemoveLastStrSplit(this StringBuilder sb, string strSplit)
        {
            string FormatString = sb.ToString();
            if (FormatString.LastIndexOf(strSplit) != -1)
            {
                if (FormatString.EndsWith(strSplit))
                {
                    FormatString = FormatString.Remove(FormatString.Length - strSplit.Length);
                }
                sb.Replace(sb.ToString(), FormatString);
            }
        }

        #endregion

    }
}
