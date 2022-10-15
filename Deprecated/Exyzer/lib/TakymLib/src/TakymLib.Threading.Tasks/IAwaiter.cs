/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  <see cref="TakymLib.Threading.Tasks.IAwaitable"/>の待機を行う機能を提供します。
	/// </summary>
	public interface IAwaiter : ICriticalNotifyCompletion
	{
		/// <summary>
		///  操作が完了した場合は<see langword="true"/>、まだ操作を実行している場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsCompleted { get; }

		/// <summary>
		///  待機します。
		/// </summary>
		public void GetResult();
	}

	/// <summary>
	///  <see cref="TakymLib.Threading.Tasks.IAwaitable{TResult}"/>の待機を行う機能を提供します。
	/// </summary>
	/// <typeparam name="TResult">結果の種類です。</typeparam>
	public interface IAwaiter<out TResult> : IAwaiter
	{
		/// <summary>
		///  操作の完了を待機し、結果を取得します。
		/// </summary>
		/// <returns>操作の結果を表すオブジェクトです。</returns>
		public new TResult? GetResult();

		void IAwaiter.GetResult()
		{
			this.GetResult();
		}
	}
}
