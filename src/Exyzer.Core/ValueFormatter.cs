/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Exyzer.Properties;

namespace Exyzer
{
	/// <summary>
	///  数値を文字列に変換または逆変換します。
	/// </summary>
	public static class ValueFormatter
	{
		public static string ToFormattedString<T>(this T value, ValueFormat format)
			where T: struct
		{
			return format switch {
				ValueFormat.DisplayName => Resources.ValueFormatter_ToFormattedString_Unknown,
				ValueFormat.Binary      => ToFormattedStringCore_Bin(value),
				ValueFormat.Octal       => ToFormattedStringCore_Oct(value),
				ValueFormat.Decimal     => ToFormattedStringCore_Dec(value),
				ValueFormat.Hexadecimal => ToFormattedStringCore_Hex(value),
				ValueFormat.Utf8Chars   => throw new NotImplementedException(),
				_ => value.ToString() ?? string.Empty
			};
		}

		private static string ToFormattedStringCore_Bin<T>(this T value)
			where T : struct
		{
			Span<byte> span = stackalloc byte[Unsafe.SizeOf<T>()];
			Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);

			Span<char> result = stackalloc char[span.Length * 8];

			for (int i = span.Length - 1; i >= 0; --i) {
				byte b = span[i];
				result[0] = (b & 0b10000000) == 0 ? '0' : '1';
				result[1] = (b & 0b01000000) == 0 ? '0' : '1';
				result[2] = (b & 0b00100000) == 0 ? '0' : '1';
				result[3] = (b & 0b00010000) == 0 ? '0' : '1';
				result[4] = (b & 0b00001000) == 0 ? '0' : '1';
				result[5] = (b & 0b00000100) == 0 ? '0' : '1';
				result[6] = (b & 0b00000010) == 0 ? '0' : '1';
				result[7] = (b & 0b00000001) == 0 ? '0' : '1';
				result = result[8..^0];
			}

			return new(result);
		}

		private static string ToFormattedStringCore_Oct<T>(this T value)
			where T : struct
		{
			throw new NotImplementedException();
		}

		private static string ToFormattedStringCore_Dec<T>(this T value)
			where T : struct
		{
			throw new NotImplementedException();
		}

		private static string ToFormattedStringCore_Hex<T>(this T value)
			where T : struct
		{
			Span<byte> span = stackalloc byte[Unsafe.SizeOf<T>()];
			Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);

			Span<char> result = stackalloc char[span.Length * 2];

			for (int i = span.Length - 1; i >= 0; --i) {
				byte b = span[i];
				result[0] = GetChar(b >> 4);
				result[1] = GetChar(b & 0x0F);
				result = result[2..^0];
			}

			return new(result);

			static char GetChar(int b)
			{
				return ((char)(b < 10 ? '0' + b : 'A' + b - 10));
			}
		}
	}
}
