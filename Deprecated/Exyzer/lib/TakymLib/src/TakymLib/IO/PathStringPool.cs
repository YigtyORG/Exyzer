/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.IO;

namespace TakymLib.IO
{
	/// <summary>
	///  パス文字列をキャッシュします。
	/// </summary>
	public static class PathStringPool
	{
		private static readonly ConcurrentDictionary<string, PathString> _cache;

		static PathStringPool()
		{
			_cache = new();
		}

		/// <summary>
		///  現在の作業ディレクトリを指し示すパス文字列を取得します。
		/// </summary>
		/// <returns>キャッシュされたパス文字列です。</returns>
		public static PathString Get()
		{
			return Get(Environment.CurrentDirectory);
		}

		/// <summary>
		///  現在の作業ディレクトリを指し示すパス文字列を取得します。
		/// </summary>
		/// <param name="path1">文字列型の分割されたパス文字列です。</param>
		/// <param name="path2">文字列型の分割されたパス文字列です。</param>
		/// <returns>キャッシュされたパス文字列です。</returns>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path1"/>または<paramref name="path2"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		/// <exception cref="System.OverflowException"/>
		public static PathString Get(string path1, string path2)
		{
			return Get(Path.Combine(path1, path2));
		}

		/// <summary>
		///  現在の作業ディレクトリを指し示すパス文字列を取得します。
		/// </summary>
		/// <param name="path1">文字列型の分割されたパス文字列です。</param>
		/// <param name="path2">文字列型の分割されたパス文字列です。</param>
		/// <param name="path3">文字列型の分割されたパス文字列です。</param>
		/// <returns>キャッシュされたパス文字列です。</returns>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path1"/>、<paramref name="path2"/>、または<paramref name="path3"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		/// <exception cref="System.OverflowException"/>
		public static PathString Get(string path1, string path2, string path3)
		{
			return Get(Path.Combine(path1, path2, path3));
		}

		/// <summary>
		///  現在の作業ディレクトリを指し示すパス文字列を取得します。
		/// </summary>
		/// <param name="path1">文字列型の分割されたパス文字列です。</param>
		/// <param name="path2">文字列型の分割されたパス文字列です。</param>
		/// <param name="path3">文字列型の分割されたパス文字列です。</param>
		/// <param name="path4">文字列型の分割されたパス文字列です。</param>
		/// <returns>キャッシュされたパス文字列です。</returns>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path1"/>、<paramref name="path2"/>、<paramref name="path3"/>、または<paramref name="path4"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		/// <exception cref="System.OverflowException"/>
		public static PathString Get(string path1, string path2, string path3, string path4)
		{
			return Get(Path.Combine(path1, path2, path3, path4));
		}

		/// <summary>
		///  現在の作業ディレクトリを指し示すパス文字列を取得します。
		/// </summary>
		/// <param name="paths">文字列型の分割されたパス文字列を含む配列です。</param>
		/// <returns>キャッシュされたパス文字列です。</returns>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="paths"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		/// <exception cref="System.OverflowException"/>
		public static PathString Get(params string[] paths)
		{
			return Get(Path.Combine(paths));
		}

		/// <summary>
		///  指定されたパスを指し示すパス文字列を取得します。
		/// </summary>
		/// <param name="path">文字列型のパス文字列です。</param>
		/// <returns>キャッシュされたパス文字列です。</returns>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		/// <exception cref="System.OverflowException"/>
#pragma warning disable TakymLib_PathString_ctor // 型またはメンバーが旧型式です
		public static PathString Get(string path)
		{
			path.EnsureNotNull();
			return _cache.GetOrAdd(path, path => new(path));
		}
#pragma warning restore TakymLib_PathString_ctor // 型またはメンバーが旧型式です

		/// <summary>
		///  キャッシュされた全てのパス文字列を削除します。
		/// </summary>
		public static void Clear()
		{
			_cache.Clear();

			// GC.Collect();
			// GC.WaitForPendingFinalizers();
			// GC.Collect();
		}
	}
}
