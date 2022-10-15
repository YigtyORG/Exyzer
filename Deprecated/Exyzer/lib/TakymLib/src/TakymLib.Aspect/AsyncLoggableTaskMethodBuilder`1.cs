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
using TakymLib.Aspect.Internals;
using TakymLib.Threading.Tasks;

namespace TakymLib.Aspect
{
	/// <summary>
	///  <see cref="TakymLib.Aspect.LoggableTask{TResult}"/>を利用した非同期関数を生成します。
	/// </summary>
	[StructLayout(LayoutKind.Auto)]
	public struct AsyncLoggableTaskMethodBuilder<TResult> : IAsyncMethodBuilder<LoggableTask<TResult>, TResult>
	{
		private AsyncValueTaskMethodBuilder<TResult> _builder;
		private AsyncLoggableTaskMethodBuilderCore   _core;

		/// <summary>
		///  <see cref="TakymLib.Aspect.LoggableTask{TResult}"/>を取得します。
		/// </summary>
		public LoggableTask<TResult> Task
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new(_builder.Task);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Start<TStateMachine>(ref TStateMachine stateMachine)
			where TStateMachine: IAsyncStateMachine
		{
			_core.Start();
			_builder.Start(ref stateMachine);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStateMachine(IAsyncStateMachine? stateMachine)
		{
			_builder.SetStateMachine(stateMachine!);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetException(Exception e)
		{
			_builder.SetException(e);
			_core.Stop();
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetResult(TResult? result)
		{
			_builder.SetResult(result!);
			_core.Stop();
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : INotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			_builder.AwaitOnCompleted(ref awaiter, ref stateMachine);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : ICriticalNotifyCompletion
			where TStateMachine: IAsyncStateMachine
		{
			_builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
		}

		/// <summary>
		///  <see cref="TakymLib.Aspect.AsyncLoggableTaskMethodBuilder{TResult}"/>を作成します。
		/// </summary>
		/// <returns><see cref="TakymLib.Aspect.AsyncLoggableTaskMethodBuilder{TResult}"/>を返します。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AsyncLoggableTaskMethodBuilder<TResult> Create()
		{
			var result = new AsyncLoggableTaskMethodBuilder<TResult>();
			result._core.Init();
			return result;
		}
	}
}
