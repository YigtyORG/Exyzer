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
	///  入出力処理の戻り値を表します。
	/// </summary>
	public enum IOResult
	{
		/// <summary>
		///  無効な戻り値を表します。
		/// </summary>
		Invalid = -1,

		/// <summary>
		///  成功した事を表します。
		/// </summary>
		Success,

		/// <summary>
		///  失敗した事を表します。
		///  原因は不明です。
		/// </summary>
		Failed,

		/// <summary>
		///  範囲外のアドレスを指定した事を表します。
		/// </summary>
		OutOfRange,

		/// <summary>
		///  操作の権限が無く失敗した事を表します。
		/// </summary>
		AccessDenied
	}
}
