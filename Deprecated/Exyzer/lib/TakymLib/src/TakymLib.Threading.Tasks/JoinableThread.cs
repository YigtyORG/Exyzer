/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using TakymLib.Threading.Tasks.Core;
using TakymLib.Threading.Tasks.Internals;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  結合可能なスレッドを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class JoinableThread : DisposableBase
	{
		/// <summary>
		///  新しい結合可能なスレッドを生成します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.JoinableThread"/>オブジェクトです。</returns>
		public static JoinableThread Create()
		{
			return new JoinableThreadInternal();
		}

		/// <summary>
		///  現在のスレッドから新しい結合可能なスレッドを生成します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Core.DefaultJoinableThread"/>オブジェクトです。</returns>
		public static DefaultJoinableThread CreateFromCurrentThread()
		{
			return new();
		}

		/// <summary>
		///  対象のスレッドを取得します。
		/// </summary>
		public virtual Thread Thread => Thread.CurrentThread;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.JoinableThread"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected JoinableThread() { }

		/// <summary>
		///  対象のスレッドへ切り替えます。切り替えを行う場合、戻り値を待機する必要があります。
		/// </summary>
		/// <returns>スレッドの切り替えを行う非同期操作を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public SwitchAsyncResult SwitchAsync()
		{
			this.EnsureNotDisposed();
			return new(this);
		}

		/// <summary>
		///  指定された処理を対象のスレッドで実行する様に設定します。
		/// </summary>
		/// <param name="action"><see cref="TakymLib.Threading.Tasks.JoinableThread.Thread"/>で実行する処理を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void Schedule(Action action)
		{
			action.EnsureNotNull();
			this.RunSafely(this.ScheduleCore, action);
		}

		/// <summary>
		///  上書きされた場合、指定された処理を対象のスレッドで実行する様に設定します。
		/// </summary>
		/// <param name="action"><see cref="TakymLib.Threading.Tasks.JoinableThread.Thread"/>で実行する処理を指定します。</param>
		protected abstract void ScheduleCore(Action action);

		/// <summary>
		///  <see cref="TakymLib.Threading.Tasks.JoinableThread.SwitchAsync"/>の戻り値を表します。
		/// </summary>
		public struct SwitchAsyncResult : IAwaitable, IAwaiter
		{
			private readonly JoinableThread? _owner;
			private          bool            _completed;

			/// <inheritdoc/>
			public bool IsCompleted => _completed;

			internal SwitchAsyncResult(JoinableThread owner)
			{
				_owner     = owner;
				_completed = false;
			}

			/// <summary>
			///  現在のインスタンスを返します。
			/// </summary>
			/// <returns><see cref="TakymLib.Threading.Tasks.JoinableThread.SwitchAsyncResult"/>オブジェクトです。</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public SwitchAsyncResult GetAwaiter()
			{
				return this;
			}

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void GetResult()
			{
				// do nothing
			}

			/// <inheritdoc/>
			/// <exception cref="System.ObjectDisposedException"/>
			public void OnCompleted(Action continuation)
			{
				if (_completed) {
					return;
				}
				_completed = true;
				if (_owner is null) {
					continuation();
				} else {
					_owner.Schedule(continuation);
				}
			}

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UnsafeOnCompleted(Action continuation)
			{
				this.OnCompleted(continuation);
			}

			IAwaiter IAwaitable.GetAwaiter()
			{
				return this.GetAwaiter();
			}
		}
	}
}
