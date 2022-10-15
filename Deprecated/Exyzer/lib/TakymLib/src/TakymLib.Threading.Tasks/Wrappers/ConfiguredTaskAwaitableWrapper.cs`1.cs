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
	///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}"/>を
	///  <see cref="TakymLib.Threading.Tasks.IAwaitable{TResult}"/>として扱える様にします。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct ConfiguredTaskAwaitableWrapper<TResult> : IAwaitable<TResult>
	{
		private readonly ConfiguredTaskAwaitable<TResult> _awaitable;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredTaskAwaitableWrapper{TResult}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="awaitable">ラップする待機可能なオブジェクトです。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredTaskAwaitableWrapper(ConfiguredTaskAwaitable<TResult> awaitable)
		{
			_awaitable = awaitable;
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.GetAwaiter"/>を呼び出します。
		/// </summary>
		/// <returns>
		///  <see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredTaskAwaitableWrapper{TResult}.ConfiguredTaskAwaiterWrapper"/>オブジェクトです。
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredTaskAwaiterWrapper GetAwaiter()
		{
			return new(_awaitable.GetAwaiter());
		}

		IAwaiter<TResult> IAwaitable<TResult>.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter"/>を
		///  <see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>として扱える様にします。
		///  このクラスは読み取り専用構造体です。
		/// </summary>
		public readonly struct ConfiguredTaskAwaiterWrapper : IAwaiter<TResult>
		{
			private readonly ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter _awaiter;

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter.IsCompleted"/>の値を取得します。
			/// </summary>
			public bool IsCompleted
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => _awaiter.IsCompleted;
			}

			/// <summary>
			///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredTaskAwaitableWrapper{TResult}.ConfiguredTaskAwaiterWrapper"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="awaiter">ラップする待機オブジェクトです。</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ConfiguredTaskAwaiterWrapper(ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter awaiter)
			{
				_awaiter = awaiter;
			}

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter.GetResult"/>を呼び出します。
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public TResult GetResult()
			{
				return _awaiter.GetResult();
			}

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter.OnCompleted(Action)"/>を呼び出します。
			/// </summary>
			/// <param name="continuation">完了時に実行する操作です。</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void OnCompleted(Action continuation)
			{
				_awaiter.OnCompleted(continuation);
			}

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter.UnsafeOnCompleted(Action)"/>を呼び出します。
			/// </summary>
			/// <param name="continuation">完了時に実行する操作です。</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UnsafeOnCompleted(Action continuation)
			{
				_awaiter.UnsafeOnCompleted(continuation);
			}
		}
	}
}
