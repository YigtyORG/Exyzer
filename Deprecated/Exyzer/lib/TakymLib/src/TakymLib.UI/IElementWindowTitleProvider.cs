/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace TakymLib.UI
{
	/// <summary>
	///  <see cref="TakymLib.UI.ElementWindow"/>に題名を提供します。
	/// </summary>
	public interface IElementWindowTitleProvider
	{
		/// <summary>
		///  題名を取得または設定します。
		/// </summary>
		public string Title { get; }
	}
}
