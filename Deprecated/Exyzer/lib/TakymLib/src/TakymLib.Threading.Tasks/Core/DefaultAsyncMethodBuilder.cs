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
	///  既定の<see cref="TakymLib.Threading.Tasks.IAsyncMethodBuilder{TTask}"/>の実装を提供します。
	/// </summary>
	[StructLayout(LayoutKind.Auto)]
	public struct DefaultAsyncMethodBuilder : IAsyncMethodBuilder<IAsyncMethodResult>
	{
		private DefaultAsyncMethodResult<VoidResult>? _task;
		private bool                                  _will_complete_sync;

		/// <inheritdoc/>
		public IAsyncMethodResult Task => _task ?? NotCompletedTask<VoidResult>._inst;

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
		public void SetResult()
		{
			if (_task is not null) {
				_task.SetResult(default, _will_complete_sync);
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
		///  <see cref="TakymLib.Threading.Tasks.Core.DefaultAsyncMethodBuilder"/>を作成します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Core.DefaultAsyncMethodBuilder"/>を返します。</returns>
		public static DefaultAsyncMethodBuilder Create()
		{
			var result = new DefaultAsyncMethodBuilder();
			result._will_complete_sync = true;
			return result;
		}
	}
}
