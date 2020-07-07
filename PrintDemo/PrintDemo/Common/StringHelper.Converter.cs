using System;

namespace PrintDemo.Common
{
	public static partial class StringHelper
	{

		public static string Safe2Upper(this string str)
		{
			if (str.IsNullOrWhiteSpace())
			{
				return "";
			}

			str = str.ToUpper();

			return str;
		}

		public static string Safe2Lower(this string str)
		{
			if (str.IsNullOrWhiteSpace())
			{
				return "";
			}

			str = str.ToLower();

			return str;
		}

		#region Safely Convert Obj To Str

		/// <summary>
		/// return trimed str
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SafeToString(this object obj)
		{
			string str = string.Empty;
			if (obj == null)
			{
				return str;
			}
			else
			{
				return obj.ToString().Trim();
			}
		}

		public static string Safe2String(this object obj)
		{
			string str = string.Empty;
			if (obj == null)
			{
				return str;
			}
			else
			{
				return obj.ToString().Trim();
			}
		}

		#endregion

		#region Safe To float

		public static Single SafeToFloat(this object obj)
		{
			string input = obj.SafeToString();
			float output = 0;
			if (float.TryParse(input, out output))
			{
				return output;
			}
			return 0;
		}

		#endregion

		#region Safe To Single

		public static Single SafeToSingle(this object obj)
		{
			string input = obj.SafeToString();
			Single output = 0;
			if (Single.TryParse(input, out output))
			{
				return output;
			}
			return 0;
		}

		#endregion

		#region  Safe To Double

		public static double SafeToDouble(this object obj)
		{
			string input = obj.SafeToString();
			double output = 0;
			if (Double.TryParse(input, out output))
			{
				return output;
			}
			return 0;
		}

		#endregion

		#region  Safe To Int

		public static Int32 SafeToInt32(this object obj)
		{
			string input = obj.SafeToString();
			int output = 0;
			if (Int32.TryParse(input, out output))
			{
				return output;
			}
			return 0;
		}

		#endregion

		public static DateTime SafetoDateTime(this object obj)
		{
			string input = obj.SafeToString();
			DateTime output;
			if (DateTime.TryParse(input, out output))
			{
				return output;
			}

			return default(DateTime);
		}


		public static decimal SafetoDecimal(this object obj)
		{
			string input = obj.SafeToString();
			decimal output = 0m;
			if (Decimal.TryParse(input, out output))
			{
				return output;
			}

			return 0m;
		}

		#region bool

		public static string BooltoChinese(bool bl)
		{
			if (bl)
			{
				return "成功";
			}

			return "失败";
		}

		/// <summary>
		/// Y/N true/false 转化为 bool
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool SafeToBool(this object obj)
		{
			bool bl = false;
			string str = obj.SafeToString().Replace(" ", "");
			if (str.ToUpper() == "Y")
			{
				str = "true";
			}
			if (str.ToUpper() == "N")
			{
				str = "false";
			}
			if (Boolean.TryParse(str, out bl))
			{
				return bl;
			}
			return false;
		}

		#endregion



	}
}
