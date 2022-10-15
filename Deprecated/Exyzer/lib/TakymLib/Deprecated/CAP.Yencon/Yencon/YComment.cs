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
	///  コメントを保持するノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YComment : YNode
	{
		/// <summary>
		///  上書きされた場合、可読なメッセージを取得または設定します。
		/// </summary>
		public abstract string? Message { get; set; }

		/// <summary>
		///  型'<see cref="CAP.Yencon.YString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しいコメントの親セクションまたは親配列です。
		/// </param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		protected YComment(YNode parent) : base(parent, string.Empty) { }
	}
}
