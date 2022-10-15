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

namespace TakymLib.Threading.Tasks.Internals
{
	internal sealed class JoinableThreadInternal : JoinableThread
	{
		private readonly ConcurrentQueue<Action> _queue;

		public override Thread Thread { get; }

		internal JoinableThreadInternal()
		{
			_queue = new();

			this.Thread = new(this.RunLoop);
			this.Thread.IsBackground = true;
			this.Thread.Start();
		}

		private void RunLoop()
		{
			while (!this.IsDisposed) {
				if (_queue.TryDequeue(out var action)) {
					action();
				}
				TaskUtility.YieldAndWait();
			}
		}

		protected override void ScheduleCore(Action action)
		{
			_queue.Enqueue(action);
		}

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
