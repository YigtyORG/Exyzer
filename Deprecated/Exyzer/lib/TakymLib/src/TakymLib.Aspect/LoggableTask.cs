/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Logging;
using TakymLib.Threading.Tasks;

namespace TakymLib.Aspect
{
	/// <summary>
	///  非同期関数の処理を分断し、ログ出力可能にします。
	/// </summary>
	[StructLayout(LayoutKind.Auto)]
	[AsyncMethodBuilder(typeof(AsyncLoggableTaskMethodBuilder))]
	public readonly struct LoggableTask : IAwaitable
	{
		private static ICallerLogger? _logger;

		/// <summary>
		///  ログの出力先を取得または設定します。
		/// </summary>
		public static ICallerLogger? Logger
		{
			get => _logger;
			set
			{
				var logger = _logger;
				while (Interlocked.CompareExchange(ref _logger, value, logger) != logger) {
					TaskUtility.YieldAndWait();
					logger = _logger;
				}
			}
		}

		private readonly ValueTask _task;
		private readonly bool      _continue_on_captured_context;

		/// <summary>
		///  この<see cref="TakymLib.Aspect.LoggableTask"/>の内部で利用される<see cref="System.Threading.Tasks.ValueTask"/>を取得します。
		/// </summary>
		public ValueTask Task => _task;

		internal LoggableTask(ValueTask task, bool continueOnCapturedContext = true)
		{
			_task                         = task;
			_continue_on_captured_context = continueOnCapturedContext;
		}

		/// <summary>
		///  この<see cref="TakymLib.Aspect.LoggableTask"/>を構成します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元の実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <returns>構成された待機可能なオブジェクトです。</returns>
		public LoggableTask ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_task, continueOnCapturedContext);
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter"/>を取得します。
		/// </summary>
		/// <returns><see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter"/>を返します。</returns>
		public ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter GetAwaiter()
		{
			return _task.ConfigureAwait(_continue_on_captured_context).GetAwaiter();
		}

		IAwaiter IAwaitable.GetAwaiter()
		{
			return _task.WrapAwaitable().ConfigureAwait(false).GetAwaiter();
		}
	}
}
