/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using TakymLib.IO;

namespace TakymLib.Logging
{
	/// <summary>
	///  ログファイルの名前を作成します。
	/// </summary>
	public static class LogFileName
	{
		/// <summary>
		///  ログファイルの名前を生成します。
		/// </summary>
		/// <param name="dt">作成日時です。</param>
		/// <param name="proc">プロセスを表すオブジェクトです。</param>
		/// <param name="tag">ファイル名に付加するタグです。既定値は<see langword="null"/>です。</param>
		/// <param name="useLongName">
		///  長い形式を利用する場合は<see langword="true"/>、
		///  短い形式を利用する場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="false"/>です。
		/// </param>
		/// <returns>生成されたログファイルの名前を表す文字列です。</returns>
		public static string Create(DateTime dt, Process? proc, string? tag = null, bool useLongName = true)
		{
			string dtstring;
			if (useLongName) {
				dtstring = dt.ToString("yyyy-MM-dd_HH-mm-ss+fffffff");
			} else {
				dtstring = dt.ToString("yyyyMMddHHmmssfffffff");
			}
			int pid = proc?.Id ?? 0;
			if (tag is null) {
				return $"{dtstring}.[{pid}].log";
			} else {
				return $"{dtstring}.[{pid}].{tag}.log";
			}
		}

		/// <summary>
		///  ログファイルの名前を生成します。
		/// </summary>
		/// <param name="tag">ファイル名に付加するタグです。</param>
		/// <returns>生成されたログファイルの名前を表す文字列です。</returns>
		public static string Create(string? tag)
		{
			return Create(DateTime.Now, Process.GetCurrentProcess(), tag);
		}

		/// <summary>
		///  ログファイルの名前を生成します。
		/// </summary>
		/// <returns>生成されたログファイルの名前を表す文字列です。</returns>
		public static string Create()
		{
			return Create(DateTime.Now, Process.GetCurrentProcess());
		}

		/// <summary>
		///  ログファイルのパス文字列を生成します。
		/// </summary>
		/// <param name="dt">作成日時です。</param>
		/// <param name="proc">プロセスを表すオブジェクトです。</param>
		/// <param name="tag">ファイル名に付加するタグです。既定値は<see langword="null"/>です。</param>
		/// <param name="useLongName">
		///  長い形式を利用する場合は<see langword="true"/>、
		///  短い形式を利用する場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="false"/>です。
		/// </param>
		/// <returns>生成されたログファイルのパス文字列です。</returns>
		public static PathString CreatePath(DateTime dt, Process proc, string? tag = null, bool useLongName = true)
		{
			return PathStringPool.Get(Create(dt, proc, tag, useLongName));
		}

		/// <summary>
		///  ログファイルのパス文字列を生成します。
		/// </summary>
		/// <param name="dir">基底ディレクトリです。</param>
		/// <param name="dt">作成日時です。</param>
		/// <param name="proc">プロセスを表すオブジェクトです。</param>
		/// <param name="tag">ファイル名に付加するタグです。既定値は<see langword="null"/>です。</param>
		/// <param name="useLongName">
		///  長い形式を利用する場合は<see langword="true"/>、
		///  短い形式を利用する場合は<see langword="false"/>を指定します。
		///  既定値は<see langword="false"/>です。
		/// </param>
		/// <returns>生成されたログファイルのパス文字列です。</returns>
		public static PathString CreatePath(PathString dir, DateTime dt, Process proc, string? tag = null, bool useLongName = true)
		{
			dir.EnsureNotNull();
			return dir.Combine(Create(dt, proc, tag, useLongName));
		}

		/// <summary>
		///  ログファイルのパス文字列を生成します。
		/// </summary>
		/// <returns>生成されたログファイルのパス文字列です。</returns>
		public static PathString CreatePath()
		{
			return PathStringPool.Get(Create());
		}

		/// <summary>
		///  ログファイルのパス文字列を生成します。
		/// </summary>
		/// <param name="tag">ファイル名に付加するタグです。</param>
		/// <returns>生成されたログファイルのパス文字列です。</returns>
		public static PathString CreatePath(string? tag)
		{
			return PathStringPool.Get(Create(tag));
		}

		/// <summary>
		///  ログファイルのパス文字列を生成します。
		/// </summary>
		/// <param name="dir">基底ディレクトリです。</param>
		/// <returns>生成されたログファイルのパス文字列です。</returns>
		public static PathString CreatePath(PathString dir)
		{
			dir.EnsureNotNull();
			return dir.Combine(Create());
		}

		/// <summary>
		///  ログファイルのパス文字列を生成します。
		/// </summary>
		/// <param name="dir">基底ディレクトリです。</param>
		/// <param name="tag">ファイル名に付加するタグです。</param>
		/// <returns>生成されたログファイルのパス文字列です。</returns>
		public static PathString CreatePath(PathString dir, string? tag)
		{
			dir.EnsureNotNull();
			return dir.Combine(Create(tag));
		}
	}
}
