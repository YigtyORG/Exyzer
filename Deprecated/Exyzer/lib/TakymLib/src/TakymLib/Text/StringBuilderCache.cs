/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using System.Threading;

namespace TakymLib.Text
{
	/// <summary>
	///  <see cref="System.Text.StringBuilder"/>のインスタンスをキャッシュします。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class StringBuilderCache
	{
		private readonly Memory<StringBuilder?> _cache;

		/// <summary>
		///  キャッシュするインスタンスの数を取得します。
		/// </summary>
		public int Capacity => _cache.Length;

		/// <summary>
		///  型'<see cref="TakymLib.Text.StringBuilderCache"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="capacity">キャッシュするインスタンスの数を指定します。</param>
		public StringBuilderCache(int capacity = 8)
		{
			_cache = new StringBuilder[capacity];
		}

		/// <summary>
		///  キャッシュされた<see cref="System.Text.StringBuilder"/>のインスタンスを取得します。
		/// </summary>
		/// <returns><see cref="System.Text.StringBuilder"/>オブジェクトを返します。</returns>
		public StringBuilder Acquire()
		{
			var cache = _cache.Span;
			for (int i = 0; i < cache.Length; ++i) {
				var sb = Interlocked.Exchange(ref cache[i], null);
				if (sb is not null) {
					return sb;
				}
			}
			return new();
		}

		/// <summary>
		///  <see cref="System.Text.StringBuilder"/>のインスタンスを解放します。
		/// </summary>
		/// <param name="sb">解放する<see cref="System.Text.StringBuilder"/>のインスタンスを指定します。</param>
		/// <returns><paramref name="sb"/>に格納されていた文字列を返します。</returns>
		/// <remarks>
		///  解放後は、別の場所で使用される可能性があるため、<paramref name="sb"/>を使用しないでください。
		/// </remarks>
		public string Release(StringBuilder? sb)
		{
			if (sb is null) {
				return string.Empty;
			}
			string result = sb.ToString();
			sb.Clear();
			var cache = _cache.Span;
			for (int i = 0; i < cache.Length; ++i) {
				if (Interlocked.CompareExchange(ref cache[i], sb, null) is null) {
					return result;
				}
			}
			return result;
		}
	}
}
