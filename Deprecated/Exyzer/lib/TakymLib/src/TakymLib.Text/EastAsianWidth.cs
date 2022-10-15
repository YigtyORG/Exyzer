/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using TakymLib.Text.Properties;

namespace TakymLib.Text
{
	/// <summary>
	///  東アジアの文字幅を扱います。
	///  このクラスは抽象クラスです。
	/// </summary>
	/// <remarks>
	///  詳しくは<see href="https://www.unicode.org/reports/tr11/"/>をご覧ください。
	/// </remarks>
	public abstract partial class EastAsianWidth
	{
		/// <summary>
		///  上書きされた場合、指定された文字の文字幅を判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <returns><see cref="TakymLib.Text.EastAsianWidthType"/>列挙値を返します。</returns>
		public abstract EastAsianWidthType GetCharType(char c);

		/// <summary>
		///  指定された文字が半角文字かどうか判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <param name="useHalfWidthForAmbiguous">
		///  <see cref="TakymLib.Text.EastAsianWidthType.Ambiguous"/>を半角として扱う場合は<see langword="true"/>を指定します。
		///  既定値は<see langword="false"/>です。
		/// </param>
		/// <param name="useHalfWidthForNeutral">
		///  <see cref="TakymLib.Text.EastAsianWidthType.Neutral"/>を半角として扱う場合は<see langword="true"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <returns>半角文字である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.InvalidOperationException"/>
		public virtual bool IsHalfWidth(char c, bool useHalfWidthForAmbiguous = false, bool useHalfWidthForNeutral = true)
		{
			var t = this.GetCharType(c);
			return t switch {
				EastAsianWidthType.Ambiguous => useHalfWidthForAmbiguous,
				EastAsianWidthType.FullWidth => false,
				EastAsianWidthType.HalfWidth => true,
				EastAsianWidthType.Neutral   => useHalfWidthForNeutral,
				EastAsianWidthType.Narrow    => true,
				EastAsianWidthType.Wide      => false,
				_ => throw new InvalidOperationException(string.Format(Resources.EastAsianWidth_IsHalfWidth_InvalidOperationException, c))
			};
		}

		/// <summary>
		///  指定された文字が半角文字かどうか判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <param name="culture">
		///  <see cref="TakymLib.Text.EastAsianWidthType.Ambiguous"/>の扱いを決定する為に利用されるカルチャ情報です。
		/// </param>
		/// <returns>半角文字である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public bool IsHalfWidth(char c, CultureInfo culture)
		{
			culture.EnsureNotNull();
			string locale;
			do {
				locale = culture.Name;
			} while (!string.IsNullOrEmpty((culture = culture.Parent).Name));
			switch (locale) {
			case "ja":
			case "zh":
			case "ko":
				return this.IsHalfWidth(c, false, true);
			default:
				return this.IsHalfWidth(c, true, true);
			}
		}

		/// <summary>
		///  指定された文字が半角文字かどうか判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <returns>半角文字である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.InvalidOperationException"/>
		public static bool IsHalfWidth(char c)
		{
			return Default.IsHalfWidth(c, CultureInfo.CurrentCulture);
		}

		/// <summary>
		///  指定された文字が全角文字かどうか判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <param name="useFullWidthForAmbiguous">
		///  <see cref="TakymLib.Text.EastAsianWidthType.Ambiguous"/>を全角として扱う場合は<see langword="true"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <param name="useFullWidthForNeutral">
		///  <see cref="TakymLib.Text.EastAsianWidthType.Neutral"/>を全角として扱う場合は<see langword="true"/>を指定します。
		///  既定値は<see langword="false"/>です。
		/// </param>
		/// <returns>全角文字である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.InvalidOperationException"/>
		public bool IsFullWidth(char c, bool useFullWidthForAmbiguous = true, bool useFullWidthForNeutral = false)
		{
			return !this.IsHalfWidth(c, !useFullWidthForAmbiguous, !useFullWidthForNeutral);
		}

		/// <summary>
		///  指定された文字が全角文字かどうか判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <param name="culture">
		///  <see cref="TakymLib.Text.EastAsianWidthType.Ambiguous"/>の扱いを決定する為に利用されるカルチャ情報です。
		/// </param>
		/// <returns>全角文字である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public bool IsFullWidth(char c, CultureInfo culture)
		{
			return !this.IsHalfWidth(c, culture);
		}

		/// <summary>
		///  指定された文字が全角文字かどうか判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <returns>全角文字である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.InvalidOperationException"/>
		public static bool IsFullWidth(char c)
		{
			return Default.IsFullWidth(c, CultureInfo.CurrentCulture);
		}
	}
}
