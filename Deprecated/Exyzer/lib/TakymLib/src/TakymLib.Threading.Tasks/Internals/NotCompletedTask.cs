/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading;

namespace TakymLib.Threading.Tasks.Internals
{
	internal sealed class NotCompletedTask<TResult> : IAsyncMethodResult<TResult>, IAwaiter<TResult>
	{
		internal static readonly IAsyncMethodResult<TResult> _inst = new NotCompletedTask<TResult>();

		public Exception? Exception               => null;
		public object?    AsyncState              => null;
		public WaitHandle AsyncWaitHandle         => DisposedWaitHandle._inst;
		public bool       CompletedSynchronously  => false;
		public bool       IsCompleted             => false;
		public bool       IsCompletedSuccessfully => false;
		public bool       IsFailed                => false;
		public bool       IsCancelled             => false;

		private NotCompletedTask() { }

		public IAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return this;
		}

		public IAwaiter<TResult> GetAwaiter()
		{
			return this;
		}

		public TResult? GetResult()
		{
			return default;
		}

		public void OnCompleted(Action continuation)
		{
			// do nothing
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			// do nothing
		}

		private sealed class DisposedWaitHandle : WaitHandle
		{
			internal static readonly DisposedWaitHandle _inst = new();

			private DisposedWaitHandle()
			{
				this.Dispose();
			}
		}
	}
}
