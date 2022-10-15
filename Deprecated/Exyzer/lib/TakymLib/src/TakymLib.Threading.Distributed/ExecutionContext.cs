/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Concurrent;
using System.Threading.Tasks;
using TakymLib.Threading.Tasks;

namespace TakymLib.Threading.Distributed
{
	/// <summary>
	///  実行文脈情報を表します。
	/// </summary>
	public class ExecutionContext : DisposableBase
	{
		private readonly ConcurrentQueue<(ExecutionContext?, object?)> _objs;

		/// <summary>
		///  <see cref="TakymLib.Threading.Distributed.ExecutionContext.SendObject(ExecutionContext?, object?)"/>
		///  を非同期的に実行するかどうかを示す論理値を取得または設定します。
		/// </summary>
		/// <remarks>
		///  既定値は<see langword="false"/>です。
		/// </remarks>
		/// <value>
		///  非同期的に実行する必要がある場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </value>
		/// <returns>
		///  非同期的に実行する様に設定されている場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public bool RunSendObjectAsync { get; set; }

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Distributed.ExecutionContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ExecutionContext()
		{
			_objs = new();
		}

		/// <summary>
		///  オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="sender">オブジェクトの送信を行うスレッドの実行文脈です。</param>
		/// <param name="value">送信するオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public async ValueTask SendObject(ExecutionContext? sender, object? value)
		{
			this.EnterRunLock();
			try {
				var item = (sender, value);
				if (this.RunSendObjectAsync) {
					await Task.Run(() => _objs.Enqueue(item));
				} else {
					_objs.Enqueue(item);
				}
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="value">送信するオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public ValueTask SendObject(object? value)
		{
			return this.SendObject(null, value);
		}

		/// <summary>
		///  別スレッドからオブジェクトを受信します。
		///  また、送信元スレッドの実行文脈も取得します。
		/// </summary>
		/// <returns>送信元情報と受信したオブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public async ValueTask<(ExecutionContext? Sender, object? Value)> ReceiveObjectWithSender()
		{
			this.EnterRunLock();
			try {
				(ExecutionContext?, object?) result;
				while (_objs.TryDequeue(out result)) {
					await TaskUtility.Yield();
				}
				return result;
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  別スレッドからオブジェクトを受信します。
		/// </summary>
		/// <returns>受信したオブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public async ValueTask<object?> ReceiveObject()
		{
			return (await this.ReceiveObjectWithSender()).Value;
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			_objs.Clear();
			this.Dispose(disposing);
		}
	}
}
