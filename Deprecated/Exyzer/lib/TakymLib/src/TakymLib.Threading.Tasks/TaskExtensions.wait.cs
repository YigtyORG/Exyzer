/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace TakymLib.Threading.Tasks
{
	partial class TaskExtensions
	{
		/// <summary>
		///  指定されたタスクの実行が完了するまで待機します。
		/// </summary>
		/// <param name="task"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトを指定します。</param>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		public static void Wait(this ValueTask task, bool doYield = true)
		{
			while (!task.IsCompleted) {
				if (doYield) {
					TaskUtility.YieldAndWait();
				}
			}
		}

		/// <summary>
		///  指定されたタスクの実行が完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="task"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトを指定します。</param>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <returns><paramref name="task"/>の実行結果を返します。</returns>
		public static TResult Wait<TResult>(this ValueTask<TResult> task, bool doYield = true)
		{
			while (!task.IsCompleted) {
				if (doYield) {
					TaskUtility.YieldAndWait();
				}
			}
			return task.Result;
		}

		/// <summary>
		///  指定された全てのタスクが完了するまで待機します。
		/// </summary>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトの配列を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public static void WaitAll(bool doYield, params ValueTask[] tasks)
		{
			tasks.EnsureNotNull();
			for (int i = 0; i < tasks.Length; ++i) {
				tasks[i].Wait(doYield);
			}
		}

		/// <summary>
		///  指定された全てのタスクが完了するまで待機します。
		/// </summary>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトの配列を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public static void WaitAll(params ValueTask[] tasks)
		{
			WaitAll(true, tasks);
		}

		/// <summary>
		///  指定された全てのタスクが完了するまで待機します。
		/// </summary>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトを列挙するオブジェクトを指定します。</param>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <exception cref="System.ArgumentNullException"/>
		public static void WaitAll(this IEnumerable<ValueTask> tasks, bool doYield = true)
		{
			tasks.EnsureNotNull();
			if (tasks is ValueTask[] array) {
				WaitAll(doYield, array);
			}
			foreach (var task in tasks) {
				task.Wait(doYield);
			}
		}

		/// <summary>
		///  指定された全てのタスクが完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトの配列を指定します。</param>
		/// <returns><paramref name="tasks"/>の実行結果の配列を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TResult[] WaitAll<TResult>(bool doYield, params ValueTask<TResult>[] tasks)
		{
			tasks.EnsureNotNull();
			var results = new TResult[tasks.Length];
			for (int i = 0; i < tasks.Length; ++i) {
				results[i] = tasks[i].Wait(doYield);
			}
			return results;
		}

		/// <summary>
		///  指定された全てのタスクが完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトの配列を指定します。</param>
		/// <returns><paramref name="tasks"/>の実行結果の配列を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TResult[] WaitAll<TResult>(params ValueTask<TResult>[] tasks)
		{
			return WaitAll(true, tasks);
		}

		/// <summary>
		///  指定された全てのタスクが完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトを列挙するオブジェクトを指定します。</param>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <returns><paramref name="tasks"/>の実行結果を列挙するオブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static IEnumerable<TResult> WaitAll<TResult>(this IEnumerable<ValueTask<TResult>> tasks, bool doYield = true)
		{
			tasks.EnsureNotNull();
			if (tasks is ValueTask<TResult>[] array) {
				return WaitAll(doYield, array);
			}
			return WaitAllCore();

			IEnumerable<TResult> WaitAllCore()
			{
				foreach (var task in tasks) {
					yield return task.Wait(doYield);
				}
			}
		}

		/// <summary>
		///  指定されたタスクの何れかが完了するまで待機します。
		/// </summary>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトの配列を指定します。</param>
		/// <returns>完了したタスクのインデックス番号を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static int WaitAny(bool doYield, params ValueTask[] tasks)
		{
			tasks.EnsureNotNull();
			while (true) {
				for (int i = 0; i < tasks.Length; ++i) {
					var task = tasks[i];
					if (task.IsCompleted) {
						return i;
					} else {
						if (doYield) {
							TaskUtility.YieldAndWait(i);
						}
					}
				}
			}
		}

		/// <summary>
		///  指定されたタスクの何れかが完了するまで待機します。
		/// </summary>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトの配列を指定します。</param>
		/// <returns>完了したタスクのインデックス番号を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static int WaitAny(params ValueTask[] tasks)
		{
			return WaitAny(true, tasks);
		}

		/// <summary>
		///  指定されたタスクの何れかが完了するまで待機します。
		/// </summary>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask"/>オブジェクトを列挙するオブジェクトを指定します。</param>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <returns>完了したタスクを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static ValueTask WaitAny(this IEnumerable<ValueTask> tasks, bool doYield = true)
		{
			tasks.EnsureNotNull();
			if (tasks is ValueTask[] array) {
				return array[WaitAny(doYield, array)];
			}
			while (true) {
				foreach (var task in tasks) {
					if (task.IsCompleted) {
						return task;
					} else {
						if (doYield) {
							TaskUtility.YieldAndWait();
						}
					}
				}
			}
		}

		/// <summary>
		///  指定されたタスクの何れかが完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトの配列を指定します。</param>
		/// <returns>完了したタスクのインデックス番号を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static int WaitAny<TResult>(bool doYield, params ValueTask<TResult>[] tasks)
		{
			tasks.EnsureNotNull();
			while (true) {
				for (int i = 0; i < tasks.Length; ++i) {
					var task = tasks[i];
					if (task.IsCompleted) {
						return i;
					} else {
						if (doYield) {
							TaskUtility.YieldAndWait(i);
						}
					}
				}
			}
		}

		/// <summary>
		///  指定されたタスクの何れかが完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトの配列を指定します。</param>
		/// <returns>完了したタスクのインデックス番号を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static int WaitAny<TResult>(params ValueTask<TResult>[] tasks)
		{
			return WaitAny(true, tasks);
		}

		/// <summary>
		///  指定されたタスクの何れかが完了するまで待機します。
		/// </summary>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="tasks"><see cref="System.Threading.Tasks.ValueTask{TResult}"/>オブジェクトを列挙するオブジェクトを指定します。</param>
		/// <param name="doYield">
		///  待機時に別のスレッドへ処理時間を譲る場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="true"/>です。
		/// </param>
		/// <returns>完了したタスクを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static ValueTask<TResult> WaitAny<TResult>(this IEnumerable<ValueTask<TResult>> tasks, bool doYield = true)
		{
			tasks.EnsureNotNull();
			if (tasks is ValueTask<TResult>[] array) {
				return array[WaitAny(doYield, array)];
			}
			while (true) {
				foreach (var task in tasks) {
					if (task.IsCompleted) {
						return task;
					} else {
						if (doYield) {
							TaskUtility.YieldAndWait();
						}
					}
				}
			}
		}
	}
}
