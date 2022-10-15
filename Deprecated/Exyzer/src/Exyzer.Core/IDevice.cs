/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace Exyzer
{
	/// <summary>
	///  <see cref="Exyzer"/>の外部装置を提供します。
	/// </summary>
	public interface IDevice
	{
		/// <summary>
		///  現在の外部装置の表示名を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  現在の外部装置に割り振られた一意識別子を取得します。
		/// </summary>
		public Guid Guid { get; }

		/// <summary>
		///  指定された場所から情報を入力します。
		/// </summary>
		/// <param name="address">符号付き32ビット整数で情報の読み取り元を指定します。</param>
		/// <param name="data">読み取った情報を保持する符号付き32ビット整数型の変数を指定します。</param>
		/// <returns>入力処理の実行結果を返します。</returns>
		public IOResult Input(int address, out int data);

		/// <summary>
		///  指定された場所から情報を入力します。
		/// </summary>
		/// <param name="address">符号付き64ビット整数で情報の読み取り元を指定します。</param>
		/// <param name="data">読み取った情報を保持する符号付き64ビット整数型の変数を指定します。</param>
		/// <returns>入力処理の実行結果を返します。</returns>
		public IOResult Input(long address, out long data);

		/// <summary>
		///  指定された場所へ指定された情報を出力します。
		/// </summary>
		/// <param name="address">符号付き32ビット整数で情報の書き込み先を指定します。</param>
		/// <param name="data">符号付き32ビット整数で書き込む情報を指定します。</param>
		/// <returns>出力処理の実行結果を返します。</returns>
		public IOResult Output(int address, int data);

		/// <summary>
		///  指定された場所へ指定された情報を出力します。
		/// </summary>
		/// <param name="address">符号付き64ビット整数で情報の書き込み先を指定します。</param>
		/// <param name="data">符号付き64ビット整数で書き込む情報を指定します。</param>
		/// <returns>出力処理の実行結果を返します。</returns>
		public IOResult Output(long address, long data);
	}
}
