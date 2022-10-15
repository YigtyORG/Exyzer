/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace TakymLib.Logging.Globalization
{
	/// <summary>
	///  例外からエラーレポートを日本語で作成します。
	/// </summary>
	public class JapaneseErrorReportBuilder : ErrorReportBuilder
	{
		/// <summary>
		///  型'<see cref="TakymLib.Logging.Globalization.JapaneseErrorReportBuilder"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="exception">作成するエラーレポートの例外オブジェクトです。</param>
		/// <param name="option">
		///  作成するエラーレポートのオプションです。
		///  <c>S</c>は短い形式、<c>L</c>は長い形式を表します。
		/// </param>
		/// <param name="detailProviders">追加情報を翻訳するオブジェクトの列挙体です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public JapaneseErrorReportBuilder(Exception exception, string option, IEnumerable<ICustomErrorDetailProvider> detailProviders)
			: base(exception, option, detailProviders) { }

		/// <summary>
		///  見出しの1行目(題名)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedHeaderLine1_Caption()
		{
			return "例外報告書";
		}

		/// <summary>
		///  見出しの2行目(作成日時)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="dt">作成日時です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedHeaderLine2_Created(DateTime dt)
		{
			switch (this.Option) {
			case "S":
				return $"作成日時：{dt:yyyy/MM/dd HH:mm:ss.fffffff}";
			case "L":
				return $"作成日時：{dt:ggyyyy年MM月dd日(dddd)tthh時mm分ss.fffffff秒}";
			default:
				return $"作成日時：{dt:yyyy年MM月dd日 HH時mm分ss.fffffff秒}";
			}
		}

		/// <summary>
		///  見出しの3行目(プロセスID)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="pid">プロセスIDです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedHeaderLine3_ProcessId(int pid)
		{
			return $"プロセスID：{pid}";
		}

		/// <summary>
		///  見出しの4行目(注意文)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedHeaderLine4_Notice()
		{
			return "※エラーが発生し解決しない場合はこのファイルを開発者へ送信してください。"
				+ Environment.NewLine + "※このファイルには個人情報が含まれている可能性があります。";
		}

		/// <summary>
		///  内容の0行目(型名)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="typename">例外オブジェクトの型名です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine0_TypeName(string? typename)
		{
			return $"例外型          ：{typename}";
		}

		/// <summary>
		///  内容の1行目(メッセージ)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="message">メッセージです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine1_Message(string message)
		{
			return $"メッセージ      ：{message}";
		}

		/// <summary>
		///  内容の2行目(H-RESULT)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="hresult">H-RESULTです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine2_HResult(int hresult)
		{
			return $"H-RESULT        ：0x{hresult:X08} ({hresult})";
		}

		/// <summary>
		///  内容の3行目(ヘルプリンク)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="helplink">ヘルプリンクです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine3_HelpLink(string? helplink)
		{
			return $"ヘルプリンク    ：{helplink}";
		}

		/// <summary>
		///  内容の4行目(発生源)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="source">発生源です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine4_Source(string? source)
		{
			return $"発生源          ：{source}";
		}

		/// <summary>
		///  内容の5行目(発生場所)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="methodName">例外を発生させた関数の名前です。</param>
		/// <param name="className">例外を発生させた型の名前です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine5_TargetSite(string? methodName, string? className)
		{
			if (string.IsNullOrEmpty(methodName) && string.IsNullOrEmpty(className)) {
				return "発生場所        ：";
			} else if (string.IsNullOrEmpty(methodName)) {
				if (this.Option == "S") {
					return $"発生場所        ：[T] {className}";
				} else {
					return $"発生場所        ：型「{className}」";
				}
			} else if (string.IsNullOrEmpty(className)) {
				if (this.Option == "S") {
					return $"発生場所        ：[M] {methodName}";
				} else {
					return $"発生場所        ：関数「{methodName}」";
				}
			} else {
				if (this.Option == "S") {
					return $"発生場所        ：{className}.{methodName}";
				} else {
					return $"発生場所        ：型「{className}」内の関数「{methodName}」";
				}
			}
		}

		/// <summary>
		///  内容の6行目(スタックトレース)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="index">スタックフレームの番号です。</param>
		/// <param name="stackframe">スタックフレームを表す文字列です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine6_StackTrace(int index, string stackframe)
		{
			return $"スタックトレース：【フレーム番号：{index}】{stackframe}";
		}

		/// <summary>
		///  内容の7行目(内部データ)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="content">内部データの個数を表す文字列です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine7_Data(string content)
		{
			if (this.Option == "S") {
				return $"データ          ：{content}";
			} else if (content == "<null>") {
				return "データ          ：空";
			} else if (content == "<empty>") {
				return "データ          ：0個";
			} else {
				return $"データ          ：{content}個";
			}
		}

		/// <summary>
		///  内容の8行目(内部例外)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected override string GetLocalizedBodyLine8_InnerException()
		{
			return "内部例外：";
		}
	}
}
