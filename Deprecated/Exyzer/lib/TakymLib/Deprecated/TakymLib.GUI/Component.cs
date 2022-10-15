/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.GUI
{
	/// <summary>
	///  機能を持たない部品(コンポーネント)を表します。
	/// </summary>
	public class Component : DisposableBase, IComponent
	{
		internal ConcurrentQueue<Action> Actions { get; }

		/// <summary>
		///  現在の部品に関連付けられた<see cref="TakymLib.GUI.Dispatcher"/>を取得します。
		/// </summary>
		public Dispatcher Dispatcher { get; }

		/// <inheritdoc/>
		public ISite? Site { get; set; }

		/// <inheritdoc/>
		public event EventHandler? Disposed;

		/// <summary>
		///  型'<see cref="TakymLib.GUI.Component"/>'の新しいインスタンスを生成します。
		/// </summary>
		public Component()
		{
			this.Actions    = new();
			this.Dispatcher = new(this);
		}

		/// <summary>
		///  型'<see cref="TakymLib.GUI.Component"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="container">新しく生成する部品を格納するオブジェクトを指定します。</param>
		public Component(IContainer container) : this()
		{
			container.Add(this);
		}

		/// <summary>
		///  指定された処理を現在の部品を生成したスレッドで実行する様に予約し、
		///  実行が終わるまで待機します。
		/// </summary>
		/// <param name="action">実行する処理を指定します。</param>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public void Invoke(Action action)
		{
			this.RunSafely(static state => {
				state.Item1.Dispatcher.Schedule(state.action);

				bool wait;
				do {
					wait = false;
					foreach (var action in state.Item1.Actions) {
						if (action == state.action) {
							wait = true;
							Thread.Yield();
							continue;
						}
					}
				} while (wait);
			}, (this, action));
		}

		/// <summary>
		///  指定された処理を現在の部品を生成したスレッドで実行する様に予約し、
		///  実行が終わるまで非同期的に待機します。
		/// </summary>
		/// <param name="action">実行する処理を指定します。</param>
		/// <returns>この処理の非同期操作を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public ValueTask InvokeAsync(Action action)
		{
			return this.RunSafelyAsync(static async state => {
				state.Item1.Dispatcher.Schedule(state.action);

				bool wait;
				do {
					wait = false;
					foreach (var action in state.Item1.Actions) {
						if (action == state.action) {
							wait = true;
							await Task.Yield();
							continue;
						}
					}
				} while (wait);
			}, (this, action));
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				this.Dispatcher.Dispose();
			}
			base.Dispose(disposing);
			this.Disposed?.Invoke(this, new());
		}

		/// <inheritdoc/>
		protected override async ValueTask DisposeAsyncCore()
		{
			await this.Dispatcher.DisposeAsync();
			await base.DisposeAsyncCore();
		}
	}
}
