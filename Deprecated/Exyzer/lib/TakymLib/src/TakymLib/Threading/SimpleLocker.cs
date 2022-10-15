/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading;
using System.Threading.Tasks;
using TakymLib.Threading.Tasks;

namespace TakymLib.Threading
{
	/// <summary>
	///  単純な排他制御を行います。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  再帰的に使用した場合の動作は未定義です。
	/// </remarks>
	public sealed class SimpleLocker
	{
		private const    uint    SHARED       = 0;
		private const    uint    LOCKED       = 1;
		private const    uint    LOCKED_ASYNC = 2;
		private          uint    _state;
		private          Thread? _thread;
		private readonly bool    _do_yield;

		/// <summary>
		///  待機時に別のスレッドへ処理時間を譲るかどうかを示す論理値を取得します。
		/// </summary>
		public bool DoYield => _do_yield;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.SimpleLocker"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		public SimpleLocker(bool doYield = true)
		{
			_state    = SHARED;
			_thread   = null;
			_do_yield = doYield;
		}

		/// <summary>
		///  ロックを取得します。
		///  既にロックされている場合は解除されるまで待機します。
		/// </summary>
		/// <param name="lockTaken">
		///  ロックの取得に成功した場合は<see langword="true"/>を返します。
		///  現在のスレッドで既にロックされている場合は<see langword="false"/>を返します。
		/// </param>
		public void EnterLock(ref bool lockTaken)
		{
			var thread = Thread.CurrentThread;
			if (_thread == thread && _state == LOCKED) {
				lockTaken = false;
				return;
			}
			while (Interlocked.CompareExchange(ref _state, LOCKED, SHARED) != SHARED) {
				if (_do_yield) {
					TaskUtility.YieldAndWait();
				}
			}
			_thread   = thread;
			lockTaken = true;
		}

		/// <summary>
		///  非同期的にロックします。
		///  既にロックされている場合は解除されるまで待機します。
		/// </summary>
		/// <returns>この処理の非同期操作を返します。</returns>
		public async ValueTask EnterLockAsync()
		{
			while (Interlocked.CompareExchange(ref _state, LOCKED_ASYNC, SHARED) != SHARED) {
				if (_do_yield) {
					await TaskUtility.Yield();
				}
			}

			// 非同期の場合はスレッドに依存しない。
			_thread = null;
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.SimpleLocker.EnterLock(ref bool)"/>により取得したロックを解除します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.Threading.SimpleLocker.EnterLock(ref bool)"/>を呼び出したスレッドと同じスレッドからのみ呼び出す事ができます。
		/// </remarks>
		/// <returns>成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool LeaveLock()
		{
			if (_thread is not null && _thread != Thread.CurrentThread) {
				return false;
			}
			while (true) {
				uint oldStatus = _state;
				if (oldStatus == LOCKED_ASYNC) {
					return false;
				}
				if (oldStatus == SHARED ||
					Interlocked.CompareExchange(ref _state, SHARED, oldStatus) == oldStatus) {
					return true;
				}
				if (_do_yield) {
					TaskUtility.YieldAndWait();
				}
			}
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.SimpleLocker.LeaveLockAsync"/>により取得したロックを解除します。
		/// </summary>
		/// <returns>成功したかどうかを示す論理値を含むこの処理の非同期操作を返します。</returns>
		public async ValueTask<bool> LeaveLockAsync()
		{
			while (true) {
				uint oldStatus = _state;
				if (oldStatus == LOCKED) {
					return false;
				}
				if (oldStatus == SHARED ||
					Interlocked.CompareExchange(ref _state, SHARED, oldStatus) == oldStatus) {
					return true;
				}
				if (_do_yield) {
					await TaskUtility.Yield();
				}
			}
		}

		/// <summary>
		///  現在の状態を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.SimpleLocker.State"/>列挙値を返します。</returns>
		public State GetState()
		{
			if (_state == SHARED) {
				return State.Shared;
			}
			return State.Locked;
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.SimpleLocker.State"/>の状態を表します。
		/// </summary>
		public enum State
		{
			/// <summary>
			///  共有中である事を表します。
			/// </summary>
			Shared,

			/// <summary>
			///  排他制御中である事を表します。
			/// </summary>
			Locked,
		}
	}
}
