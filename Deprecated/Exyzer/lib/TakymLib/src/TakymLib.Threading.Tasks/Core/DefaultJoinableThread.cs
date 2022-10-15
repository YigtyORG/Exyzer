/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.Threading;
using TakymLib.Threading.Tasks.Properties;

namespace TakymLib.Threading.Tasks.Core
{
	/// <summary>
	///  既定の<see cref="TakymLib.Threading.Tasks.JoinableThread"/>の実装を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class DefaultJoinableThread : JoinableThread
	{
		private readonly ConcurrentQueue<Action> _queue;

		/// <inheritdoc/>
		public override Thread Thread { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Core.DefaultJoinableThread"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DefaultJoinableThread()
		{
			_queue = new();

			this.Thread = Thread.CurrentThread;
		}

		/// <summary>
		///  スケジュールされた処理を実行し続けます。
		/// </summary>
		/// <remarks>
		///  現在のインスタンスを生成したスレッドでのみ実行できます。
		/// </remarks>
		/// <param name="cancellationToken">
		///  処理の中断を制御するトークンを指定します。
		///  この引数は省略可能です。
		/// </param>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void RunLoop(CancellationToken cancellationToken = default)
		{
			this.EnterRunLock();
			try {
				this.ValidateThread();
				while (!cancellationToken.IsCancellationRequested) {
					this.RunNextCore();
					TaskUtility.YieldAndWait();
				}
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  スケジュールされた次の処理を実行します。
		/// </summary>
		/// <remarks>
		///  現在のインスタンスを生成したスレッドでのみ実行できます。
		/// </remarks>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void RunNext()
		{
			this.EnterRunLock();
			try {
				this.ValidateThread();
				this.RunNextCore();
			} finally {
				this.LeaveRunLock();
			}
		}

		private void ValidateThread()
		{
			if (this.Thread.ManagedThreadId != Environment.CurrentManagedThreadId) {
				throw new InvalidOperationException(Resources.DefaultJoinableThread_ValidateThread_InvalidOperationException);
			}
		}

		private void RunNextCore()
		{
			if (_queue.TryDequeue(out var action)) {
				action();
			}
		}

		/// <inheritdoc/>
		protected override void ScheduleCore(Action action)
		{
			_queue.Enqueue(action);
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			_queue.Clear();
			base.Dispose(disposing);
		}
	}
}
