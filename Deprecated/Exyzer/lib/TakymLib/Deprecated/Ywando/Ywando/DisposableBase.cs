/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ywando
{
	/// <summary>
	///  破棄可能なクラスの基底クラスです。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class DisposableBase : IDisposable, IAsyncDisposable
	{
		/// <summary>
		///  このオブジェクトの破棄処理を実行している場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposing { get; private set; }

		/// <summary>
		///  このオブジェクトが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		///  型'<see cref="Ywando.DisposableBase"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected DisposableBase() { }

		/// <summary>
		///  型'<see cref="Ywando.DisposableBase"/>'の現在のインスタンスを破棄します。
		/// </summary>
		~DisposableBase()
		{
			if (this.IsDisposing) {
				return;
			}
			this.IsDisposing = true;
			this.Dispose(false);
			this.IsDisposing = false;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			if (this.IsDisposing) {
				return;
			}
			this.IsDisposing = true;
			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.IsDisposing = false;
		}

#pragma warning disable CA1816 // Dispose メソッドは、SuppressFinalize を呼び出す必要があります
		/// <inheritdoc/>
		public async ValueTask DisposeAsync()
		{
			if (this.IsDisposing) {
				return;
			}
			this.IsDisposing = true;
			await this.DisposeAsyncCore();
			this.Dispose(false);
			GC.SuppressFinalize(this);
			this.IsDisposing = false;
		}
#pragma warning restore CA1816 // Dispose メソッドは、SuppressFinalize を呼び出す必要があります

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		/// </summary>
		/// <remarks>
		///  この関数内では例外を発生させてはいけません。
		/// </remarks>
		/// <param name="disposing">
		///  マネージドリソースとアンマネージリソース両方を破棄する場合は<see langword="true"/>、
		///  アンマネージリソースのみを破棄する場合は<see langword="false"/>を設定します。
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			this.IsDisposed = true;
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているマネージドリソースを非同期的に破棄します。
		/// </summary>
		/// <remarks>
		///  この関数内では例外を発生させてはいけません。
		/// </remarks>
		/// <returns>この処理の非同期操作です。</returns>
		protected virtual ValueTask DisposeAsyncCore()
		{
			return default;
		}

		/// <summary>
		///  現在のインスタンスが破棄されている場合に例外を発生させます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException" />
		[DebuggerHidden()]
		protected void EnsureNotDisposed()
		{
			if (this.IsDisposing || this.IsDisposed) {
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}
	}
}
