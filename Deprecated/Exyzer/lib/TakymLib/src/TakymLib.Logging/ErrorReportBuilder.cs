/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using TakymLib.IO;
using TakymLib.Logging.Globalization;
using TakymLib.Logging.Properties;
using TakymLib.Text;

namespace TakymLib.Logging
{
	/// <summary>
	///  例外からエラーレポートを作成します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class ErrorReportBuilder
	{
		private static readonly DefaultErrorDetailProvider _default_detailProvider = new();
		private static readonly HResultDetailProvider      _hresult_detailProvider = new();
		private static readonly PathString                 _default_log_dir        = PathStringPool.Get(AppContext.BaseDirectory, "Logs");

		/// <summary>
		///  前回の<see cref="TakymLib.Logging.ErrorReportBuilder.Create(Exception, ICustomErrorDetailProvider[])"/>
		///  の呼び出しで発生したエラーを取得します。成功した場合は<see langword="null"/>を返します。
		/// </summary>
		public static ErrorReportBuilder? LastCreationError { get; private set; }

		/// <summary>
		///  現在のカルチャに合致する型'<see cref="TakymLib.Logging.ErrorReportBuilder"/>'のインスタンスを生成します。
		/// </summary>
		/// <param name="exception">作成するエラーレポートの例外です。</param>
		/// <returns>新しい翻訳された<see cref="TakymLib.Logging.ErrorReportBuilder"/>のインスタンスです。</returns>
		public static ErrorReportBuilder Create(Exception exception)
		{
			return Create(exception, _default_detailProvider, _hresult_detailProvider);
		}

		/// <summary>
		///  現在のカルチャに合致する型'<see cref="TakymLib.Logging.ErrorReportBuilder"/>'のインスタンスを生成します。
		/// </summary>
		/// <param name="exception">作成するエラーレポートの例外です。</param>
		/// <param name="detailProviders">追加情報を翻訳するオブジェクトの配列です。</param>
		/// <returns>新しい翻訳された<see cref="TakymLib.Logging.ErrorReportBuilder"/>のインスタンスです。</returns>
		public static ErrorReportBuilder Create(Exception exception, params ICustomErrorDetailProvider[] detailProviders)
		{
			LastCreationError = null;
			detailProviders ??= Array.Empty<ICustomErrorDetailProvider>();
			try {
				var t = Type.GetType(Resources.Type, false, true) ?? typeof(EnglishErrorReportBuilder);
				var o = Activator.CreateInstance(t, exception, Resources.Option, detailProviders) as ErrorReportBuilder;
				return o ?? new EnglishErrorReportBuilder(exception, "S", detailProviders);
			} catch (Exception e) {
				LastCreationError = new JapaneseErrorReportBuilder(e, "S", detailProviders);
				return new EnglishErrorReportBuilder(exception, "S", detailProviders);
			}
		}

		/// <summary>
		///  <see cref="TakymLib.Logging.ErrorReportBuilder.LastCreationError"/>を保存します。
		/// </summary>
		/// <param name="dir">ログファイルの保管場所を表すパス文字列です。</param>
		public static void SaveERBC(PathString dir)
		{
			// ERBC = ErrorReportBuilder Creation
			LastCreationError?.Save(dir, "ERBC");
		}

		/// <summary>
		///  指定された例外をコンソール画面に出力しログファイルに保存します。
		/// </summary>
		/// <param name="e">対象の例外オブジェクトです。</param>
		/// <returns>自動生成された保存先のファイルパスです。失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.AggregateException" />
		public static PathString? PrintAndLog(Exception e)
		{
			return PrintAndLog(e, _default_log_dir);
		}

		/// <summary>
		///  指定された例外をコンソール画面に出力し、指定されたディレクトリにログファイルに保存します。
		/// </summary>
		/// <param name="e">対象の例外オブジェクトです。</param>
		/// <param name="dir">ログファイルの保存先のディレクトリです。</param>
		/// <returns>自動生成された保存先のファイルパスです。失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.AggregateException" />
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static PathString? PrintAndLog(Exception e, PathString dir)
		{
			try {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine();
				Console.Error.WriteLine(e.ToString());
				Console.ResetColor();
				if (dir.IsFile) {
					Console.Error.WriteLine();
					Console.Error.WriteLine(Resources.ERB_PrintAndLog_PathToFile, Resources.ERB_PrintAndLog_Failed);
					Console.Error.WriteLine(Resources.ERB_PrintAndLog_Failed_Reason, dir);
					Console.Error.WriteLine();
					return null;
				} else {
					if (!dir.IsDirectory) {
						Directory.CreateDirectory(dir);
					}
					_hresult_detailProvider.ClearCache();
					var log = Create(e).Save(dir, null);
					_hresult_detailProvider.ClearCache();
					SaveERBC(dir);
					Console.Error.WriteLine();
					Console.Error.WriteLine(Resources.ERB_PrintAndLog_PathToFile, log);
					Console.Error.WriteLine();
					return log;
				}
			} catch (Exception e2) {
				throw new AggregateException(Resources.ERB_PrintAndLog_AggregateException, e2, e);
			}
		}

		private string? _text;

		/// <summary>
		///  このオブジェクトが作成された日時を取得します。
		/// </summary>
		public DateTime DateTime { get; }

		/// <summary>
		///  コンストラクタに渡された例外を取得します。
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		///  作成するエラーレポートのオプションを取得します。
		///  この変数は派生クラスで利用されます。
		/// </summary>
		public string? Option { get; }

		/// <summary>
		///  追加情報を翻訳するオブジェクトの列挙体を取得します。
		/// </summary>
		public IEnumerable<ICustomErrorDetailProvider> DetailProviders { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Logging.ErrorReportBuilder"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="exception">作成するエラーレポートの例外オブジェクトです。</param>
		/// <param name="option">
		///  作成するエラーレポートのオプションです。
		///  この引数は派生クラスで利用されます。
		/// </param>
		/// <param name="detailProviders">追加情報を翻訳するオブジェクトの列挙体です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected ErrorReportBuilder(Exception exception, string option, IEnumerable<ICustomErrorDetailProvider> detailProviders)
		{
			exception      .EnsureNotNull();
			detailProviders.EnsureNotNull();
			this.DateTime        = DateTime.Now;
			this.Exception       = exception;
			this.Option          = option;
			this.DetailProviders = detailProviders;
		}

		/// <summary>
		///  エラーレポートを生成し、指定された場所へ保存します。
		/// </summary>
		/// <param name="dir">ログファイルの保管場所を表すパス文字列です。</param>
		/// <param name="name">エラーレポートの名前です。</param>
		/// <returns>自動生成された保存先のファイルパスです。</returns>
		public PathString Save(PathString dir, string? name)
		{
			if (string.IsNullOrEmpty(name)) {
				name = "ErrorReport";
			} else {
				name = "ER_" + name;
			}
			var path = LogFileName.CreatePath(dir, name);
			this.Save(path);
			return path;
		}

		/// <summary>
		///  エラーレポートを生成し、指定されたパスを持つファイルへ保存します。
		/// </summary>
		/// <param name="path">エラーレポートの保存先のファイルパスです。</param>
		public void Save(PathString path)
		{
			using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)) {
				this.Save(fs);
			}
		}

