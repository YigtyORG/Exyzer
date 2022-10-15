/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Input;
using TakymLib.UI.Properties;

namespace TakymLib.UI.Models
{
	/// <summary>
	///  <see cref="TakymLib.UI.Models.ViewModelBase"/>内にコマンドを定義します。
	/// </summary>
	public sealed class DelegateCommand : ICommand
	{
		private readonly Delegate _action;

		/// <summary>
		///  非同期処理の実行結果を取得します。
		/// </summary>
		/// <remarks>
		///  このプロパティは<see cref="TakymLib.UI.Models.DelegateCommand.DelegateCommand(Func{object?, ValueTask})"/>
		///  からインスタンスを生成した場合にのみ有効です。
		/// </remarks>
		/// <value>
		///  <see cref="System.Threading.Tasks.ValueTask"/>の取り扱いに注意してください。
		/// </value>
		public ConcurrentQueue<ValueTask>? ActionResults { get; }

		/// <summary>
		///  型'<see cref="TakymLib.UI.Models.DelegateCommand"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="action">実行する処理を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public DelegateCommand(Action<object?> action)
		{
			action.EnsureNotNull();
			_action = action;
		}

		/// <summary>
		///  型'<see cref="TakymLib.UI.Models.DelegateCommand"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="actionAsync">非同期的に実行する処理を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public DelegateCommand(Func<object?, ValueTask> actionAsync)
		{
			actionAsync.EnsureNotNull();
			_action            = actionAsync;
			this.ActionResults = new();
		}

		/// <summary>
		///  コマンドを実行します。
		/// </summary>
		/// <param name="parameter">コマンドに渡す引数を指定します。</param>
		/// <exception cref="System.InvalidOperationException"/>
		public void Execute(object? parameter)
		{
			switch (_action) {
			case Action<object?> action:
				action(parameter);
				break;
#pragma warning disable CA2012 // ValueTask を正しく使用する必要があります
			case Func<object?, ValueTask> actionAsync:
				this.ActionResults?.Enqueue(actionAsync(parameter));
				break;
#pragma warning restore CA2012 // ValueTask を正しく使用する必要があります
			default:
				// ここには到達しない。
				throw new InvalidOperationException(string.Format(
					Resources.DelegateCommand_Execute_InvalidOperationException,
					(_action?.GetType() ?? typeof(void)).FullName
				));
			}
		}

		#region CanExecute の値が変わる事は一切ない。

		event EventHandler? ICommand.CanExecuteChanged { add { } remove { } }

		bool ICommand.CanExecute(object? parameter)
		{
			return true;
		}

		#endregion
	}
}
