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
	///  空の値を保持するノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YEmpty : YNode
	{
		/// <summary>
		///  型'<see cref="CAP.Yencon.YString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しい空値の親セクションまたは親配列です。
		/// </param>
		/// <param name="name">新しい空値の名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		protected YEmpty(YNode parent, string name) : base(parent, name) { }
	}
}