		/// <summary>
		///  エラーレポートを生成し、指定されたストリームへ保存します。
		/// </summary>
		/// <param name="stream">エラーレポートの保存先のストリームです。</param>
		public void Save(Stream stream)
		{
			using (var sw = new StreamWriter(stream, Encoding.UTF8, -1, true)) {
				this.Save(sw);
			}
		}

		/// <summary>
		///  エラーレポートを生成し、指定されたライターへ保存します。
		/// </summary>
		/// <param name="writer">エラーレポートの保存先のライターです。</param>
		public void Save(TextWriter writer)
		{
			if (_text == null) {
				_text = this.Build();
			}
			writer.WriteLine(_text);
		}

		/// <summary>
		///  エラーレポートを生成します。
		/// </summary>
		/// <returns>生成されたエラーレポートを表す文字列です。</returns>
		protected virtual string Build()
		{
			var sb = BuildString.Begin();
			this.BuildHeader(sb, this.DateTime, Environment.ProcessId);
			this.BuildBody(sb, this.Exception);
			sb.AppendLine();
			return BuildString.End(sb);
		}

		/// <summary>
		///  エラーレポートの見出しを作成します。
		/// </summary>
		/// <param name="sb">戻り値を格納するオブジェクトです。</param>
		/// <param name="dt">エラーレポートの作成日時です。</param>
		/// <param name="pid">エラーレポートを作成したプロセスのIDです。</param>
		protected virtual void BuildHeader(StringBuilder sb, DateTime dt, int pid)
		{
			sb.AppendLine();
			sb.AppendLine("************************************************");
			sb.AppendLine();
			sb.AppendLine($"  {this.GetLocalizedHeaderLine1_Caption()}");
			sb.AppendLine();
			sb.AppendLine($"  {this.GetLocalizedHeaderLine2_Created(dt)}");
			sb.AppendLine($"  {this.GetLocalizedHeaderLine3_ProcessId(pid)}");
			sb.AppendLine();
			sb.AppendLine("************************************************");
			sb.AppendLine();
			sb.AppendLine(this.GetLocalizedHeaderLine4_Notice());
			sb.AppendLine();
		}

