/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel.XrcdlNode"/>の検証に失敗した時に呼び出されます。
	/// </summary>
	/// <param name="sender">検証に失敗したオブジェクトです。</param>
	/// <param name="message">失敗の内容を表す翻訳済みのメッセージです。</param>
	public delegate void XrcdlNodeValidationError(XrcdlNode sender, string message);
}
