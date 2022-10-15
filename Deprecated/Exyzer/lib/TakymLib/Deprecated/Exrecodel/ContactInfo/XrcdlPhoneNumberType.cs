/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exrecodel.ContactInfo
{
	/// <summary>
	///  電話番号の種類を表します。
	/// </summary>
	public enum XrcdlPhoneNumberType
	{
		/// <summary>
		///  不明な種類の電話番号を表します。
		/// </summary>
		Unknown,

		/// <summary>
		///  通話用の固定電話番号または携帯電話番号を表します。
		/// </summary>
		Telephone,

		/// <summary>
		///  模写電送または写真電送用の電話番号を表します。
		/// </summary>
		Facsimile,

		/// <summary>
		///  短文の文字列メッセージの送受信用の電話番号を表します。
		/// </summary>
		ShortMessageService
	}
}
