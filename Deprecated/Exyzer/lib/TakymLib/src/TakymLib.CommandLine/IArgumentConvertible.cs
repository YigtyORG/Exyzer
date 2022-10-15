/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace TakymLib.CommandLine
{
	/// <summary>
	///  コマンド行引数からの変換可能である事を示します。
	/// </summary>
	/// <remarks>
	///  <see cref="TakymLib.CommandLine.OptionAttribute"/>属性を付加した変数の型で実装します。
	/// </remarks>
	public interface IArgumentConvertible
	{
		/// <summary>
		///  指定された文字列から現在のインスタンスに値を設定します。
		/// </summary>
		/// <param name="value">文字列です。</param>
		public void FromString(string value);

		/// <summary>
		///  指定された文字列配列から現在のインスタンスに値を設定します。
		/// </summary>
		/// <param name="array">文字列配列です。</param>
		public void FromStringArray(string[] array);
	}
}
