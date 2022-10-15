/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace TakymLib.Threading.Tasks.Internals
{
	internal sealed class DefaultAsyncMethodResult<TResult> : IAsyncMethodResult<TResult>, IAwaiter<TResult>
	{
		private Exception? _exception;
		private TResult?   _result;
		private Action     _continuation;
		public  object?    AsyncState             { get; internal set; }
		public  WaitHandle AsyncWaitHandle        { get; }
		public  Exception? Exception              => _exception;
		public  bool       CompletedSynchronously { get; private set; }
		public  bool       IsCompleted            { get; private set; }

		internal DefaultAsyncMethodResult()
		{
			_continuation        = () => { };
			this.AsyncWaitHandle = new DefaultWaitHandle();
		}

		public IAwaiter<TResult> GetAwaiter()
		{
			return this;
		}

		public TResult? GetResult()
		{
			while (!this.IsCompleted) {
				TaskUtility.YieldAndWait();
			}
			var exception = this.Exception;
			if (exception is not null) {
				ExceptionDispatchInfo.Capture(exception).Throw();
			}
			return _result;
		}

		internal void SetException(Exception e, bool completedSynchronously)
		{
			Exception? e1, e2;
			LoadException();
			while (Interlocked.CompareExchange(ref _exception, e2, e1) != e1) {
				TaskUtility.YieldAndWait();
				LoadException();
			}
			this.CompleteCore(completedSynchronously);

			void LoadException()
			{
				e1 = _exception;
				e2 = e1 switch {
					null                  => e,
					AggregateException ae => new AggregateException(ae.InnerExceptions.Append(e)),
					_                     => new AggregateException(e1, e)
				};
			}
		}

		internal void SetResult(TResult? result, bool completedSynchronously)
		{
			_result = result;
			this.CompleteCore(completedSynchronously);
		}

		private void CompleteCore(bool completedSynchronously)
		{
			this.CompletedSynchronously = completedSynchronously;
			this.IsCompleted            = true;
			_continuation();
		}

		public void OnCompleted(Action continuation)
		{
			var c1 = _continuation;
			while (Interlocked.CompareExchange(ref _continuation, c1 + continuation, c1) != c1) {
				TaskUtility.YieldAndWait();
				c1 = _continuation;
			}
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			this.OnCompleted(continuation);
		}

		public IAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return this;
		}

		private sealed class DefaultWaitHandle : WaitHandle { }
	}
}
