/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Text;

namespace TakymLib.CommandLine
{
	/// <summary>
	///  コマンド行引数の説明書を提供します。
	/// </summary>
	/// <remarks>
	///  <see cref="TakymLib.CommandLine.SwitchAttribute"/>属性を付加したクラスまたは構造体で実装します。
	/// </remarks>
	public interface IHelpProvider
	{
		/// <summary>
		///  翻訳済みの説明文を指定された文字列バッファへ書き込みます。
		/// </summary>
		/// <param name="sb">書き込み先の文字列バッファです。</param>
		/// <param name="optionName">
		///  オプション名です。
		///  この引数が<see langword="null"/>の場合は、スイッチ全体の説明を提供します。
		///  有効なオプション名が渡された場合は、そのオプションの説明を提供します。
		/// </param>
		public void WriteHelp(StringBuilder sb, string? optionName = null);
	}
}
