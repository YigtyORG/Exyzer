/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Globalization;

namespace TakymLib
{
	/// <summary>
	///  言語に関する便利関数を提供します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class LanguageUtility
	{
		/// <summary>
		///  指定した言語コードからカルチャ情報を取得します。
		/// </summary>
		/// <param name="langCode">取得するカルチャ情報の言語コードを指定します。</param>
		/// <returns><see cref="System.Globalization.CultureInfo"/>オブジェクトを返します。</returns>
		public static CultureInfo GetCulture(string? langCode)
		{
			return langCode is null ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(langCode);
		}

		/// <summary>
		///  指定したロケールを既定の言語に設定します。
		/// </summary>
		/// <param name="locale">設定する言語の文字列で表されたロケール(言語コード)を指定します。</param>
		/// <returns>新しい既定の言語を表す<see cref="System.Globalization.CultureInfo"/>オブジェクトを返します。</returns>
		public static CultureInfo SetCulture(string? locale)
		{
			var cinfo = GetCulture(locale);
			SetCulture(cinfo);
			return cinfo;
		}

		/// <summary>
		///  指定したロケールを既定の言語に設定します。
		/// </summary>
		/// <param name="locale">設定する言語のロケールを表すオブジェクト(<see cref="System.Globalization.CultureInfo"/>)を指定します。</param>
		public static void SetCulture(CultureInfo locale)
		{
			CultureInfo.DefaultThreadCurrentCulture   = locale;
			CultureInfo.DefaultThreadCurrentUICulture = locale;
			CultureInfo.CurrentCulture                = locale;
			CultureInfo.CurrentUICulture              = locale;
		}
	}
}
