using System;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  タスクに関する便利関数を提供します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class TaskUtility
	{
		[ThreadStatic()]
		private static Random? _rnd;

		/// <summary>
		///  <c>8</c>分の<c>1</c>の確率で<see cref="System.Threading.Thread.Yield"/>を呼び出し待機します。
		/// </summary>
		public static void YieldAndWait()
		{
			_rnd ??= new();
			if (_rnd.Next() % 8 == 0) {
				Thread.Yield();
			}
		}

		/// <summary>
		///  <paramref name="i"/>を<c>32</c>で割った余りが<c>31</c>の時に<see cref="System.Threading.Thread.Yield"/>を呼び出し待機します。
		/// </summary>
		/// <param name="i"><see cref="System.Threading.Thread.Yield"/>の呼び出し頻度を調節する整数を指定します。</param>
		public static void YieldAndWait(int i)
		{
			if (i % 32 == 31) {
				Thread.Yield();
			}
		}

		/// <summary>
		///  <c>8</c>分の<c>1</c>の確率で<see cref="System.Threading.Tasks.Task.Yield"/>を呼び出します。
		/// </summary>
		/// <returns>この処理の非同期操作です。</returns>
		public static async ValueTask Yield()
		{
			_rnd ??= new();
			if (_rnd.Next() % 8 == 0) {
				await Task.Yield();
			}
		}

		/// <summary>
		///  <paramref name="i"/>を<c>32</c>で割った余りが<c>31</c>の時に<see cref="System.Threading.Tasks.Task.Yield"/>を呼び出します。
		/// </summary>
		/// <param name="i"><see cref="System.Threading.Tasks.Task.Yield"/>の呼び出し頻度を調節する整数を指定します。</param>
		/// <returns>この処理の非同期操作です。</returns>
		public static async ValueTask Yield(int i)
		{
			if (i % 32 == 31) {
				await Task.Yield();
			}
		}
	}
}
