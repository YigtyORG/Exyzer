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
	///  論理値を保持するノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YBoolean : YNode
	{
		/// <summary>
		///  上書きされた場合、このノードが保持する値を取得または設定します。
		/// </summary>
		public abstract bool Value { get; set; }

		/// <summary>
		///  型'<see cref="CAP.Yencon.YBoolean"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しい論理値の親セクションまたは親配列です。
		/// </param>
		/// <param name="name">新しい論理値の名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		protected YBoolean(YNode parent, string name) : base(parent, name) { }
	}
}
