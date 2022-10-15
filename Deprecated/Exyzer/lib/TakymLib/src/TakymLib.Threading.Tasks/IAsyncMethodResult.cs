/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using TakymLib.Threading.Tasks.Core;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  非同期関数の戻り値を表します。
	/// </summary>
	[AsyncMethodBuilder(typeof(DefaultAsyncMethodBuilder))]
	public interface IAsyncMethodResult : IAwaitable, IAsyncResult
	{
		/// <summary>
		///  操作が正常に完了した場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsCompletedSuccessfully => this.IsCompleted && this.Exception is null;

		/// <summary>
		///  操作の実行に失敗した場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsFailed => this.IsCompleted && this.Exception is not null;

		/// <summary>
		///  操作が中止された場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsCancelled => this.IsCompleted && this.Exception is OperationCanceledException;

		/// <summary>
		///  失敗の原因となった例外を取得します。
		/// </summary>
		/// <remarks>
		///  成功した場合は<see langword="null"/>を返します。
		/// </remarks>
		public Exception? Exception { get; }

		/// <summary>
		///  この<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult"/>を構成します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元の実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <returns>構成された待機可能なオブジェクトです。</returns>
		public IAwaitable ConfigureAwait(bool continueOnCapturedContext);
	}

	/// <summary>
	///  非同期関数の戻り値を表します。
	/// </summary>
	/// <typeparam name="TResult">戻り値の種類です。</typeparam>
	[AsyncMethodBuilder(typeof(DefaultAsyncMethodBuilder<>))]
	public interface IAsyncMethodResult<out TResult> : IAsyncMethodResult, IAwaitable<TResult>
	{
		/// <summary>
		///  この<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult"/>を構成します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元の実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <returns>構成された待機可能なオブジェクトです。</returns>
		public new IAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext);

		IAwaitable IAsyncMethodResult.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
	}
}
