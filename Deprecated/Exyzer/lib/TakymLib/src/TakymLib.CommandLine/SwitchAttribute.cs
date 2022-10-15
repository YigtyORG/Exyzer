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
	///  コマンド行引数からの情報を保持するクラスまたは構造体である事を示します。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  クラスまたは構造体の引数無しコンストラクタは公開されている必要があります。
	///  また、読み取り専用構造体と参照構造体は利用できません。
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public sealed class SwitchAttribute : Attribute
	{
		/// <summary>
		///  名前を取得します。
		/// </summary>
		public string? Name { get; }

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.SwitchAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">名前を設定します。</param>
		public SwitchAttribute(string? longName)
		{
			this.Name = longName;
		}
	}
}
