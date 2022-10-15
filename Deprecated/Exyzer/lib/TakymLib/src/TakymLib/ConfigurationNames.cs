/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace TakymLib
{
	/// <summary>
	///  ビルド構成(<see cref="TakymLib.VersionInfo.Configuration"/>)の名前を取得します。
	/// </summary>
	public static class ConfigurationNames
	{
		/// <summary>
		///  デバッグ(実験)ビルドを表します。
		/// </summary>
		public const string Debug = nameof(Debug);

		/// <summary>
		///  リリース(公開)ビルドを表します。
		/// </summary>
		public const string Release = nameof(Release);
	}
}
