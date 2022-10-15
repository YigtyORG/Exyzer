/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace TakymLib.Logging
{
	/// <summary>
	///  <see cref="TakymLib.Logging.ErrorReportBuilder"/>に追加の例外を提供します。
	///  このインターフェースは<see cref="TakymLib.Logging.ICustomErrorDetailProvider"/>から派生しています。
	/// </summary>
	public interface IMoreExceptionsProvider : ICustomErrorDetailProvider
	{
		/// <summary>
		///  追加の例外を取得します。
		/// </summary>
		/// <param name="exception">追加の例外の取得元です。</param>
		/// <returns>追加の例外を含む<see cref="System.Collections.Generic.IEnumerable{T}"/>オブジェクトです。</returns>
		public IEnumerable<Exception> GetMoreExceptions(Exception exception);
	}
}
