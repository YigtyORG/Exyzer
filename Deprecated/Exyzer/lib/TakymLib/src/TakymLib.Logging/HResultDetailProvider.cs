/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace TakymLib.Logging
{
	/// <summary>
	///  <see cref="TakymLib.Logging.ErrorReportBuilder"/>にH-RESULT情報を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class HResultDetailProvider : ICustomErrorDetailProvider
	{
		private readonly ConcurrentDictionary<int, string> _messages;

		/// <summary>
		///  型'<see cref="TakymLib.Logging.HResultDetailProvider"/>'の新しいインスタンスを生成します。
		/// </summary>
		public HResultDetailProvider()
		{
			_messages = new();
		}

		/// <summary>
		///  追加情報を可読な翻訳済みの文字列へ変換します。
		/// </summary>
		/// <param name="exception">変換するデータを保持している例外オブジェクトです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		public string GetLocalizedDetail(Exception exception)
		{
			if (exception is null) {
				return string.Empty;
			}
			string result = $"H-RESULT Message: {this.GetMessage(exception.HResult)}";
			if (exception is Win32Exception w32e) {
				return result + Environment.NewLine
					+ $"H-RESULT Message (Win32): {this.GetMessage(w32e.NativeErrorCode)}";
			} else {
				return result;
			}
		}

		/// <summary>
		///  内部キャッシュを削除します。
		/// </summary>
		public void ClearCache()
		{
			_messages.Clear();
		}

		private string GetMessage(int hresult)
		{
			return _messages.GetOrAdd(hresult, hresult => new Win32Exception(hresult).Message);
		}
	}
}
