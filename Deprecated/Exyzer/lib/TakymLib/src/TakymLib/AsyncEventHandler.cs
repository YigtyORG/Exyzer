/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  イベントを処理する非同期関数を表します。
	/// </summary>
	/// <param name="sender">イベントの発生源です。</param>
	/// <param name="e">空のイベントデータを格納しているオブジェクトです。</param>
	/// <returns>この処理の非同期操作です。</returns>
	public delegate ValueTask AsyncEventHandler(object? sender, EventArgs e);

	/// <summary>
	///  イベントを処理する非同期関数を表します。
	/// </summary>
	/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
	/// <param name="sender">イベントの発生源です。</param>
	/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
	/// <returns>この処理の非同期操作です。</returns>
	public delegate ValueTask AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e) where TEventArgs: EventArgs;

	/// <summary>
	///  イベントを処理する非同期関数を表します。
	/// </summary>
	/// <typeparam name="TSender">発生源の種類です。</typeparam>
	/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
	/// <param name="sender">イベントの発生源です。</param>
	/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
	/// <returns>この処理の非同期操作です。</returns>
	public delegate ValueTask AsyncEventHandler<TSender, TEventArgs>(TSender? sender, TEventArgs e) where TEventArgs: EventArgs;

	/// <summary>
	///  型'<see cref="TakymLib.AsyncEventHandler"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class AsyncEventHandlerExtensions
	{
		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">空のイベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		public static ValueTask Dispatch(this AsyncEventHandler? handler, object? sender, EventArgs e)
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		public static ValueTask Dispatch<TEventArgs>(this AsyncEventHandler<TEventArgs>? handler, object? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TSender">発生源の種類です。</typeparam>
		/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		public static ValueTask Dispatch<TSender, TEventArgs>(this AsyncEventHandler<TSender, TEventArgs>? handler, TSender? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TDelegate">デリゲートの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">空のイベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		[Obsolete("代わりに AsyncEventHandler を利用してください。", DiagnosticId = "TakymLib_Dispatch")]
		public static ValueTask Dispatch<TDelegate>(this TDelegate handler, object? sender, EventArgs e)
			where TDelegate: Delegate
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TDelegate">デリゲートの種類です。</typeparam>
		/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		[Obsolete("代わりに AsyncEventHandler を利用してください。", DiagnosticId = "TakymLib_Dispatch")]
		public static ValueTask Dispatch<TDelegate, TEventArgs>(this TDelegate handler, object? sender, TEventArgs e)
			where TDelegate : Delegate
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TDelegate">デリゲートの種類です。</typeparam>
		/// <typeparam name="TSender">発生源の種類です。</typeparam>
		/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		[Obsolete("代わりに AsyncEventHandler を利用してください。", DiagnosticId = "TakymLib_Dispatch")]
		public static ValueTask Dispatch<TDelegate, TSender, TEventArgs>(this TDelegate handler, TSender? sender, TEventArgs e)
			where TDelegate : Delegate
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

#pragma warning disable CA2012 // ValueTask を正しく使用する必要があります
		private static async ValueTask DispatchCore<TSender, TEventArgs>(Delegate handler, TSender? sender, TEventArgs e)
			where TEventArgs : EventArgs
		{
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = WrapHandler<TSender, TEventArgs>(handlers[i])(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1回のみ実行可
				}
			}
		}
#pragma warning restore CA2012 // ValueTask を正しく使用する必要があります

		private static AsyncEventHandler<TSender, TEventArgs> WrapHandler<TSender, TEventArgs>(Delegate handler)
			where TEventArgs : EventArgs
		{
			return handler switch {
				AsyncEventHandler<TSender, TEventArgs> h => h,
				AsyncEventHandler<TEventArgs>          h => (sender, e) =>   h(sender, e),
				AsyncEventHandler                      h => (sender, e) =>   h(sender, e),
				Func<object?,  EventArgs,  ValueTask>  h => (sender, e) =>   h(sender, e),
				Func<object?,  TEventArgs, ValueTask>  h => (sender, e) =>   h(sender, e),
				Func<TSender?, TEventArgs, ValueTask>  h => (sender, e) =>   h(sender, e),
				EventHandler<TEventArgs>               h => (sender, e) => { h(sender, e); return default; },
				EventHandler                           h => (sender, e) => { h(sender, e); return default; },
				Action<object?,  EventArgs>            h => (sender, e) => { h(sender, e); return default; },
				Action<object?,  TEventArgs>           h => (sender, e) => { h(sender, e); return default; },
				Action<TSender?, TEventArgs>           h => (sender, e) => { h(sender, e); return default; },
				_ => throw new ArgumentException(Resources.AsyncEventHandlerExtensions_WrapHandler, nameof(handler))
			};
		}
	}
}
