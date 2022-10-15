/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace TakymLib
{
	/// <summary>
	///  文字列をより便利に扱う為の機能を提供します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class StringUtility
	{
		private const           string _chars = "!#$0aAbBcCdDeEfF%&'gGhH1234()-=IiJjKkLlMmNnOoPp^~@[{]}5678qrstuv;+QRSTUV9WXYZwxyz,._";
		private static readonly Random _rnd   = new();

		/// <summary>
		///  英数字と記号をランダムに並べた8～64文字の文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText()
		{
			return GetRandomText(8, 64);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  文字数は指定された範囲から自動的に決定します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="min">最小の文字数です。</param>
		/// <param name="max">最大の文字数です。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int min, int max)
		{
			return GetRandomText(min, max, _rnd);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="len">生成される文字列の長さです。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int len)
		{
			return GetRandomText(len, _rnd);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  文字数は指定された範囲から自動的に決定します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="min">最小の文字数です。</param>
		/// <param name="max">最大の文字数です。</param>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int min, int max, Random random)
		{
			return GetRandomText(random.Next(min, max), random);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="length">生成される文字列の長さです。</param>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int length, Random random)
		{
			string result = string.Empty;
			for (int i = 0; i < length; ++i) {
				result += _chars[random.Next(_chars.Length)];
			}
			return result;
		}
	}
}
