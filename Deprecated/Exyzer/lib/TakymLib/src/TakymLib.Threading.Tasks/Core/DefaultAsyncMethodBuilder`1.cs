/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TakymLib.Threading.Tasks.Internals;

namespace TakymLib.Threading.Tasks.Core
{
	/// <summary>
	///  既定の<see cref="TakymLib.Threading.Tasks.IAsyncMethodBuilder{TTask, TResult}"/>の実装を提供します。
	/// </summary>
	[StructLayout(LayoutKind.Auto)]
	public struct DefaultAsyncMethodBuilder<TResult> : IAsyncMethodBuilder<IAsyncMethodResult<TResult>, TResult>
	{
		private DefaultAsyncMethodResult<TResult>? _task;
		private bool                               _will_complete_sync;

		/// <inheritdoc/>
		public IAsyncMethodResult<TResult> Task => _task ?? NotCompletedTask<TResult>._inst;

		/// <inheritdoc/>
		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			stateMachine.EnsureNotNull();
			_task = new();
			stateMachine.MoveNext();
		}

		/// <inheritdoc/>
		public void SetStateMachine(IAsyncStateMachine? stateMachine)
		{
			if (_task is not null) {
				_task.AsyncState = stateMachine;
			}
		}

		/// <inheritdoc/>
		public void SetException(Exception e)
		{
			if (_task is not null) {
				_task.SetException(e, _will_complete_sync);
			}
		}

		/// <inheritdoc/>
		public void SetResult(TResult? result)
		{
			if (_task is not null) {
				_task.SetResult(result, _will_complete_sync);
			}
		}

		/// <inheritdoc/>
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : INotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			awaiter     .EnsureNotNull();
			stateMachine.EnsureNotNull();
			_will_complete_sync = false;
			awaiter.OnCompleted(stateMachine.MoveNext);
		}

		/// <inheritdoc/>
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : ICriticalNotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			awaiter     .EnsureNotNull();
			stateMachine.EnsureNotNull();
			_will_complete_sync = false;
			awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.Tasks.Core.DefaultAsyncMethodBuilder{TResult}"/>を作成します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Core.DefaultAsyncMethodBuilder{TResult}"/>を返します。</returns>
		public static DefaultAsyncMethodBuilder<TResult> Create()
		{
			var result = new DefaultAsyncMethodBuilder<TResult>();
			result._will_complete_sync = true;
			return result;
		}
	}
}
