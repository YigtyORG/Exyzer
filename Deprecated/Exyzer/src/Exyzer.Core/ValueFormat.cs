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
	///  値の書式を表します。
	/// </summary>
	public enum ValueFormat
	{
		/// <summary>
		///  値の名前を取得する事を表します。
		///  設定する場合は必ず失敗します。
		/// </summary>
		DisplayName,

		/// <summary>
		///  ゼロ埋めされた二進数表現を表します。
		/// </summary>
		Binary,

		/// <summary>
		///  ゼロ埋めされた八進数表現を表します。
		/// </summary>
		Octal,

		/// <summary>
		///  ゼロ埋めされた十進数表現を表します。
		/// </summary>
		Decimal,

		/// <summary>
		///  ゼロ埋めされた十六進数表現を表します。
		/// </summary>
		Hexadecimal,

		/// <summary>
		///  <see langword="UTF-8"/>形式の文字列表現を表します。
		/// </summary>
		Utf8Chars
	}
}
