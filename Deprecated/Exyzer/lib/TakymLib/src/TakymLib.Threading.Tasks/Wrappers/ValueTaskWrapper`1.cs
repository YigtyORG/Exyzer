/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Tasks.Wrappers
{
	/// <summary>
	///  <see cref="System.Threading.Tasks.ValueTask{TResult}"/>を<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult{TResult}"/>として扱える様にします。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct ValueTaskWrapper<TResult> : IAsyncMethodResult<TResult>
	{
		private readonly ValueTask<TResult> _task;

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		[Obsolete("現在は ValueTaskWrapper<TResult> ではサポートされていません。", DiagnosticId = "TakymLib_ValueTaskWrapper")]
		public object? AsyncState
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => throw new NotSupportedException();
		}

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		[Obsolete("現在は ValueTaskWrapper<TResult> ではサポートされていません。", DiagnosticId = "TakymLib_ValueTaskWrapper")]
		public WaitHandle AsyncWaitHandle
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => throw new NotSupportedException();
		}

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		[Obsolete("現在は ValueTaskWrapper<TResult> ではサポートされていません。", DiagnosticId = "TakymLib_ValueTaskWrapper")]
		public Exception? Exception
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => throw new NotSupportedException();
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsCompleted"/>の値を取得します。
		/// </summary>
		public bool IsCompleted
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCompleted;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsCompletedSuccessfully"/>の値を取得します。
		/// </summary>
		public bool IsCompletedSuccessfully
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCompletedSuccessfully;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsFaulted"/>の値を取得します。
		/// </summary>
		public bool IsFailed
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsFaulted;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.IsCanceled"/>の値を取得します。
		/// </summary>
		public bool IsCancelled
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _task.IsCanceled;
		}

		/// <summary>
		///  サポートされていません。常に例外が発生します。
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		[Obsolete("現在は ValueTaskWrapper<TResult> ではサポートされていません。", DiagnosticId = "TakymLib_ValueTaskWrapper")]
		public bool CompletedSynchronously
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => throw new NotSupportedException();
		}

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.ValueTaskWrapper{TResult}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTaskWrapper(ValueTask<TResult> task)
		{
			task.EnsureNotNull();
			_task = task;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.GetAwaiter"/>を呼び出します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.ValueTaskAwaiterWrapper{TResult}"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTaskAwaiterWrapper<TResult> GetAwaiter()
		{
			return new(_task.GetAwaiter());
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.ValueTask{TResult}.ConfigureAwait(bool)"/>を呼び出します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元の実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredValueTaskAwaitableWrapper{TResult}"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredValueTaskAwaitableWrapper<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_task.ConfigureAwait(continueOnCapturedContext));
		}

		IAwaiter<TResult> IAwaitable<TResult>.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		IAwaitable<TResult> IAsyncMethodResult<TResult>.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
	}
}
