/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TakymLib.Logging
{
	/// <summary>
	///  呼び出し元の情報を出力ストリームに書き込む機能を提供します。
	/// </summary>
	public class TextWriterCallerLogger : DisposableBase, ICallerLogger
	{
		private static   TextWriterCallerLogger? _stdout;
		private readonly TextWriter              _tw;

		/// <summary>
		///  標準出力ストリームに出力を行うロガーを取得します。
		/// </summary>
		public static TextWriterCallerLogger StandardOutput
		{
			get
			{
				if (_stdout is null || _stdout.IsDisposing || _stdout.IsDisposed) {
					_stdout = new(Console.Out);
				}
				return _stdout;
			}
		}

		/// <inheritdoc/>
		protected sealed override bool CanClearDisposed => false;

		/// <summary>
		///  型'<see cref="TakymLib.Logging.TextWriterCallerLogger"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="tw">出力先のライターです。</param>
		public TextWriterCallerLogger(TextWriter tw)
		{
			tw.EnsureNotNull();
			_tw = tw;
		}

		/// <inheritdoc/>
		/// <exception cref="System.ObjectDisposedException"/>
		public virtual void Begin(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1)
		{
			this.EnsureNotDisposed();
			_tw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}] begin {memberName} `{filePath}({lineNumber})`");
		}

		/// <inheritdoc/>
		/// <exception cref="System.ObjectDisposedException"/>
		public virtual void Begin(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = "")
		{
			this.EnsureNotDisposed();
			_tw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}] begin {memberName}; {messageExpression} `{filePath}({lineNumber})`");
		}

		/// <inheritdoc/>
		/// <exception cref="System.ObjectDisposedException"/>
		public virtual void Begin<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = "")
		{
			this.EnsureNotDisposed();
			_tw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}] begin {memberName}; {messageExpression} `{filePath}({lineNumber})`");
		}

		/// <inheritdoc/>
		/// <exception cref="System.ObjectDisposedException"/>
		public virtual void End(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1)
		{
			this.EnsureNotDisposed();
			_tw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}] end {memberName} `{filePath}({lineNumber})`");
		}

		/// <inheritdoc/>
		/// <exception cref="System.ObjectDisposedException"/>
		public virtual void End(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = "")
		{
			this.EnsureNotDisposed();
			_tw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}] end {memberName}; {messageExpression} `{filePath}({lineNumber})`");
		}

		/// <inheritdoc/>
		/// <exception cref="System.ObjectDisposedException"/>
		public virtual void End<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = "")
		{
			this.EnsureNotDisposed();
			_tw.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fffffff}] end {memberName}; {messageExpression} `{filePath}({lineNumber})`");
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				_tw.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <inheritdoc/>
		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			await _tw.ConfigureAwait(false).DisposeAsync();
			await base.DisposeAsyncCore();
		}

		/// <inheritdoc/>
		protected sealed override void EnsureNotDisposed()
		{
			base.EnsureNotDisposed();
		}
	}
}
