/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using TakymLib.CommandLine;
using TakymLib.Core.Framework.Properties;
using TakymLib.Logging;
using TakymLib.Security;

namespace TakymLib.Core.Framework
{
	/// <summary>
	///  コンソール画面を利用したアプリケーションを実行します。
	/// </summary>
	public class CuiTool : DisposableBase
	{
		private readonly CommandLineConverter? _args;
		private readonly MemoryValidation?     _mv;
		private          bool                  _started;
		private          bool                  _stopped;
		private          string?               _man;

		/// <summary>
		///  コマンド行引数解析時に呼び出される関数を取得または設定します。
		/// </summary>
		public Action<CommandLineConverter, PreParseEventArgs>? OnParseArguments { get; set; }

		/// <summary>
		///  このアプリケーションの起動方法を表すコマンド行引数を格納した配列を取得または設定します。
		/// </summary>
		public string[]? CommandUsages { get; set; }

		/// <summary>
		///  型'<see cref="TakymLib.Core.Framework.CuiTool"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="args">
		///  コマンド行引数を含む文字列配列または<see langword="null"/>です。
		///  コマンド行引数の解析を行わない場合は<see langword="null"/>を指定します。
		/// </param>
		/// <param name="doMemoryValidation">メモリの検証を実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を指定します。</param>
		public CuiTool(string[]? args = null, bool doMemoryValidation = false)
		{
			if (args is not null) {
				_args = new(args);
			}
			if (doMemoryValidation) {
				_mv = MemoryValidation.Start();
			}
		}

		/// <summary>
		///  CUIアプリケーションを開始します。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void Start()
		{
			this.EnterRunLock();
			try {
				if (_started || _stopped) {
					throw new InvalidOperationException(Resources.CuiTool_Start_InvalidOperationException);
				}
				_started = true;
				this.StartCore();
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  開始処理を行います。
		/// </summary>
		protected virtual void StartCore()
		{
			if (_args is not null) {
				_args.PreParse += this._args_PreParse;
				_args.Parse();
			}
			if (_man is null) {
				VersionInfo.Current.Print();
			} else {
				Console.WriteLine(_man);
			}
		}

		private void _args_PreParse(object? sender, PreParseEventArgs e)
		{
			var a = _args!;
			this.OnParseArguments?.Invoke(a, e);
			if (e.SubCommand == "help") {
				var man = new ManualBuilder(a);
				man.BuildFull(this.CommandUsages ?? Array.Empty<string>());
				_man = man.ToString();
			}
		}

		/// <summary>
		///  CUIアプリケーションを終了します。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void Stop()
		{
			this.EnterRunLock();
			try {
				if (!_started) {
					throw new InvalidOperationException(Resources.CuiTool_Stop_InvalidOperationException);
				}
				if (!_stopped) {
					_stopped = true;
					_started = false;
					this.StopCore();
					_mv?.StopImmediately();
				}
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  終了処理を行います。
		/// </summary>
		protected virtual void StopCore() { }

		/// <summary>
		///  CUIアプリケーションを実行します。
		///  例外は標準エラーストリームに出力しログファイルに保存します。
		/// </summary>
		/// <exception cref="System.AggregateException"/>
		public int Run()
		{
			try {
				this.Start();
				this.Stop();
				return 0;
			} catch (Exception e) {
				ErrorReportBuilder.PrintAndLog(e);
				return e.HResult;
			}
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				_mv?.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <inheritdoc/>
		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			if (_mv is not null) {
				await _mv.DisposeAsync();
			}
			await base.DisposeAsyncCore();
		}
	}
}
