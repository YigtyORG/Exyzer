/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;

namespace Exrecodel.Nodes
{
	/// <summary>
	///  <see cref="Exrecodel.Nodes.XrcdlTerm"/>の一覧です。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlTermList : XrcdlNode
	{
		/// <summary>
		///  この要素に格納されている全ての用語を読み取り専用で取得します。
		/// </summary>
		public abstract IReadOnlyList<XrcdlTerm> Children { get; }

		internal XrcdlTermList(XrcdlDocument document, XrcdlNode parent) : base(document, parent) { }
	}
}
