using System;

namespace PrintDemo.Common
{
    public static partial class StringHelper
    {

        #region IsNullOrWhiteSpace: Extension

        /// <summary>
        /// String Extension: IsNullOrWhiteSpace; simplify coding input
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string strInput)
        {
            return string.IsNullOrWhiteSpace(strInput);
        }

        #endregion


        public static bool IsNullOrEmpty(this string s)
        {
            if (s == null)
            {
                return true;
            }
            return s.Trim().Length == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="InVariant_Case">=true,不区分大小写；=false，区分大小写</param>
        /// <returns></returns>
        public static bool ContainsEx(this string str, string value, bool InVariant_Case = true)
        {
            if (InVariant_Case)
            {
                return str.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1;
            }

            return str.Contains(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="InVariant_Case">=true,不区分大小写；=false，区分大小写</param>
        /// <returns></returns>
        public static bool ContainsEx(this string str, char value, bool InVariant_Case = true)
        {
            return str.ContainsEx(value.SafeToString(), InVariant_Case);
        }

        /// <summary>
        /// Determines whether the specified eval string contains only numeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// <c>true</c> if the string is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string evalString)
        {
            return evalString.Trim("0123456789".ToCharArray()) == string.Empty;
        }

    }
}
