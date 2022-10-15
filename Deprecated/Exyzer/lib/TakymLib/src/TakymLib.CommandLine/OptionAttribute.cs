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
	///  コマンド行引数からの情報を保持するプロパティまたは変数である事を示します。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  プロパティまたは変数は公開され読み書き可能である必要があります。
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public sealed class OptionAttribute : Attribute
	{
		/// <summary>
		///  長い名前を取得します。
		/// </summary>
		public string LongName { get; }

		/// <summary>
		///  短い名前を取得します。
		/// </summary>
		public string? ShortName { get; }

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.OptionAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">長い名前を設定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public OptionAttribute(string longName)
		{
			longName.EnsureNotNull();
			this.LongName  = longName;
			this.ShortName = null;
		}

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.OptionAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="longName">長い名前を設定します。</param>
		/// <param name="shortName">短い名前を設定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public OptionAttribute(string longName, string? shortName)
		{
			longName.EnsureNotNull();
			this.LongName  = longName;
			this.ShortName = shortName;
		}
	}
}
