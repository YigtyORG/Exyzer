/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using System.Linq;

namespace TakymLib.Extensibility
{
	/// <summary>
	///  一つの追加機能を提供します。
	/// </summary>
	public interface IPlugin
	{
		/// <summary>
		///  この追加機能の翻訳済みの表示名を取得します。
		/// </summary>
		public string? DisplayName { get; }

		/// <summary>
		///  この追加機能の翻訳済みの説明を取得します。
		/// </summary>
		public string? Description { get; }

		/// <summary>
		///  この追加機能の子機能を列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IEnumerable{T}"/>オブジェクトを返します。</returns>
		public IEnumerable<IPlugin> EnumerateChildren();

		/// <summary>
		///  この追加機能の子機能を非同期的に列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		public IAsyncEnumerable<IPlugin> EnumerateChildrenAsync()
		{
			return this.EnumerateChildren().ToAsyncEnumerable() ?? AsyncEnumerable.Empty<IPlugin>();
		}
	}
}
