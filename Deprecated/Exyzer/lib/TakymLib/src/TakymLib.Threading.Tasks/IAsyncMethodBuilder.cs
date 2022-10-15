/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  独自の非同期関数を生成する機能を提供します。
	/// </summary>
	/// <typeparam name="TTask">非同期関数の種類です。</typeparam>
	public interface IAsyncMethodBuilder<out TTask>
		where TTask: IAwaitable
	{
		/// <summary>
		///  <typeparamref name="TTask"/>を取得します。
		/// </summary>
		public TTask Task { get; }

		/// <summary>
		///  非同期関数を開始します。
		/// </summary>
		/// <typeparam name="TStateMachine">状態機械の種類です。</typeparam>
		/// <param name="stateMachine">非同期関数の状態機械です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void Start<TStateMachine>(ref TStateMachine stateMachine)
			where TStateMachine: IAsyncStateMachine;

		/// <summary>
		///  状態機械を設定します。
		/// </summary>
		/// <param name="stateMachine">非同期関数の状態機械です。</param>
		public void SetStateMachine(IAsyncStateMachine? stateMachine);

		/// <summary>
		///  例外を設定します。
		/// </summary>
		/// <param name="e">例外オブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void SetException(Exception e);

		/// <summary>
		///  戻り値を空値に設定します。
		/// </summary>
		public void SetResult();

		/// <summary>
		///  待機可能なオブジェクトの完了後に継続します。
		/// </summary>
		/// <typeparam name="TAwaiter">待機可能なオブジェクトの種類です。</typeparam>
		/// <typeparam name="TStateMachine">状態機械の種類です。</typeparam>
		/// <param name="awaiter">待機可能なオブジェクトです。</param>
		/// <param name="stateMachine">非同期関数の状態機械です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : INotifyCompletion
			where TStateMachine: IAsyncStateMachine;

		/// <summary>
		///  待機可能なオブジェクトの完了後に継続します。
		/// </summary>
		/// <typeparam name="TAwaiter">待機可能なオブジェクトの種類です。</typeparam>
		/// <typeparam name="TStateMachine">状態機械の種類です。</typeparam>
		/// <param name="awaiter">待機可能なオブジェクトです。</param>
		/// <param name="stateMachine">非同期関数の状態機械です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter     : ICriticalNotifyCompletion
			where TStateMachine: IAsyncStateMachine;
	}

	/// <summary>
	///  独自の非同期関数を生成する機能を提供します。
	/// </summary>
	/// <typeparam name="TTask">非同期関数の種類です。</typeparam>
	/// <typeparam name="TResult">戻り値の種類です。</typeparam>
	public interface IAsyncMethodBuilder<out TTask, in TResult> : IAsyncMethodBuilder<TTask>
		where TTask: IAwaitable<TResult>
	{
		/// <summary>
		///  戻り値を設定します。
		/// </summary>
		/// <param name="result">呼び出し元に返す値です。</param>
		public void SetResult(TResult? result);

		void IAsyncMethodBuilder<TTask>.SetResult()
		{
			this.SetResult(default);
		}
	}
}
