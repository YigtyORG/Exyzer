/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace CAP.Yencon
{
	/// <summary>
	///  リンク文字列を保持するノードを表します。
	///  このノードは他のノードを参照します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YLink : YNode
	{
		/// <summary>
		///  上書きされた場合、このノードが保持する値を取得または設定します。
		/// </summary>
		public abstract string? Value { get; set; }

		/// <summary>
		///  型'<see cref="CAP.Yencon.YString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しいリンク文字列の親セクションまたは親配列です。
		/// </param>
		/// <param name="name">新しいリンク文字列の名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		protected YLink(YNode parent, string name) : base(parent, name) { }

		/// <summary>
		///  このリンク文字列が参照しているノードを取得します。
		/// </summary>
		/// <returns>
		///  このリンク文字列からノードを取得できた場合はそのノードを表すオブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		public YNode? GetNode()
		{
			return this.GetDocument()?.GetNodeByLink(this.Value ?? string.Empty);
		}
	}
}
