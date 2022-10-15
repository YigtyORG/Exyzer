/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exyzer
{
	/// <summary>
	///  <see cref="Exyzer.IRuntimeEngine"/>に関する情報を提供します。
	/// </summary>
	public interface IRuntimeEngineInfo
	{
		/// <summary>
		///  現在の実行環境の名前を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  現在の実行環境のバージョンを取得します。
		/// </summary>
		public uint Version { get; }

		/// <summary>
		///  現在の実行環境が最新版であるかどうかを示す論理値を取得します。
		/// </summary>
		/// <returns>
		///  最新版である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public bool IsLatest { get; }
	}
}
