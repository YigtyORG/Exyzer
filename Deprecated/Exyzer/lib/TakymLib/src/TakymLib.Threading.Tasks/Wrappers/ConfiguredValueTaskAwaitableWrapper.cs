/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable;

namespace TakymLib.Threading.Tasks.Wrappers
{
	/// <summary>
	///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable"/>を<see cref="TakymLib.Threading.Tasks.IAwaitable"/>として扱える様にします。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct ConfiguredValueTaskAwaitableWrapper : IAwaitable
	{
		private readonly ConfiguredValueTaskAwaitable _awaitable;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredValueTaskAwaitableWrapper"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="awaitable">ラップする待機可能なオブジェクトです。</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredValueTaskAwaitableWrapper(ConfiguredValueTaskAwaitable awaitable)
		{
			_awaitable = awaitable;
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.GetAwaiter"/>を呼び出します。
		/// </summary>
		/// <returns>
		///  <see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredValueTaskAwaitableWrapper.ConfiguredValueTaskAwaiterWrapper"/>オブジェクトです。
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredValueTaskAwaiterWrapper GetAwaiter()
		{
			return new(_awaitable.GetAwaiter());
		}

		IAwaiter IAwaitable.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter"/>を
		///  <see cref="TakymLib.Threading.Tasks.IAwaiter"/>として扱える様にします。
		///  このクラスは読み取り専用構造体です。
		/// </summary>
		public readonly struct ConfiguredValueTaskAwaiterWrapper : IAwaiter
		{
			private readonly ConfiguredValueTaskAwaiter _awaiter;

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter.IsCompleted"/>の値を取得します。
			/// </summary>
			public bool IsCompleted
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => _awaiter.IsCompleted;
			}

			/// <summary>
			///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredValueTaskAwaitableWrapper.ConfiguredValueTaskAwaiterWrapper"/>'
			///  の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="awaiter">ラップする待機オブジェクトです。</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ConfiguredValueTaskAwaiterWrapper(ConfiguredValueTaskAwaiter awaiter)
			{
				_awaiter = awaiter;
			}

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter.GetResult"/>を呼び出します。
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void GetResult()
			{
				_awaiter.GetResult();
			}

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter.OnCompleted(Action)"/>を呼び出します。
			/// </summary>
			/// <param name="continuation">完了時に実行する操作です。</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void OnCompleted(Action continuation)
			{
				_awaiter.OnCompleted(continuation);
			}

			/// <summary>
			///  <see cref="System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter.UnsafeOnCompleted(Action)"/>を呼び出します。
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
