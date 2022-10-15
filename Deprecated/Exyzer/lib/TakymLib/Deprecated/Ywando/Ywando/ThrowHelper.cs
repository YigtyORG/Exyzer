/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;

namespace Ywando
{
	/// <summary>
	///  状況に応じて例外を発生させます。
	/// </summary>
	public static class ThrowHelper
	{
		/// <summary>
		///  <paramref name="arg"/>が<see langword="null"/>かどうか判定し、<see langword="null"/>である場合に例外を発生させます。
		/// </summary>
		/// <typeparam name="T"><paramref name="arg"/>の型です。</typeparam>
		/// <param name="arg"><see langword="null"/>判定を行うオブジェクトです。</param>
		/// <param name="argName"><paramref name="arg"/>の名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		[DebuggerHidden()]
		public static void EnsureNotNull<T>(this T arg, string? argName)
		{
			if (arg is null) {
				throw new ArgumentNullException(argName);
			}
		}
	}
}
