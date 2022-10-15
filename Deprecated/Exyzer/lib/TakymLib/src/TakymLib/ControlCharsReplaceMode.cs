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
	///  <see cref="TakymLib.StringExtensions.RemoveControlChars(string, ControlCharsReplaceMode, bool, bool, bool)"/>の動作モードを指定します。
	/// </summary>
	public enum ControlCharsReplaceMode
	{
		/// <summary>
		///  制御文字を全て削除します。
		/// </summary>
		RemoveAll,

		/// <summary>
		///  制御文字を表す文字列へ変換します。
		/// </summary>
		ConvertToText,

		/// <summary>
		///  制御文字を表す図形へ変換します。
		/// </summary>
		ConvertToIcon,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>と同等です。
		/// </summary>
		ConvertToString = ConvertToText,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>と同等です。
		/// </summary>
		ConvertToSymbol = ConvertToText,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToText"/>と同等です。
		/// </summary>
		ConvertToMark = ConvertToText,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToIcon"/>と同等です。
		/// </summary>
		ConvertToPictogram = ConvertToIcon,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToIcon"/>と同等です。
		/// </summary>
		ConvertToPicture = ConvertToIcon,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToIcon"/>と同等です。
		/// </summary>
		ConvertToLogo = ConvertToIcon,

		/// <summary>
		///  <see cref="TakymLib.ControlCharsReplaceMode.ConvertToIcon"/>と同等です。
		/// </summary>
		ConvertToEmoji = ConvertToIcon,
	}
}
