/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;
using System.Text;

namespace TakymLib.Text
{
	/// <summary>
	///  文字列を生成します。
	///  このクラスは静的クラスです。
	/// </summary>
	internal static class BuildString
	{
		private static readonly StringBuilderCache _sb_cache = new();

		/// <summary>
		///  文字列の構築を開始し、
		///  キャッシュされた<see cref="System.Text.StringBuilder"/>のインスタンスを取得します。
		/// </summary>
		/// <returns><see cref="System.Text.StringBuilder"/>オブジェクトを返します。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static StringBuilder Begin()
		{
			return _sb_cache.Acquire();
		}

		/// <summary>
		///  文字列の構築を終了し、
		///  <see cref="System.Text.StringBuilder"/>のインスタンスを解放します。
		/// </summary>
		/// <param name="sb">解放する<see cref="System.Text.StringBuilder"/>のインスタンスを指定します。</param>
		/// <returns><paramref name="sb"/>に格納されていた文字列を返します。</returns>
		/// <remarks>
		///  解放後は、別の場所で使用される可能性があるため、<paramref name="sb"/>を使用しないでください。
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string End(StringBuilder sb)
		{
			return _sb_cache.Release(sb);
		}
	}
}
