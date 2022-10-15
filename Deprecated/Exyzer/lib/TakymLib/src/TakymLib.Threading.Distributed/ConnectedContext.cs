/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using TakymLib.Threading.Tasks;

namespace TakymLib.Threading.Distributed
{
	/// <summary>
	///  スレッド間で接続された文脈情報を表します。
	/// </summary>
	public class ConnectedContext : DisposableBase
	{
		private readonly bool _leave_open;

		/// <summary>
		///  接続先のスレッドの実行文脈情報を取得します。
		/// </summary>
		public ExecutionContext Server { get; }

		/// <summary>
		///  現在のスレッドの実行文脈情報を取得します。
		/// </summary>
		public ExecutionContext Client { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Distributed.ConnectedContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="server">接続先のスレッドの実行文脈情報です。</param>
		/// <param name="client">現在のスレッドの実行文脈情報です。</param>
		/// <param name="leaveOpen">
		///  <see cref="TakymLib.Threading.Distributed.ConnectedContext"/>を破棄した後に、
		///  <paramref name="server"/>と<paramref name="client"/>を破棄する場合は<see langword="false"/>、
		///  それ以外の場合は<see langword="true"/>です。
		///  既定値は<see langword="false"/>です。
		/// </param>
		public ConnectedContext(ExecutionContext server, ExecutionContext client, bool leaveOpen = false)
		{
			this.Server = server;
			this.Client = client;
			_leave_open = leaveOpen;
		}

		/// <summary>
		///  オブジェクトを接続先のスレッドへ送信します。
		/// </summary>
		/// <param name="obj">送信するオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public ValueTask SendObject(object obj)
		{
			this.EnterRunLock();
			try {
				return this.Server.SendObject(this.Client, obj);
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  接続先のスレッドからオブジェクトを受信します。
		///  <see cref="TakymLib.Threading.Distributed.ConnectedContext.Server"/>以外の送信元を持つオブジェクトは読み飛ばされます。
		/// </summary>
		/// <returns>受信したオブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public async ValueTask<object?> ReceiveObject()
		{
			this.EnterRunLock();
			try {
				while (true) {
					var data = await this.Client.ReceiveObjectWithSender();
					if (data.Sender == this.Server) {
						return data.Value;
					}
					await TaskUtility.Yield();
				}
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  接続先のスレッドへオブジェクトを送信し、結果の受信が完了するまで待機します。
		/// </summary>
		/// <param name="obj">送信するオブジェクトです。</param>
		/// <returns>受信したオブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public async ValueTask<object?> SendAndReceive(object obj)
		{
			await this.SendObject(obj);
			return await this.ReceiveObject();
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing && !_leave_open) {
				this.Server.Dispose();
				this.Client.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <inheritdoc/>
		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			if (!_leave_open) {
				await this.Server.ConfigureAwait(false).DisposeAsync();
				await this.Client.ConfigureAwait(false).DisposeAsync();
			}
			await base.DisposeAsyncCore();
		}
	}
}
