/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;

namespace TakymLib.Logging
{
	/// <summary>
	///  呼び出し元の情報をログ出力する機能を提供します。
	/// </summary>
	public interface ICallerLogger
	{
		/// <summary>
		///  処理の開始をログに出力します。
		/// </summary>
		/// <param name="memberName">呼び出し元の名前を指定します。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスを指定します。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号を指定します。</param>
		public void Begin(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1
		);

		/// <summary>
		///  処理の開始をログに出力します。
		/// </summary>
		/// <param name="message">出力するメッセージを指定します。</param>
		/// <param name="memberName">呼び出し元の名前を指定します。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスを指定します。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号を指定します。</param>
		/// <param name="messageExpression">メッセージの式表現を指定します。</param>
		public void Begin(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);

		/// <summary>
		///  処理の開始をログに出力します。
		/// </summary>
		/// <typeparam name="T">メッセージの種類を指定します。</typeparam>
		/// <param name="message">出力するメッセージを指定します。</param>
		/// <param name="memberName">呼び出し元の名前を指定します。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスを指定します。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号を指定します。</param>
		/// <param name="messageExpression">メッセージの式表現を指定します。</param>
		public void Begin<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);

		/// <summary>
		///  処理の終了をログに出力します。
		/// </summary>
		/// <param name="memberName">呼び出し元の名前を指定します。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスを指定します。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号を指定します。</param>
		public void End(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1
		);

		/// <summary>
		///  処理の終了をログに出力します。
		/// </summary>
		/// <param name="message">出力するメッセージを指定します。</param>
		/// <param name="memberName">呼び出し元の名前を指定します。。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスを指定します。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号を指定します。</param>
		/// <param name="messageExpression">メッセージの式表現を指定します。</param>
		public void End(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);

		/// <summary>
		///  処理の終了をログに出力します。
		/// </summary>
		/// <typeparam name="T">メッセージの種類を指定します。</typeparam>
		/// <param name="message">出力するメッセージを指定します。</param>
		/// <param name="memberName">呼び出し元の名前を指定します。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスを指定します。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号を指定します。</param>
		/// <param name="messageExpression">メッセージの式表現を指定します。</param>
		public void End<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);
	}
}
