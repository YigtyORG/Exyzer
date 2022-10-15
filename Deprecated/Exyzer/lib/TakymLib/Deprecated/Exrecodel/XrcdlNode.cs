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
	///  <see cref="Exrecodel"/>文書の要素を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlNode : IXrcdlElement
	{
		/// <summary>
		///  この要素の名前を取得または設定します。
		/// </summary>
		public abstract string Name { get; set; }

		/// <summary>
		///  この要素の親要素を取得します。
		/// </summary>
		public IXrcdlElement Parent { get; }

		/// <summary>
		///  この要素と関連付けられている文書を取得します。
		/// </summary>
		public XrcdlDocument Document { get; }

		/// <summary>
		///  この要素が根要素かどうかを表す論理値を取得します。
		/// </summary>
		public bool IsRoot => this.Document == this.Parent && this is XrcdlRoot;

		internal XrcdlNode(XrcdlDocument document, IXrcdlElement parent)
		{
			this.Document = document;
			this.Parent   = parent;
		}

		/// <summary>
		///  現在の要素の変換処理を行うオブジェクトを取得します。
		/// </summary>
		/// <returns><see cref="Exrecodel.IXrcdlConverter"/>へ変換可能なオブジェクトです。</returns>
		public abstract IXrcdlConverter GetConverter();

		/// <summary>
		///  現在の要素と子要素を検証します。
		/// </summary>
		/// <param name="onError">検証に失敗した時に呼び出されます。</param>
		/// <returns>
		///  現在の要素が有効な<see cref="Exrecodel"/>要素である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </returns>
		public abstract bool Validate(XrcdlNodeValidationError onError);

		/// <summary>
		///  現在の要素と子要素を検証します。
		/// </summary>
		/// <returns>
		///  現在の要素が有効な<see cref="Exrecodel"/>要素である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </returns>
		public bool Validate()
		{
			return this.Validate((_, _) => { });
		}
	}
}
