/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using TakymLib.Properties;
using TakymLib.Text;

namespace TakymLib
{
	/// <summary>
	///  文字列の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		///  指定された文字列を論理値への変換を試行します。
		///  <see langword="true"/>、<see langword="false"/>以外の一部の単語にも対応しています。
		/// </summary>
		/// <param name="s">変換する文字列です。</param>
		/// <param name="result">変換結果を格納する変数です。</param>
		/// <returns>
		///  指定された文字列が有効な論理値を表す場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public static bool TryToBoolean(this string s, out bool result)
		{
			string text = (s ?? string.Empty).ToLower().Trim();
			switch (text) {
			case "true":
			case "yes":
			case "ot":
			case "on":
			case "enable":
			case "enabled":
			case "allow":
			case "high":
			case "pos":
			case "positive":
			case "one":
			case "1":
			case "t":
			case "y":
			case "+":
				result = true;
				return true;
			case "false":
			case "no":
			case "not":
			case "off":
			case "disable":
			case "disabled":
			case "deny":
			case "low":
			case "neg":
			case "negative":
			case "zero":
			case "0":
			case "f":
			case "n":
			case "-":
				result = false;
				return true;
			default:
				result = false;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を論理値へ変換します。
		///  <see langword="true"/>、<see langword="false"/>以外の一部の単語にも対応しています。
		/// </summary>
		/// <param name="s">変換する文字列です。</param>
		/// <returns>変換結果の論理値です。</returns>
		/// <exception cref="System.FormatException"/>
		public static bool ToBoolean(this string s)
		{
			if (s.TryToBoolean(out bool result)) {
				return result;
			} else {
				throw new FormatException(string.Format(Resources.StringExtensions_ToBoolean, s));
			}
		}

		/// <summary>
		///  指定された文字列を指定された文字数に省略します。
		/// </summary>
		/// <remarks>
		///  <paramref name="s"/>の文字数が<paramref name="count"/>より大きい場合は、
		///  先頭の<c><paramref name="count"/>-3</c>文字に三つのピリオドを付加して省略後の文字数に合わせます。
		///  <paramref name="s"/>の文字数が<paramref name="count"/>より小さい場合は、
		///  <see cref="string.PadRight(int)"/>を利用して省略後の文字数に合わせます。
		///  <paramref name="s"/>の文字数が<paramref name="count"/>と同じ場合はそのまま返します。
		/// </remarks>
		/// <param name="s">省略する文字列です。</param>
		/// <param name="count">省略後の文字数です。4文字以上でなければなりません。</param>
		/// <returns>
		///  指定された文字数に収まる文字列です。
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public static string Abridge(this string? s, int count)
		{
			count.EnsureNotNullWithinClosedRange(4, int.MaxValue);
			s ??= string.Empty;
			if (s.Length > count) {
				return s.Remove(count - 3) + "...";
			} else if (s.Length == count) {
				return s;
			} else {
				return s.PadRight(count);
			}
		}

		/// <summary>
		///  指定された文字列を1行に収めます。
		/// </summary>
		/// <remarks>
		///  この拡張関数は推奨されていません。
		///  代わりに<see cref="TakymLib.StringExtensions.RemoveControlChars(string, bool)"/>を利用してください。
		/// </remarks>
		/// <param name="s">1行に収める必要のある文字列です。</param>
		/// <returns>改行やタブが削除され、1行で表現された文字列です。</returns>
		[Obsolete("代わりに RemoveControlChars を利用してください。", DiagnosticId = "TakymLib_FitToLine")]
		public static string FitToLine(this string s)
		{
			return s.Replace("\r", "[CR]").Replace("\n", "[LF]").Replace("\t", "[TB]").Replace("　", "[SP]");
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する文字列です。</param>
		/// <param name="useMarks">
		///  制御文字を記号へ変換する場合は<see langword="true"/>、
		///  完全に削除する場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static string RemoveControlChars(this string s, bool useMarks = true)
		{
			return s.RemoveControlChars(useMarks ? ControlCharsReplaceMode.ConvertToText : ControlCharsReplaceMode.RemoveAll);
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する文字列です。</param>
		/// <param name="mode">制御文字の削除または変換の方法を指定します。</param>
		/// <param name="removeSpace">
		///  空白文字を制御文字として扱う場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tabIsSpace">
		///  タブ文字を空白文字として扱う場合は<see langword="true"/>、
		///  制御文字として扱う場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="useAltName">
		///  一部の制御文字で別名を利用する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  このオプションは<paramref name="mode"/>が<see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>の時にのみ有効です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static string RemoveControlChars(this string s, ControlCharsReplaceMode mode, bool removeSpace = false, bool tabIsSpace = true, bool useAltName = false)
		{
			s ??= string.Empty;
			if (s.Length == 0) {
				return s;
			} else {
				return new(s.AsSpan().RemoveControlChars(mode, removeSpace, tabIsSpace, useAltName));
			}
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する文字配列です。</param>
		/// <param name="mode">制御文字の削除または変換の方法を指定します。</param>
		/// <param name="removeSpace">
		///  空白文字を制御文字として扱う場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tabIsSpace">
		///  タブ文字を空白文字として扱う場合は<see langword="true"/>、
		///  制御文字として扱う場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="useAltName">
		///  一部の制御文字で別名を利用する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  このオプションは<paramref name="mode"/>が<see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>の時にのみ有効です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static char[] RemoveControlChars(
			this char[] s,
			ControlCharsReplaceMode mode,
			bool removeSpace = false,
			bool tabIsSpace = true,
			bool useAltName = false)
		{
			return ((ReadOnlySpan<char>)(s.AsSpan())).RemoveControlChars(mode, removeSpace, tabIsSpace, useAltName).ToArray();
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する文字メモリです。</param>
		/// <param name="mode">制御文字の削除または変換の方法を指定します。</param>
		/// <param name="removeSpace">
		///  空白文字を制御文字として扱う場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tabIsSpace">
		///  タブ文字を空白文字として扱う場合は<see langword="true"/>、
		///  制御文字として扱う場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="useAltName">
		///  一部の制御文字で別名を利用する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  このオプションは<paramref name="mode"/>が<see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>の時にのみ有効です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static ReadOnlyMemory<char> RemoveControlChars(
			this Memory<char> s,
			ControlCharsReplaceMode mode,
			bool removeSpace = false,
			bool tabIsSpace = true,
			bool useAltName = false)
		{
			return new(((ReadOnlySpan<char>)(s.Span)).RemoveControlChars(mode, removeSpace, tabIsSpace, useAltName).ToArray());
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する読み取り専用文字メモリです。</param>
		/// <param name="mode">制御文字の削除または変換の方法を指定します。</param>
		/// <param name="removeSpace">
		///  空白文字を制御文字として扱う場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tabIsSpace">
		///  タブ文字を空白文字として扱う場合は<see langword="true"/>、
		///  制御文字として扱う場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="useAltName">
		///  一部の制御文字で別名を利用する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  このオプションは<paramref name="mode"/>が<see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>の時にのみ有効です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static ReadOnlyMemory<char> RemoveControlChars(
			this ReadOnlyMemory<char> s,
			ControlCharsReplaceMode mode,
			bool removeSpace = false,
			bool tabIsSpace = true,
			bool useAltName = false)
		{
			return new(s.Span.RemoveControlChars(mode, removeSpace, tabIsSpace, useAltName).ToArray());
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する文字スパンです。</param>
		/// <param name="mode">制御文字の削除または変換の方法を指定します。</param>
		/// <param name="removeSpace">
		///  空白文字を制御文字として扱う場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tabIsSpace">
		///  タブ文字を空白文字として扱う場合は<see langword="true"/>、
		///  制御文字として扱う場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="useAltName">
		///  一部の制御文字で別名を利用する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  このオプションは<paramref name="mode"/>が<see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>の時にのみ有効です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static ReadOnlySpan<char> RemoveControlChars(
			this Span<char>         s,
			ControlCharsReplaceMode mode,
			bool                    removeSpace = false,
			bool                    tabIsSpace  = true,
			bool                    useAltName  = false)
		{
			return ((ReadOnlySpan<char>)(s)).RemoveControlChars(mode, removeSpace, tabIsSpace, useAltName);
		}

		/// <summary>
		///  指定された文字列から制御文字を削除します。
		/// </summary>
		/// <param name="s">制御文字を削除する読み取り専用文字スパンです。</param>
		/// <param name="mode">制御文字の削除または変換の方法を指定します。</param>
		/// <param name="removeSpace">
		///  空白文字を制御文字として扱う場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tabIsSpace">
		///  タブ文字を空白文字として扱う場合は<see langword="true"/>、
		///  制御文字として扱う場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="useAltName">
		///  一部の制御文字で別名を利用する場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  このオプションは<paramref name="mode"/>が<see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>の時にのみ有効です。
		/// </param>
		/// <returns>制御文字が削除された文字列です。</returns>
		public static ReadOnlySpan<char> RemoveControlChars(
			this ReadOnlySpan<char> s,
			ControlCharsReplaceMode mode,
			bool                    removeSpace = false,
			bool                    tabIsSpace  = true,
			bool                    useAltName  = false)
		{
			if (s.Length == 0) {
				return ReadOnlySpan<char>.Empty;
			} else {
				var sb = BuildString.Begin();
				for (int i = 0; i < s.Length; ++i) {
					char c = s[i];
					switch (mode) {
					case ControlCharsReplaceMode.ConvertToText:
						sb.Append(c switch {
							'\0'   => "[NUL]",
							'\x01' => "[SOH]",
							'\x02' => "[STX]",
							'\x03' => "[ETX]",
							'\x04' => "[EOT]",
							'\x05' => "[ENQ]",
							'\x06' => "[ACK]",
							'\a'   => "[BEL]",
							'\b'   => "[BS]",
							'\n'   => "[LF]",
							'\v'   => "[VT]",
							'\f'   => "[FF]",
							'\r'   => "[CR]",
							'\x0E' => "[SO]",
							'\x0F' => "[SI]",
							'\x10' => "[DLE]",
							'\x11' => "[DC1]",
							'\x12' => "[DC2]",
							'\x13' => "[DC3]",
							'\x14' => "[DC4]",
							'\x15' => "[NAK]",
							'\x16' => "[SYN]",
							'\x17' => "[ETB]",
							'\x18' => "[CAN]",
							'\x19' => "[EM]",
							'\x1A' => "[SUB]",
							'\x1B' => "[ESC]", // '\e'
							'\x1C' => "[FS]",
							'\x1D' => "[GS]",
							'\x1E' => "[RS]",
							'\x1F' => "[US]",
							'\x7F' => "[DEL]",
							'\x87' => "[CUS]",
							'\x88' => "[NSB]",
							'\x89' => "[NSE]",
							'\x8A' => "[FIL]",
							'\x8B' => useAltName ? "[TCI]" : "[PLD]",
							'\x8C' => useAltName ? "[ICI]" : "[PLU]",
							'\x8D' => useAltName ? "[OSC]" : "[ZWJ]",
							'\x8E' => useAltName ? "[SS2]" : "[ZWNJ]",
							'\x8F' => "[SS3]",
							'\x91' => "[EAB]",
							'\x92' => "[EAE]",
							'\x93' => "[ISB]",
							'\x94' => "[ISE]",
							'\x95' => "[SIB]",
							'\x96' => "[SIE]",
							'\x97' => "[SSB]",
							'\x98' => "[SSE]",
							'\x99' => "[INC]",
							'\x9C' => "[KWB]",
							'\x9D' => "[KWE]",
							'\x9E' => "[PSB]",
							'\x9F' => "[PSE]",
							(>= '\x80' and <= '\x86') or '\x90' or '\x9A' or '\x9B' => $"[<control-{((ushort)(c)):X04}>]",
							'\t'   => tabIsSpace ? (removeSpace ? "[TAB]" : "\t") : "[HT]",
							' '    => removeSpace ? "[SPh]" : " ",
							'　'   => removeSpace ? "[SPw]" : "　",
							_      => c.ToString()
						});;
						break;
					case ControlCharsReplaceMode.ConvertToIcon:
						sb.Append(c switch {
							'\0'   => '\u2400',
							'\x01' => '\u2401',
							'\x02' => '\u2402',
							'\x03' => '\u2403',
							'\x04' => '\u2404',
							'\x05' => '\u2405',
							'\x06' => '\u2406',
							'\a'   => '\u2407',
							'\b'   => '\u2408',
							'\n'   => '\u240A',
							'\v'   => '\u240B',
							'\f'   => '\u240C',
							'\r'   => '\u240D',
							'\x0E' => '\u240E',
							'\x0F' => '\u240F',
							'\x10' => '\u2410',
							'\x11' => '\u2411',
							'\x12' => '\u2412',
							'\x13' => '\u2413',
							'\x14' => '\u2414',
							'\x15' => '\u2415',
							'\x16' => '\u2416',
							'\x17' => '\u2417',
							'\x18' => '\u2418',
							'\x19' => '\u2419',
							'\x1A' => '\u241A',
							'\x1B' => '\u241B', // '\e'
							'\x1C' => '\u241C',
							'\x1D' => '\u241D',
							'\x1E' => '\u241E',
							'\x1F' => '\u241F',
							'\x7F' => '\u2421',
							(>= '\x80' and <= '\x9F') => '\u2425',
							'\t'   => tabIsSpace ? (removeSpace ? '\u2423' : '\t') : '\u2409',
							' '    => removeSpace ? '\u2420' : ' ',
							'　'   => removeSpace ? '\u2420' : '　',
							_      => c
						});
						break;
					default:
						if (c > 0x1F && c != 0x7F && (c < 0x80 || c > 0x9F) &&
							(!removeSpace || (c != ' ' && c != '　' && (!tabIsSpace || (c != '\t'))))) {
							sb.Append(c);
						}
						break;
					}
				}
				return BuildString.End(sb).AsSpan();
			}
		}
	}
}
