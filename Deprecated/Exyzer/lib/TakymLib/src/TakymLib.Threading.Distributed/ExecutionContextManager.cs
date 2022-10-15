/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading;
using TakymLib.Threading.Distributed.Internals;

namespace TakymLib.Threading.Distributed
{
	/// <summary>
	///  実行文脈情報を管理します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class ExecutionContextManager : DisposableBase
	{
		/// <summary>
		///  既定の<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>オブジェクトを取得します。
		/// </summary>
		/// <remarks>
		///  このインスタンスは破棄（<see cref="System.IDisposable.Dispose"/>の呼び出し）できません。
		/// </remarks>
		/// <returns>有効な<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>オブジェクトです。</returns>
		public static ExecutionContextManager GetDefault()
		{
			return DefaultExecutionContextManager._inst;
		}

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  稀に破棄処理時に<see cref="System.Threading.SynchronizationLockException"/>が発生する可能性があります。
		/// </remarks>
		/// <returns>有効な<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>オブジェクトです。</returns>
		public static ExecutionContextManager Create()
		{
			return new ExecutionContextManagerInternal();
		}

		/// <summary>
		///  現在のスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		public ExecutionContext Client
		{
			get
			{
				this.EnterRunLock();
				try {
					return this.GetClientContextCore();
				} finally {
					this.LeaveRunLock();
				}
			}
		}

		/// <summary>
		///  上書きされた場合、現在のスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Distributed.ExecutionContext"/>オブジェクトです。</returns>
		protected abstract ExecutionContext GetClientContextCore();

		/// <summary>
		///  指定されたスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <param name="thread">スレッドです。</param>
		/// <returns><see cref="TakymLib.Threading.Distributed.ExecutionContext"/>オブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public ExecutionContext GetServerContext(Thread thread)
		{
			thread.EnsureNotNull();
			this.EnterRunLock();
			try {
				return this.GetServerContextCore(thread);
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  上書きされた場合、指定されたスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <param name="thread">スレッドです。</param>
		/// <returns><see cref="TakymLib.Threading.Distributed.ExecutionContext"/>オブジェクトです。</returns>
		protected abstract ExecutionContext GetServerContextCore(Thread thread);

		/// <summary>
		///  指定されたスレッドへ接続します。
		/// </summary>
		/// <param name="thread">接続先のスレッドです。</param>
		/// <returns>接続情報を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public ConnectedContext Connect(Thread thread)
		{
			thread.EnsureNotNull();
			this.EnterRunLock();
			try {
				return new ConnectedContext(
					this.GetServerContextCore(thread),
					this.GetClientContextCore(),
					true
				);
			} finally {
				this.LeaveRunLock();
			}
		}
	}
}
