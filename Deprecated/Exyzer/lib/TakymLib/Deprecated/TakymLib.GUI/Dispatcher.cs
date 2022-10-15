/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading;
using TakymLib.GUI.Properties;
using TakymLib.Threading.Tasks;

namespace TakymLib.GUI
{
	/// <summary>
	///  <see cref="TakymLib.GUI.Component"/>で使用される<see cref="TakymLib.Threading.Tasks.JoinableThread"/>を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class Dispatcher : JoinableThread
	{
		private readonly Component _owner;

		/// <inheritdoc/>
		public override Thread Thread { get; }

		internal Dispatcher(Component owner)
		{
			owner.EnsureNotNull(nameof(owner));
			_owner = owner;

			this.Thread = Thread.CurrentThread;
		}

		/// <summary>
		///  予約された全ての処理を<see cref="TakymLib.GUI.Dispatcher.Thread"/>で実行します。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void RunScheduledActions()
		{
			this.EnsureThread();
			this.EnterRunLock();
			try {
				while (_owner.Actions.TryDequeue(out var action)) {
					action();
				}
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <inheritdoc/>
		protected override void ScheduleCore(Action action)
		{
			_owner.Actions.Enqueue(action);
		}

		/// <summary>
		///  現在のスレッドが<see cref="TakymLib.GUI.Dispatcher.Thread"/>と等しいか判定し、
		///  等しくない場合は例外を発生させます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void EnsureThread()
		{
			this.EnsureNotDisposed();
			if (this.Thread != Thread.CurrentThread) {
				throw new InvalidOperationException(Resources.Dispatcher_EnsureThread_InvalidOperationException);
			}
		}
	}
}
