/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exrecodel.Nodes
{
	/// <summary>
	///  用語を定義します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlTerm : XrcdlNode
	{
		internal XrcdlTerm(XrcdlDocument document, XrcdlTermList parent) : base(document, parent) { }
	}
}
