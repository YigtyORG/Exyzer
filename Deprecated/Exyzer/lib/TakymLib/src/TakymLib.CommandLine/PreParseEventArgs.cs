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
	///  コマンド行引数の解析前イベントのイベントデータを表します。
	/// </summary>
	public class PreParseEventArgs : EventArgs
	{
		/// <summary>
		///  子コマンドを取得します。
		/// </summary>
		public string? SubCommand { get; }

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.PreParseEventArgs"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="subCommand">子コマンドです。</param>
		public PreParseEventArgs(string? subCommand)
		{
			this.SubCommand = subCommand;
		}
	}
}
