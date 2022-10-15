/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace TakymLib.Logging
{
	/// <summary>
	///  <see cref="TakymLib.Logging.ErrorReportBuilder"/>に例外の追加情報を提供します。
	/// </summary>
	public interface ICustomErrorDetailProvider
	{
		/// <summary>
		///  追加情報を可読な翻訳済みの文字列へ変換します。
		/// </summary>
		/// <param name="exception">変換するデータを保持している例外オブジェクトです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		public string GetLocalizedDetail(Exception exception);
	}
}
