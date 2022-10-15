/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using TakymLib.Threading.Tasks.Wrappers;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  型'<see cref="System.Threading.Tasks.Task"/>'、
	///  型'<see cref="System.Threading.Tasks.Task{TResult}"/>'、
	///  型'<see cref="System.Threading.Tasks.ValueTask"/>'、
	///  型'<see cref="System.Threading.Tasks.ValueTask{TResult}"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static partial class TaskExtensions
	{
		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトとして使用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップする<see cref="System.Threading.Tasks.Task"/>オブジェクトを指定します。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter"/>を実装した構造体を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TaskWrapper WrapAwaitable(this Task task)
		{
			task.EnsureNotNull();
			return new(task);
		}

		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトとして使用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップする<see cref="System.Threading.Tasks.Task{TResult}"/>オブジェクトを指定します。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>を実装した構造体を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TaskWrapper<TResult> WrapAwaitable<TResult>(this Task<TResult> task)
		{
			task.EnsureNotNull();
			return new(task);
		}

		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトとして使用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップする<see cref="System.Threading.Tasks.ValueTask"/>オブジェクトを指定します。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter"/>を実装した構造体を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static ValueTaskWrapper WrapAwaitable(this ValueTask task)
		{
			task.EnsureNotNull();
			return new(task);
		}

		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトとして使用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップする<see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトを指定します。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>を実装した構造体を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static ValueTaskWrapper<TResult> WrapAwaitable<TResult>(this ValueTask<TResult> task)
		{
			task.EnsureNotNull();
			return new(task);
		}
	}
}
