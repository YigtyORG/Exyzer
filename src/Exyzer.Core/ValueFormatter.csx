/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

//
// このソースファイルは T4 を使用して自動生成しています。
//

<#@ template language="C#" #>
<#@ assembly name="System.Core" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Text" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ output extension="generated.cs" #>
<#
	var types = new (string name_u, string name_s, string funcName, int bits, string msb, int len_oct, int len_hex, int len_utf8)[] {
		("byte",   "sbyte", "Byte",   8, "0x80",                3,  2, 1),
		("ushort", "short", "Int16", 16, "0x8000",              6,  4, 2),
		("uint",   "int",   "Int32", 32, "0x80000000",         11,  8, 4),
		("ulong",  "long",  "Int64", 64, "0x8000000000000000", 22, 16, 8)
	};
#>
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Exyzer
{
	/// <summary>
	///  値の書式設定を行います。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class ValueFormatter
	{
		private static readonly Encoding _enc = Encoding.UTF8;
<# /****/ for (int i = 0; i < types.Length; ++i) { var type = types[i]; #>

		#region <#= type.bits.ToString("D2") #> ビット型

		/******** <#= type #> ********/

		/// <summary>
		///  指定された符号無し<#= type.bits #>ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号無し<#= type.bits #>ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this <#= type.name_u #> value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?            result,
			                                               IFormatProvider?   provider = null)
		{
			switch (format) {
			case ValueFormat.Binary:      return TryToString_Binary     (value, out result);
			case ValueFormat.Octal:       return TryToString_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryToString_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryToString_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryToString_Utf8Chars  (value, out result);
			default:
				result = null;
				return false;
			}
		}

		/// <summary>
		///  指定された符号付き<#= type.bits #>ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号付き<#= type.bits #>ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this <#= type.name_s #> value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?            result,
			                                               IFormatProvider?   provider = null)
		{
			switch (format) {
			case ValueFormat.Binary:
				return TryToString_Binary(unchecked((<#= type.name_u #>)(value)), out result);
			case ValueFormat.Octal:
				return TryToString_Octal(unchecked((<#= type.name_u #>)(value)), out result);
			case ValueFormat.Decimal:
				if (value >= 0) {
					return TryToString_Decimal(unchecked((<#= type.name_u #>)(+value)), out result, provider);
				} else if (TryToString_Decimal(unchecked((<#= type.name_u #>)(-value)), out result, provider)) {
					result = '-' + result;
					return true;
				}
				return false;
			case ValueFormat.Hexadecimal:
				return TryToString_Hexadecimal(unchecked((<#= type.name_u #>)(value)), out result);
			case ValueFormat.Utf8Chars:
				return TryToString_Utf8Chars(unchecked((<#= type.name_u #>)(value)), out result);
			default:
				result = null;
				return false;
			}
		}

		private static bool TryToString_Binary(<#= type.name_u #> value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[<#= type.bits #>];
			for (int i = 0; i < buf.Length; ++i) {
				buf[i] = unchecked((char)((value & (<#= type.msb #> >> i)) + '0'));
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Octal(<#= type.name_u #> value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[<#= type.len_oct #>];
			for (int i = buf.Length - 1; i >= 0; --i) {
				buf[i] = unchecked((char)((value & 0b111) + '0'));
				value >>= 3;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Decimal(<#= type.name_u #> value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result, IFormatProvider? provider)
		{
			result = value.ToString(provider);
			return true;
		}

		private static bool TryToString_Hexadecimal(<#= type.name_u #> value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[<#= type.len_hex #>];
			for (int i = buf.Length - 1; i >= 0; --i) {
				uint c = unchecked((uint)(value & 0x0F)); // 自動生成処理を簡略化する為に冗長な型変換を行っている。
				buf[i] = unchecked((char)(c +
					((c < 0x0A) ? ('0')
					            : ('A' - 0x0A))
				));
				value >>= 4;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Utf8Chars(<#= type.name_u #> value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<byte> buf = stackalloc byte[<#= type.len_utf8 #>];
<# /****/ if (type.name_u == "byte") { #>
			buf[0] = value;
			result = _enc.GetString(buf);
			return true;
<# /****/ } else { #>
			if (BitConverter.TryWriteBytes(buf, value)) {
				result = _enc.GetString(buf);
				return true;
			} else {
				result = null;
				return false;
			}
<# /****/ } #>
		}

		/// <summary>
		///  指定された文字列を符号無し<#= type.bits #>ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号無し<#= type.bits #>ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToU<#= type.funcName #>(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  <#= type.name_u #> result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary:      return TryTo<#= type.funcName #>_Binary     (value, out result);
			case ValueFormat.Octal:       return TryTo<#= type.funcName #>_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryTo<#= type.funcName #>_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryTo<#= type.funcName #>_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryTo<#= type.funcName #>_Utf8Chars  (value, out result);
			default:
				result = default;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号付き<#= type.bits #>ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号付き<#= type.bits #>ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToS<#= type.funcName #>(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  <#= type.name_s #> result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary when TryTo<#= type.funcName #>_Binary(value, out <#= type.name_u #> r):
				result = unchecked((<#= type.name_s #>)(r));
				return true;
			case ValueFormat.Octal when TryTo<#= type.funcName #>_Octal(value, out <#= type.name_u #> r):
				result = unchecked((<#= type.name_s #>)(r));
				return true;
			case ValueFormat.Decimal:
				if (value[0] == '-') {
					if (TryTo<#= type.funcName #>_Decimal(value.Substring(1), out <#= type.name_u #> r, provider)) {
						result = <#
						if (type.name_s == "sbyte" || type.name_s == "short") {
							#>unchecked((<#= type.name_s #>)(-r))<#
						} else {
							#>-unchecked((<#= type.name_s #>)(r))<#
						}
						#>;
						return true;
					}
				} else if (TryTo<#= type.funcName #>_Decimal(value, out <#= type.name_u #> r, provider)) {
					result = unchecked((<#= type.name_s #>)(r));
					return true;
				}
				result = default;
				return false;
			case ValueFormat.Hexadecimal when TryTo<#= type.funcName #>_Hexadecimal(value, out <#= type.name_u #> r):
				result = unchecked((<#= type.name_s #>)(r));
				return true;
			case ValueFormat.Utf8Chars when TryTo<#= type.funcName #>_Utf8Chars(value, out <#= type.name_u #> r):
				result = unchecked((<#= type.name_s #>)(r));
				return true;
			default:
				result = default;
				return false;
			}
		}

		private static bool TryTo<#= type.funcName #>_Binary(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out <#= type.name_u #> result)
		{
			if (value.Length <= <#= type.bits #>) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 1;
					switch (value[i]) {
					case '0':
						break;
					case '1':
						result |= 1;
						break;
					default:
						result = default;
						return false;
					}
				}
				return true;
			} else {
				result = default;
				return false;
			}
		}

		private static bool TryTo<#= type.funcName #>_Octal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out <#= type.name_u #> result)
		{
			if (value.Length <= <#= type.len_oct #>) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 3;
					char ch = value[i];
					if ('0' <= ch && ch <= '7') {
						result |= unchecked((<#= type.name_u == "ulong" ? "uint" : type.name_u #>)(ch - '0'));
					} else {
						result = default;
						return false;
					}
				}
				return true;
			} else {
				result = default;
				return false;
			}
		}

		private static bool TryTo<#= type.funcName #>_Decimal(
			                                              string             value,
			[MaybeNullWhen(false)][NotNullWhen(true)] out <#= type.name_u #> result,
			                                              IFormatProvider?   provider)
		{
			return <#= type.name_u #>.TryParse(value, NumberStyles.None, provider, out result);
		}

		private static bool TryTo<#= type.funcName #>_Hexadecimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out <#= type.name_u #> result)
		{
			if (value.Length <= <#= type.len_hex #>) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 4;
					char ch = value[i];
					if ('0' <= ch && ch <= '9') {
						result |= unchecked((<#= type.name_u == "ulong" ? "uint" : type.name_u #>)(ch - '0'));
					} else if ('A' <= ch && ch <= 'F') {
						result |= unchecked((<#= type.name_u == "ulong" ? "uint" : type.name_u #>)(ch - 'A' + 0x0A));
					} else if ('a' <= ch && ch <= 'f') {
						result |= unchecked((<#= type.name_u == "ulong" ? "uint" : type.name_u #>)(ch - 'a' + 0x0A));
					} else {
						result = default;
						return false;
					}
				}
				return true;
			} else {
				result = default;
				return false;
			}
		}

		private static bool TryTo<#= type.funcName #>_Utf8Chars(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out <#= type.name_u #> result)
		{
			byte[] bytes = _enc.GetBytes(value);
			if (bytes.Length == <#= type.len_utf8 #>) {
<# /****/ if (type.name_u == "byte") { #>
				result = bytes[0];
<# /****/ } else { #>
				result = BitConverter.ToU<#= type.funcName #>(bytes);
<# /****/ } #>
				return true;
			} else {
				result = default;
				return false;
			}
		}

		#endregion
<# /****/ } #>
	}
}