		/// <summary>
		///  エラーレポートの内容を作成します。
		/// </summary>
		/// <param name="sb">戻り値を格納するオブジェクトです。</param>
		/// <param name="ex">エラーレポートの例外です。</param>
		/// <param name="index">何番目の内部例外かを表す整数値です。<c>0</c>の場合は内部例外ではありません。</param>
		protected virtual void BuildBody(StringBuilder sb, Exception ex, int index = 0)
		{
			if (index == 0) {
				sb.AppendLine("========");
			} else {
				sb.AppendLine($"======== {index,10} >>>");
			}
			sb.AppendLine(this.GetLocalizedBodyLine0_TypeName(ex.GetType().AssemblyQualifiedName));
			sb.AppendLine(this.GetLocalizedBodyLine1_Message(ex.Message));
			sb.AppendLine(this.GetLocalizedBodyLine2_HResult(ex.HResult));
			sb.AppendLine(this.GetLocalizedBodyLine3_HelpLink(ex.HelpLink));
			sb.AppendLine(this.GetLocalizedBodyLine4_Source(ex.Source));
			sb.AppendLine(this.GetLocalizedBodyLine5_TargetSite(ex.TargetSite?.Name, ex.TargetSite?.ReflectedType?.AssemblyQualifiedName));
			string[] stacktrace = ex.StackTrace?.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
			for (int i = 0; i < stacktrace.Length; ++i) {
				sb.AppendLine(this.GetLocalizedBodyLine6_StackTrace(i, stacktrace[i].Trim()));
			}
			if (ex.Data == null) {
				sb.AppendLine(this.GetLocalizedBodyLine7_Data("<null>"));
			} else if (ex.Data.Count == 0) {
				sb.AppendLine(this.GetLocalizedBodyLine7_Data("<empty>"));
			} else {
				sb.AppendLine(this.GetLocalizedBodyLine7_Data(ex.Data.Count.ToString()));
				var dictenum = ex.Data.GetEnumerator();
				dictenum.Reset();
				while (dictenum.MoveNext()) {
					sb.AppendLine($"\t* [\"{dictenum.Key}\"] = {dictenum.Value}");
				}
			}
			sb.AppendLine("--------");
			var exs = new List<Exception>();
			foreach (var provider in this.DetailProviders) {
				sb.AppendLine(provider.GetLocalizedDetail(ex));
				if (provider is IMoreExceptionsProvider provider2) {
					exs.AddRange(provider2.GetMoreExceptions(ex));
				}
			}
			sb.AppendLine("--------");
			sb.AppendLine(ex.ToString());
			int count = exs.Count;
			for (int i = 0; i < count; ++i) {
				sb.AppendLine($"++++++++ {i,10} >>>");
				this.BuildBody(sb, exs[i], 0);
				sb.AppendLine($"++++++++ {i,10} <<<");
				sb.AppendLine();
			}
			if (index != 0) {
				sb.AppendLine($"======== {index,10} <<<");
			}
			if (ex.InnerException != null) {
				if (index == 0) {
					sb.AppendLine("########");
				}
				sb.AppendLine();
				sb.AppendLine(this.GetLocalizedBodyLine8_InnerException());
				this.BuildBody(sb, ex.InnerException, index + 1);
			}
		}

		/// <summary>
		///  上書きされた場合、見出しの1行目(題名)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine1_Caption();

		/// <summary>
		///  上書きされた場合、見出しの2行目(作成日時)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="dt">作成日時です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine2_Created(DateTime dt);

		/// <summary>
		///  上書きされた場合、見出しの3行目(プロセスID)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="pid">プロセスIDです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine3_ProcessId(int pid);

		/// <summary>
		///  上書きされた場合、見出しの4行目(注意文)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine4_Notice();

		/// <summary>
		///  上書きされた場合、内容の0行目(型名)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="typename">例外オブジェクトの型名です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine0_TypeName(string? typename);

		/// <summary>
		///  上書きされた場合、内容の1行目(メッセージ)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="message">メッセージです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine1_Message(string message);

		/// <summary>
		///  上書きされた場合、内容の2行目(H-RESULT)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="hresult">H-RESULTです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine2_HResult(int hresult);

		/// <summary>
		///  上書きされた場合、内容の3行目(ヘルプリンク)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="helplink">ヘルプリンクです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine3_HelpLink(string? helplink);

		/// <summary>
		///  上書きされた場合、内容の4行目(発生源)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="source">発生源です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine4_Source(string? source);

		/// <summary>
		///  上書きされた場合、内容の5行目(発生場所)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="methodName">例外を発生させた関数の名前です。</param>
		/// <param name="className">例外を発生させた型の名前です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine5_TargetSite(string? methodName, string? className);

		/// <summary>
		///  上書きされた場合、内容の6行目(スタックトレース)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="index">スタックフレームの番号です。</param>
		/// <param name="stackframe">スタックフレームを表す文字列です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine6_StackTrace(int index, string stackframe);

		/// <summary>
		///  上書きされた場合、内容の7行目(内部データ)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="content">内部データの個数を表す文字列です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine7_Data(string content);

		/// <summary>
		///  上書きされた場合、内容の8行目(内部例外)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine8_InnerException();
	}
}
