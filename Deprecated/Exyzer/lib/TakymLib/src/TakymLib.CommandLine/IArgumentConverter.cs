/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace TakymLib.CommandLine
{
	/// <summary>
	///  コマンド行引数のオプションをオブジェクトへ変換する機能を提供します。
	/// </summary>
	public interface IArgumentConverter
	{
		/// <summary>
		///  指定された文字列配列を指定された型へ変換します。
		/// </summary>
		/// <param name="args">文字列配列です。</param>
		/// <param name="target">変換先の型です。</param>
		/// <returns>文字列配列から変換された新しいオブジェクトインスタンスです。</returns>
		public object? ConvertToObject(string[] args, Type target);
	}
}
