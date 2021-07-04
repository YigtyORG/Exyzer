/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

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

		/// <summary>
		///  指定された符号無し32ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号無し32ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this uint             value,
			                                               ValueFormat      format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?          result,
			                                               IFormatProvider? provider = null)
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
		///  指定された符号付き32ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号付き32ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this int              value,
			                                               ValueFormat      format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?          result,
			                                               IFormatProvider? provider = null)
		{
			switch (format) {
			case ValueFormat.Binary:
				return TryToString_Binary(unchecked((uint)(value)), out result);
			case ValueFormat.Octal:
				return TryToString_Octal(unchecked((uint)(value)), out result);
			case ValueFormat.Decimal:
				if (value >= 0) {
					return TryToString_Decimal(unchecked((uint)(+value)), out result, provider);
				} else if (TryToString_Decimal(unchecked((uint)(-value)), out result, provider)) {
					result = '-' + result;
					return true;
				}
				return false;
			case ValueFormat.Hexadecimal:
				return TryToString_Hexadecimal(unchecked((uint)(value)), out result);
			case ValueFormat.Utf8Chars:
				return TryToString_Utf8Chars(unchecked((uint)(value)), out result);
			default:
				result = null;
				return false;
			}
		}

		private static bool TryToString_Binary(uint value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[32];
			for (int i = 0; i < buf.Length; ++i) {
				buf[i] = unchecked((char)((value & (0x80000000 >> i)) + '0'));
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Octal(uint value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[11];
			for (int i = buf.Length - 1; i >= 0; --i) {
				uint c = value & 0b111;
				buf[i] = unchecked((char)(c + '0'));
				value >>= 3;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Decimal(uint value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result, IFormatProvider? provider)
		{
			result = value.ToString(provider);
			return true;
		}

		private static bool TryToString_Hexadecimal(uint value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[8];
			for (int i = buf.Length - 1; i >= 0; --i) {
				uint c = value & 0x0F;
				if (c < 0x0A) {
					buf[i] = unchecked((char)(c + '0'));
				} else {
					buf[i] = unchecked((char)(c + 'A' - 0x0A));
				}
				value >>= 4;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Utf8Chars(uint value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<byte> buf = stackalloc byte[4];
			if (BitConverter.TryWriteBytes(buf, value)) {
				result = _enc.GetString(buf);
				return true;
			} else {
				result = null;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号無し32ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号無し32ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToUInt32(
			                                          this string           value,
			                                               ValueFormat      format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  uint             result,
			                                               IFormatProvider? provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary:      return TryToUInt32_Binary     (value, out result);
			case ValueFormat.Octal:       return TryToUInt32_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryToUInt32_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryToUInt32_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryToUInt32_Utf8Chars  (value, out result);
			default:
				result = default;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号付き32ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号付き32ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToSInt32(
			                                          this string           value,
			                                               ValueFormat      format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  int              result,
			                                               IFormatProvider? provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary when TryToUInt32_Binary(value, out uint r):
				result = unchecked((int)(r));
				return true;
			case ValueFormat.Octal when TryToUInt32_Octal(value, out uint r):
				result = unchecked((int)(r));
				return true;
			case ValueFormat.Decimal:
				if (value[0] == '-') {
					if (TryToUInt32_Decimal(value.Substring(1), out uint r, provider)) {
						result = -unchecked((int)(r));
						return true;
					}
				} else if (TryToUInt32_Decimal(value, out uint r, provider)) {
					result = unchecked((int)(r));
					return true;
				}
				result = default;
				return false;
			case ValueFormat.Hexadecimal when TryToUInt32_Hexadecimal(value, out uint r):
				result = unchecked((int)(r));
				return true;
			case ValueFormat.Utf8Chars when TryToUInt32_Utf8Chars(value, out uint r):
				result = unchecked((int)(r));
				return true;
			default:
				result = default;
				return false;
			}
		}

		private static bool TryToUInt32_Binary(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
		{
			if (value.Length <= 32) {
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

		private static bool TryToUInt32_Octal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
		{
			if (value.Length <= 11) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 3;
					char ch = value[i];
					if ('0' <= ch && ch <= '7') {
						result |= unchecked((uint)(ch - '0'));
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

		private static bool TryToUInt32_Decimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result, IFormatProvider? provider)
		{
			return uint.TryParse(value, NumberStyles.None, provider, out result);
		}

		private static bool TryToUInt32_Hexadecimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
		{
			if (value.Length <= 8) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 4;
					char ch = value[i];
					if ('0' <= ch && ch <= '9') {
						result |= unchecked((uint)(ch - '0'));
					} else if ('A' <= ch && ch <= 'F') {
						result |= unchecked((uint)(ch - 'A' + 0x0A));
					} else if ('a' <= ch && ch <= 'f') {
						result |= unchecked((uint)(ch - 'a' + 0x0A));
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

		private static bool TryToUInt32_Utf8Chars(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
		{
			byte[] bytes = _enc.GetBytes(value);
			if (bytes.Length == 4) {
				result = BitConverter.ToUInt32(bytes);
				return true;
			} else {
				result = default;
				return false;
			}
		}
	}
}
