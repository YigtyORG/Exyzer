/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exyzer
{
	/// <summary>
	///  <see cref="Exyzer"/>の処理装置を提供します。
	/// </summary>
	public interface IProcessor
	{
		/// <summary>
		///  実行を開始します。
		/// </summary>
		public void RunStart();

		/// <summary>
		///  次の処理を一つだけ実行します。
		/// </summary>
		public void RunNext();
	}
}
