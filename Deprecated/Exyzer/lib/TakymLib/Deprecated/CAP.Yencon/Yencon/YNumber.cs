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
	///  数値を保持するノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YNumber : YNode
	{
		/// <summary>
		///  上書きされた場合、このノードが保持する64ビット符号付き整数値を取得または設定します。
		/// </summary>
		public abstract long ValueS64 { get; set; }

		/// <summary>
		///  上書きされた場合、このノードが保持する64ビット符号無し整数値を取得または設定します。
		/// </summary>
		public abstract ulong ValueU64 { get; set; }

		/// <summary>
		///  上書きされた場合、このノードが保持する倍精度浮動小数点数値を取得または設定します。
		/// </summary>
		public abstract double ValueDF { get; set; }

		// .ToString("0.0" + new string('#', 340))

		/// <summary>
		///  上書きされた場合、このノードが保持する10進数値を取得または設定します。
		/// </summary>
		public abstract decimal ValueM { get; set; }

		// .ToString()

		/// <summary>
		///  型'<see cref="CAP.Yencon.YNumber"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しい数値の親セクションまたは親配列です。
		/// </param>
		/// <param name="name">新しい数値の名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		protected YNumber(YNode parent, string name) : base(parent, name) { }
	}
}
