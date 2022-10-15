/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace TakymLib.Extensibility
{
	/// <summary>
	///  <see cref="TakymLib.Extensibility.FeatureModule"/>オブジェクトの状態を表します。
	/// </summary>
	public enum FeatureModuleState
	{
		/// <summary>
		///  無効な状態を表します。
		/// </summary>
		Invalid = 0,

		/// <summary>
		///  初期化処理が未実行である事を表します。
		/// </summary>
		NotInitializedYet = 1,

		/// <summary>
		///  初期化処理が実行中である事を表します。
		/// </summary>
		Initializing = 2,

		/// <summary>
		///  初期化処理が完了した事を表します。
		/// </summary>
		Initialized = 3
	}
}
