/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;

namespace TakymLib.Threading.Tasks.Wrappers
{
	/// <summary>
	///  <see cref="System.Runtime.CompilerServices.TaskAwaiter{TResult}"/>を<see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>として扱える様にします。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct TaskAwaiterWrapper<TResult> : IAwaiter<TResult>
	{
		private readonly TaskAwaiter<TResult> _awaiter;

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.TaskAwaiter{TResult}.IsCompleted"/>の値を取得します。
		/// </summary>
		public bool IsCompleted
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _awaiter.IsCompleted;
		}

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.TaskAwaiterWrapper{TResult}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="awaiter">ラップする待機オブジェクトです。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiterWrapper(TaskAwaiter<TResult> awaiter)
		{
			_awaiter = awaiter;
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.TaskAwaiter{TResult}.GetResult"/>を呼び出します。
		/// </summary>
		/// <returns>操作の結果を表すオブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResult GetResult()
		{
			return _awaiter.GetResult();
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.TaskAwaiter{TResult}.OnCompleted(Action)"/>を呼び出します。
		/// </summary>
		/// <param name="continuation">完了時に実行する操作です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OnCompleted(Action continuation)
		{
			_awaiter.OnCompleted(continuation);
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.TaskAwaiter{TResult}.UnsafeOnCompleted(Action)"/>を呼び出します。
		/// </summary>
		/// <param name="continuation">完了時に実行する操作です。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UnsafeOnCompleted(Action continuation)
		{
			_awaiter.UnsafeOnCompleted(continuation);
		}
	}
}
