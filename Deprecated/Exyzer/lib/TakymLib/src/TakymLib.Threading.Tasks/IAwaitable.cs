/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  待機可能なクラスを定義します。
	/// </summary>
	public interface IAwaitable
	{
		/// <summary>
		///  <see cref="TakymLib.Threading.Tasks.IAwaiter"/>を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトです。</returns>
		public IAwaiter GetAwaiter();
	}

	/// <summary>
	///  戻り値を持つ待機可能なクラスを定義します。
	/// </summary>
	/// <typeparam name="TResult">戻り値の種類です。</typeparam>
	public interface IAwaitable<out TResult> : IAwaitable
	{
		/// <summary>
		///  <see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトです。</returns>
		public new IAwaiter<TResult> GetAwaiter();

		IAwaiter IAwaitable.GetAwaiter()
		{
			return this.GetAwaiter();
		}
	}
}
