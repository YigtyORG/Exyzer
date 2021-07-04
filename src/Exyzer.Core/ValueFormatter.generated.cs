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

		#region 08 ビット型

		/******** (byte, sbyte, Byte, 8, 0x80, 3, 2, 1) ********/

		/// <summary>
		///  指定された符号無し8ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号無し8ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this byte value,
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
		///  指定された符号付き8ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号付き8ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this sbyte value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?            result,
			                                               IFormatProvider?   provider = null)
		{
			switch (format) {
			case ValueFormat.Binary:
				return TryToString_Binary(unchecked((byte)(value)), out result);
			case ValueFormat.Octal:
				return TryToString_Octal(unchecked((byte)(value)), out result);
			case ValueFormat.Decimal:
				if (value >= 0) {
					return TryToString_Decimal(unchecked((byte)(+value)), out result, provider);
				} else if (TryToString_Decimal(unchecked((byte)(-value)), out result, provider)) {
					result = '-' + result;
					return true;
				}
				return false;
			case ValueFormat.Hexadecimal:
				return TryToString_Hexadecimal(unchecked((byte)(value)), out result);
			case ValueFormat.Utf8Chars:
				return TryToString_Utf8Chars(unchecked((byte)(value)), out result);
			default:
				result = null;
				return false;
			}
		}

		private static bool TryToString_Binary(byte value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[8];
			for (int i = 0; i < buf.Length; ++i) {
				buf[i] = unchecked((char)(((value & (0x80 >> i)) == 0) ? '0' : '1'));
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Octal(byte value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[3];
			for (int i = buf.Length - 1; i >= 0; --i) {
				buf[i] = unchecked((char)((value & 0b111) + '0'));
				value >>= 3;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Decimal(byte value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result, IFormatProvider? provider)
		{
			result = value.ToString(provider);
			return true;
		}

		private static bool TryToString_Hexadecimal(byte value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[2];
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

		private static bool TryToString_Utf8Chars(byte value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<byte> buf = stackalloc byte[1];
			buf[0] = value;
			result = _enc.GetString(buf);
			return true;
		}

		/// <summary>
		///  指定された文字列を符号無し8ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号無し8ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToUByte(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  byte result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary:      return TryToByte_Binary     (value, out result);
			case ValueFormat.Octal:       return TryToByte_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryToByte_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryToByte_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryToByte_Utf8Chars  (value, out result);
			default:
				result = default;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号付き8ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号付き8ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToSByte(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  sbyte result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary when TryToByte_Binary(value, out byte r):
				result = unchecked((sbyte)(r));
				return true;
			case ValueFormat.Octal when TryToByte_Octal(value, out byte r):
				result = unchecked((sbyte)(r));
				return true;
			case ValueFormat.Decimal:
				if (value[0] == '-') {
					if (TryToByte_Decimal(value.Substring(1), out byte r, provider)) {
						result = unchecked((sbyte)(-r));
						return true;
					}
				} else if (TryToByte_Decimal(value, out byte r, provider)) {
					result = unchecked((sbyte)(r));
					return true;
				}
				result = default;
				return false;
			case ValueFormat.Hexadecimal when TryToByte_Hexadecimal(value, out byte r):
				result = unchecked((sbyte)(r));
				return true;
			case ValueFormat.Utf8Chars when TryToByte_Utf8Chars(value, out byte r):
				result = unchecked((sbyte)(r));
				return true;
			default:
				result = default;
				return false;
			}
		}

		private static bool TryToByte_Binary(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out byte result)
		{
			if (value.Length <= 8) {
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

		private static bool TryToByte_Octal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out byte result)
		{
			if (value.Length <= 3) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 3;
					char ch = value[i];
					if ('0' <= ch && ch <= '7') {
						result |= unchecked((byte)(ch - '0'));
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

		private static bool TryToByte_Decimal(
			                                              string             value,
			[MaybeNullWhen(false)][NotNullWhen(true)] out byte result,
			                                              IFormatProvider?   provider)
		{
			return byte.TryParse(value, NumberStyles.None, provider, out result);
		}

		private static bool TryToByte_Hexadecimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out byte result)
		{
			if (value.Length <= 2) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 4;
					char ch = value[i];
					if ('0' <= ch && ch <= '9') {
						result |= unchecked((byte)(ch - '0'));
					} else if ('A' <= ch && ch <= 'F') {
						result |= unchecked((byte)(ch - 'A' + 0x0A));
					} else if ('a' <= ch && ch <= 'f') {
						result |= unchecked((byte)(ch - 'a' + 0x0A));
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

		private static bool TryToByte_Utf8Chars(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out byte result)
		{
			byte[] bytes = _enc.GetBytes(value);
			if (bytes.Length == 1) {
				result = bytes[0];
				return true;
			} else {
				result = default;
				return false;
			}
		}

		#endregion

		#region 16 ビット型

		/******** (ushort, short, Int16, 16, 0x8000, 6, 4, 2) ********/

		/// <summary>
		///  指定された符号無し16ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号無し16ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this ushort value,
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
		///  指定された符号付き16ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号付き16ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this short value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?            result,
			                                               IFormatProvider?   provider = null)
		{
			switch (format) {
			case ValueFormat.Binary:
				return TryToString_Binary(unchecked((ushort)(value)), out result);
			case ValueFormat.Octal:
				return TryToString_Octal(unchecked((ushort)(value)), out result);
			case ValueFormat.Decimal:
				if (value >= 0) {
					return TryToString_Decimal(unchecked((ushort)(+value)), out result, provider);
				} else if (TryToString_Decimal(unchecked((ushort)(-value)), out result, provider)) {
					result = '-' + result;
					return true;
				}
				return false;
			case ValueFormat.Hexadecimal:
				return TryToString_Hexadecimal(unchecked((ushort)(value)), out result);
			case ValueFormat.Utf8Chars:
				return TryToString_Utf8Chars(unchecked((ushort)(value)), out result);
			default:
				result = null;
				return false;
			}
		}

		private static bool TryToString_Binary(ushort value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[16];
			for (int i = 0; i < buf.Length; ++i) {
				buf[i] = unchecked((char)(((value & (0x8000 >> i)) == 0) ? '0' : '1'));
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Octal(ushort value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[6];
			for (int i = buf.Length - 1; i >= 0; --i) {
				buf[i] = unchecked((char)((value & 0b111) + '0'));
				value >>= 3;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Decimal(ushort value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result, IFormatProvider? provider)
		{
			result = value.ToString(provider);
			return true;
		}

		private static bool TryToString_Hexadecimal(ushort value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[4];
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

		private static bool TryToString_Utf8Chars(ushort value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<byte> buf = stackalloc byte[2];
			if (BitConverter.TryWriteBytes(buf, value)) {
				result = _enc.GetString(buf);
				return true;
			} else {
				result = null;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号無し16ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号無し16ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToUInt16(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  ushort result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary:      return TryToInt16_Binary     (value, out result);
			case ValueFormat.Octal:       return TryToInt16_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryToInt16_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryToInt16_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryToInt16_Utf8Chars  (value, out result);
			default:
				result = default;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号付き16ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号付き16ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToSInt16(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  short result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary when TryToInt16_Binary(value, out ushort r):
				result = unchecked((short)(r));
				return true;
			case ValueFormat.Octal when TryToInt16_Octal(value, out ushort r):
				result = unchecked((short)(r));
				return true;
			case ValueFormat.Decimal:
				if (value[0] == '-') {
					if (TryToInt16_Decimal(value.Substring(1), out ushort r, provider)) {
						result = unchecked((short)(-r));
						return true;
					}
				} else if (TryToInt16_Decimal(value, out ushort r, provider)) {
					result = unchecked((short)(r));
					return true;
				}
				result = default;
				return false;
			case ValueFormat.Hexadecimal when TryToInt16_Hexadecimal(value, out ushort r):
				result = unchecked((short)(r));
				return true;
			case ValueFormat.Utf8Chars when TryToInt16_Utf8Chars(value, out ushort r):
				result = unchecked((short)(r));
				return true;
			default:
				result = default;
				return false;
			}
		}

		private static bool TryToInt16_Binary(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ushort result)
		{
			if (value.Length <= 16) {
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

		private static bool TryToInt16_Octal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ushort result)
		{
			if (value.Length <= 6) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 3;
					char ch = value[i];
					if ('0' <= ch && ch <= '7') {
						result |= unchecked((ushort)(ch - '0'));
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

		private static bool TryToInt16_Decimal(
			                                              string             value,
			[MaybeNullWhen(false)][NotNullWhen(true)] out ushort result,
			                                              IFormatProvider?   provider)
		{
			return ushort.TryParse(value, NumberStyles.None, provider, out result);
		}

		private static bool TryToInt16_Hexadecimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ushort result)
		{
			if (value.Length <= 4) {
				result = 0;
				for (int i = 0; i < value.Length; ++i) {
					result <<= 4;
					char ch = value[i];
					if ('0' <= ch && ch <= '9') {
						result |= unchecked((ushort)(ch - '0'));
					} else if ('A' <= ch && ch <= 'F') {
						result |= unchecked((ushort)(ch - 'A' + 0x0A));
					} else if ('a' <= ch && ch <= 'f') {
						result |= unchecked((ushort)(ch - 'a' + 0x0A));
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

		private static bool TryToInt16_Utf8Chars(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ushort result)
		{
			byte[] bytes = _enc.GetBytes(value);
			if (bytes.Length == 2) {
				result = BitConverter.ToUInt16(bytes);
				return true;
			} else {
				result = default;
				return false;
			}
		}

		#endregion

		#region 32 ビット型

		/******** (uint, int, Int32, 32, 0x80000000, 11, 8, 4) ********/

		/// <summary>
		///  指定された符号無し32ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号無し32ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this uint value,
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
		///  指定された符号付き32ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号付き32ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this int value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?            result,
			                                               IFormatProvider?   provider = null)
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
				buf[i] = unchecked((char)(((value & (0x80000000 >> i)) == 0) ? '0' : '1'));
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Octal(uint value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[11];
			for (int i = buf.Length - 1; i >= 0; --i) {
				buf[i] = unchecked((char)((value & 0b111) + '0'));
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
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  uint result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary:      return TryToInt32_Binary     (value, out result);
			case ValueFormat.Octal:       return TryToInt32_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryToInt32_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryToInt32_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryToInt32_Utf8Chars  (value, out result);
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
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  int result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary when TryToInt32_Binary(value, out uint r):
				result = unchecked((int)(r));
				return true;
			case ValueFormat.Octal when TryToInt32_Octal(value, out uint r):
				result = unchecked((int)(r));
				return true;
			case ValueFormat.Decimal:
				if (value[0] == '-') {
					if (TryToInt32_Decimal(value.Substring(1), out uint r, provider)) {
						result = -unchecked((int)(r));
						return true;
					}
				} else if (TryToInt32_Decimal(value, out uint r, provider)) {
					result = unchecked((int)(r));
					return true;
				}
				result = default;
				return false;
			case ValueFormat.Hexadecimal when TryToInt32_Hexadecimal(value, out uint r):
				result = unchecked((int)(r));
				return true;
			case ValueFormat.Utf8Chars when TryToInt32_Utf8Chars(value, out uint r):
				result = unchecked((int)(r));
				return true;
			default:
				result = default;
				return false;
			}
		}

		private static bool TryToInt32_Binary(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
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

		private static bool TryToInt32_Octal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
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

		private static bool TryToInt32_Decimal(
			                                              string             value,
			[MaybeNullWhen(false)][NotNullWhen(true)] out uint result,
			                                              IFormatProvider?   provider)
		{
			return uint.TryParse(value, NumberStyles.None, provider, out result);
		}

		private static bool TryToInt32_Hexadecimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
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

		private static bool TryToInt32_Utf8Chars(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out uint result)
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

		#endregion

		#region 64 ビット型

		/******** (ulong, long, Int64, 64, 0x8000000000000000, 22, 16, 8) ********/

		/// <summary>
		///  指定された符号無し64ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号無し64ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this ulong value,
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
		///  指定された符号付き64ビット整数値を文字列に変換します。
		/// </summary>
		/// <param name="value">変換する符号付き64ビット整数値を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の文字列を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToString(
			                                          this long value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  string?            result,
			                                               IFormatProvider?   provider = null)
		{
			switch (format) {
			case ValueFormat.Binary:
				return TryToString_Binary(unchecked((ulong)(value)), out result);
			case ValueFormat.Octal:
				return TryToString_Octal(unchecked((ulong)(value)), out result);
			case ValueFormat.Decimal:
				if (value >= 0) {
					return TryToString_Decimal(unchecked((ulong)(+value)), out result, provider);
				} else if (TryToString_Decimal(unchecked((ulong)(-value)), out result, provider)) {
					result = '-' + result;
					return true;
				}
				return false;
			case ValueFormat.Hexadecimal:
				return TryToString_Hexadecimal(unchecked((ulong)(value)), out result);
			case ValueFormat.Utf8Chars:
				return TryToString_Utf8Chars(unchecked((ulong)(value)), out result);
			default:
				result = null;
				return false;
			}
		}

		private static bool TryToString_Binary(ulong value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[64];
			for (int i = 0; i < buf.Length; ++i) {
				buf[i] = unchecked((char)(((value & (0x8000000000000000 >> i)) == 0) ? '0' : '1'));
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Octal(ulong value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[22];
			for (int i = buf.Length - 1; i >= 0; --i) {
				buf[i] = unchecked((char)((value & 0b111) + '0'));
				value >>= 3;
			}
			result = buf.ToString();
			return true;
		}

		private static bool TryToString_Decimal(ulong value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result, IFormatProvider? provider)
		{
			result = value.ToString(provider);
			return true;
		}

		private static bool TryToString_Hexadecimal(ulong value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<char> buf = stackalloc char[16];
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

		private static bool TryToString_Utf8Chars(ulong value, [MaybeNullWhen(false)][NotNullWhen(true)] out string? result)
		{
			Span<byte> buf = stackalloc byte[8];
			if (BitConverter.TryWriteBytes(buf, value)) {
				result = _enc.GetString(buf);
				return true;
			} else {
				result = null;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号無し64ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号無し64ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToUInt64(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  ulong result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary:      return TryToInt64_Binary     (value, out result);
			case ValueFormat.Octal:       return TryToInt64_Octal      (value, out result);
			case ValueFormat.Decimal:     return TryToInt64_Decimal    (value, out result, provider);
			case ValueFormat.Hexadecimal: return TryToInt64_Hexadecimal(value, out result);
			case ValueFormat.Utf8Chars:   return TryToInt64_Utf8Chars  (value, out result);
			default:
				result = default;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を符号付き64ビット整数値に変換します。
		/// </summary>
		/// <param name="value">変換する文字列を指定します。</param>
		/// <param name="format">数値の書式を指定します。</param>
		/// <param name="result">変換結果の符号付き64ビット整数値を返します。</param>
		/// <param name="provider">書式設定を制御するオブジェクトを指定します。既定値は<see langword="null"/>です。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public static bool TryToSInt64(
			                                          this string             value,
			                                               ValueFormat        format,
			[MaybeNullWhen(false)][NotNullWhen(true)] out  long result,
			                                               IFormatProvider?   provider = null)
		{
			if (string.IsNullOrEmpty(value)) {
				result = default;
				return false;
			}
			switch (format) {
			case ValueFormat.Binary when TryToInt64_Binary(value, out ulong r):
				result = unchecked((long)(r));
				return true;
			case ValueFormat.Octal when TryToInt64_Octal(value, out ulong r):
				result = unchecked((long)(r));
				return true;
			case ValueFormat.Decimal:
				if (value[0] == '-') {
					if (TryToInt64_Decimal(value.Substring(1), out ulong r, provider)) {
						result = -unchecked((long)(r));
						return true;
					}
				} else if (TryToInt64_Decimal(value, out ulong r, provider)) {
					result = unchecked((long)(r));
					return true;
				}
				result = default;
				return false;
			case ValueFormat.Hexadecimal when TryToInt64_Hexadecimal(value, out ulong r):
				result = unchecked((long)(r));
				return true;
			case ValueFormat.Utf8Chars when TryToInt64_Utf8Chars(value, out ulong r):
				result = unchecked((long)(r));
				return true;
			default:
				result = default;
				return false;
			}
		}

		private static bool TryToInt64_Binary(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ulong result)
		{
			if (value.Length <= 64) {
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

		private static bool TryToInt64_Octal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ulong result)
		{
			if (value.Length <= 22) {
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

		private static bool TryToInt64_Decimal(
			                                              string             value,
			[MaybeNullWhen(false)][NotNullWhen(true)] out ulong result,
			                                              IFormatProvider?   provider)
		{
			return ulong.TryParse(value, NumberStyles.None, provider, out result);
		}

		private static bool TryToInt64_Hexadecimal(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ulong result)
		{
			if (value.Length <= 16) {
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

		private static bool TryToInt64_Utf8Chars(string value, [MaybeNullWhen(false)][NotNullWhen(true)] out ulong result)
		{
			byte[] bytes = _enc.GetBytes(value);
			if (bytes.Length == 8) {
				result = BitConverter.ToUInt64(bytes);
				return true;
			} else {
				result = default;
				return false;
			}
		}

		#endregion
	}
}
