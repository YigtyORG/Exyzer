/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書の他の全ての要素を含む根要素を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlRoot : XrcdlNode
	{
		/// <summary>
		///  この根要素の全ての直接の子要素を読み取り専用で取得します。
		/// </summary>
		public abstract IReadOnlyList<XrcdlNode> Children { get; }

		internal XrcdlRoot(XrcdlDocument document) : base(document, document) { }

		/// <summary>
		///  現在の根要素と子要素を検証します。
		/// </summary>
		/// <param name="onError">検証に失敗した時に呼び出されます。</param>
		/// <returns>
		///  現在の根要素が有効な<see cref="Exrecodel"/>要素である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </returns>
		public sealed override bool Validate(XrcdlNodeValidationError onError)
		{
			bool result = this.PreValidate(onError);
			int  count  = this.Children.Count;
			for (int i = 0; i < count; ++i) {
				result &= this.Children[i].Validate(onError);
			}
			return result & this.PostValidate(onError);
		}

		/// <summary>
		///  要素の検証の開始時に呼び出されます。
		/// </summary>
		/// <param name="onError">検証に失敗した時に呼び出されます。</param>
		/// <returns>
		///  現在の根要素が有効な<see cref="Exrecodel"/>要素である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </returns>
		protected virtual bool PreValidate(XrcdlNodeValidationError onError)
		{
			return true;
		}

		/// <summary>
		///  要素の検証の終了時に呼び出されます。
		/// </summary>
		/// <param name="onError">検証に失敗した時に呼び出されます。</param>
		/// <returns>
		///  現在の根要素が有効な<see cref="Exrecodel"/>要素である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </returns>
		protected virtual bool PostValidate(XrcdlNodeValidationError onError)
		{
			return true;
		}
	}
}
