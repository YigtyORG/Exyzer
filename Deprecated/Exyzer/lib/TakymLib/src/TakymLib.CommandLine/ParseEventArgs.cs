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
	///  コマンド行引数の解析時イベントのイベントデータを表します。
	/// </summary>
	public class ParseEventArgs : EventArgs
	{
		/// <summary>
		///  スイッチ名を取得します。
		/// </summary>
		public string SwitchName { get; }

		/// <summary>
		///  オプション名を取得します。
		/// </summary>
		public string OptionName { get; }

		/// <summary>
		///  文字列配列を取得します。
		/// </summary>
		public string[] Values { get; }

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.ParseEventArgs"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="switchName">スイッチ名です。</param>
		/// <param name="optionName">オプション名です。</param>
		/// <param name="values">文字列配列です。</param>
		public ParseEventArgs(string switchName, string optionName, string[] values)
		{
			this.SwitchName = switchName;
			this.OptionName = optionName;
			this.Values     = values;
		}
	}
}
