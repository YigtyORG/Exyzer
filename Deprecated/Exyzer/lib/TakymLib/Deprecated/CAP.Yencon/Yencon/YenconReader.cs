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
	///  ヱンコンファイルを読み込むリーダーを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YenconReader : CapStream
	{
		/// <summary>
		///  上書きされた場合、現在のノードを取得します。
		/// </summary>
		/// <returns>
		///  ノード全体の読み込みが完了した場合はそのノード、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		public abstract YNode? Current { get; }

		/// <summary>
		///  上書きされた場合、字句を現在のストリーム位置から読み取ります。
		/// </summary>
		/// <returns>
		///  読み取った字句を表すオブジェクトです。
		///  ストリームの終端まで読み進めた場合は<see langword="null"/>を返します。
		/// </returns>
		public abstract YToken? ReadToken();

		/// <summary>
		///  ノードを現在のストリーム位置から読み取ります。
		/// </summary>
		/// <returns>
		///  読み取ったノードを表すオブジェクトです。
		///  ストリームの終端まで読み進めた場合は<see langword="null"/>を返します。
		/// </returns>
		public YNode? ReadNode()
		{
			while (this.ReadToken() is not null) {
				if (this.Current is not null) {
					return this.Current;
				}
			}
			return null;
		}
	}
}
