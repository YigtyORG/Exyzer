/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using TakymLib.Properties;

namespace TakymLib.IO
{
	/// <summary>
	///  パス文字列を表します。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  <see cref="System.IO.Path"/>クラスと併用してください。
	/// </remarks>
	[Serializable()]
	[TypeConverter(typeof(PathStringConverter))]
	public sealed class PathString :
		ISerializable,
		IFormattable,
		IEquatable<PathString?>,
		IEquatable<string?>,
		IComparable,
		IComparable<PathString?>,
		IComparable<string?>
	{
		private const    string          ROOT_PATH              = "/";
		private const    string          DEFAULT_SEARCH_PATTERN = "*";
		private readonly string          _org_path;
		private readonly string          _path;
		private readonly Uri             _uri;
		private          FileSystemInfo? _fsinfo;
		private          DriveInfo?      _dinfo;
		private          PathString?     _base_path;

		/// <summary>
		///  基底のパス文字列を取得します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathString.GetDirectoryName"/>を内部キャッシュします。
		/// </remarks>
		public PathString? BasePath
		{
			get
			{
				if (_base_path is null) {
					_base_path = this.GetDirectoryName();
				}
				return _base_path;
			}
		}

		/// <summary>
		///  現在のパス文字列がルートディレクトリを表しているかどうかを判定します。
		/// </summary>
		public bool IsRoot => _path == Path.GetDirectoryName(_path);

		/// <summary>
		///  現在のパス文字列が実際に存在し、ドライブである場合は<see langword="true"/>を返します。
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDrive => this.IsRoot && this.IsDirectory;

		/// <summary>
		///  現在のパス文字列が実際に存在し、ディレクトリである場合は<see langword="true"/>を返します。
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDirectory => Directory.Exists(_path);

		/// <summary>
		///  現在のパス文字列が実際に存在し、ファイルである場合は<see langword="true"/>を返します。
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsFile => File.Exists(_path);

		/// <summary>
		///  現在のパス文字列が実際に存在する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool Exists => this.IsDirectory || this.IsFile;

		/// <summary>
		///  型'<see cref="TakymLib.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathStringPool.Get()"/>を利用してキャッシュされたパス文字列を取得します。
		/// </remarks>
		[Obsolete("不必要なインスタンスを生成しています。代わりに PathStringPool を利用してください。", DiagnosticId = "TakymLib_PathString_ctor")]
		public PathString() : this(Environment.CurrentDirectory) { }

		/// <summary>
		///  型'<see cref="TakymLib.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathStringPool.Get(string, string)"/>を利用してキャッシュされたパス文字列を取得します。
		/// </remarks>
		/// <param name="path1">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <param name="path2">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path1"/>または<paramref name="path2"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		[Obsolete("不必要なインスタンスを生成しています。代わりに PathStringPool を利用してください。", DiagnosticId = "TakymLib_PathString_ctor")]
		public PathString(string path1, string path2) : this(Path.Combine(path1, path2)) { }

		/// <summary>
		///  型'<see cref="TakymLib.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathStringPool.Get(string, string, string)"/>を利用してキャッシュされたパス文字列を取得します。
		/// </remarks>
		/// <param name="path1">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <param name="path2">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <param name="path3">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path1"/>、<paramref name="path2"/>、または<paramref name="path3"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		[Obsolete("不必要なインスタンスを生成しています。代わりに PathStringPool を利用してください。", DiagnosticId = "TakymLib_PathString_ctor")]
		public PathString(string path1, string path2, string path3) : this(Path.Combine(path1, path2, path3)) { }

		/// <summary>
		///  型'<see cref="TakymLib.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathStringPool.Get(string, string, string, string)"/>を利用してキャッシュされたパス文字列を取得します。
		/// </remarks>
		/// <param name="path1">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <param name="path2">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <param name="path3">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <param name="path4">
		///  新しいインスタンスに設定する分割されたパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path1"/>、<paramref name="path2"/>、<paramref name="path3"/>、または<paramref name="path4"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		[Obsolete("不必要なインスタンスを生成しています。代わりに PathStringPool を利用してください。", DiagnosticId = "TakymLib_PathString_ctor")]
		public PathString(string path1, string path2, string path3, string path4) : this(Path.Combine(path1, path2, path3, path4)) { }

		/// <summary>
		///  型'<see cref="TakymLib.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathStringPool.Get(string[])"/>を利用してキャッシュされたパス文字列を取得します。
		/// </remarks>
		/// <param name="paths">
		///  新しいインスタンスに設定する分割されたパス文字列を含む配列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="paths"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		[Obsolete("不必要なインスタンスを生成しています。代わりに PathStringPool を利用してください。", DiagnosticId = "TakymLib_PathString_ctor")]
		public PathString(params string[] paths) : this(Path.Combine(paths)) { }

		/// <summary>
		///  型'<see cref="TakymLib.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.IO.PathStringPool.Get(string)"/>を利用してキャッシュされたパス文字列を取得します。
		/// </remarks>
		/// <param name="path">
		///  新しいインスタンスに設定するパス文字列を指定します。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		[Obsolete("不必要なインスタンスを生成しています。代わりに PathStringPool を利用してください。", DiagnosticId = "TakymLib_PathString_ctor")]
		public PathString(string path)
		{
			path.EnsureNotNull();
			_org_path = path;
			try {
				_path = Path.GetFullPath(path);
				if (3 < _path.Length && _path.EndsWith(Path.DirectorySeparatorChar.ToString())) {
					_path = _path.Remove(_path.Length - 1);
				}
				_uri = new Uri("file:///" + _path);
			} catch (ArgumentException ae) {
				throw new InvalidPathFormatException(path, ae);
			} catch (NotSupportedException nse) {
				throw new InvalidPathFormatException(path, nse);
			} catch (PathTooLongException ptle) {
				throw new InvalidPathFormatException(path, ptle);
			} catch (UriFormatException ufe) {
				throw new InvalidPathFormatException(path, ufe);
			}
		}

#pragma warning disable TakymLib_PathString_ctor // 型またはメンバーが旧型式です
		private PathString(SerializationInfo info, StreamingContext context)
			: this(info.GetString("_")!) { }
#pragma warning restore TakymLib_PathString_ctor // 型またはメンバーが旧型式です

		/// <summary>
		///  現在のパス文字列を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトを指定します。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Runtime.Serialization.SerializationException"/>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.EnsureNotNull();
			info.AddValue("_", _org_path);
		}

		/// <summary>
		///  指定されたパスを現在のパスと結合します。
		/// </summary>
		/// <param name="path">結合するパス文字列を指定します。</param>
		/// <returns>結合された新しいパス文字列、または、<paramref name="path"/>が空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(string path)
		{
			if (string.IsNullOrEmpty(path)) {
				return this;
			}
			return PathStringPool.Get(Path.Combine(_path, path!));
		}

		/// <summary>
		///  指定された2つのパスを現在のパスと結合します。
		/// </summary>
		/// <param name="path1">結合する1つ目のパス文字列を指定します。</param>
		/// <param name="path2">結合する2つ目のパス文字列を指定します。</param>
		/// <returns>結合された新しいパス文字列、または、指定された全てのパスが空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(string path1, string path2)
		{
			if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2)) {
				return this;
			}
			return PathStringPool.Get(Path.Combine(_path, path1 ?? string.Empty, path2 ?? string.Empty));
		}

		/// <summary>
		///  指定された3つのパスを現在のパスと結合します。
		/// </summary>
		/// <param name="path1">結合する1つ目のパス文字列を指定します。</param>
		/// <param name="path2">結合する2つ目のパス文字列を指定します。</param>
		/// <param name="path3">結合する3つ目のパス文字列を指定します。</param>
		/// <returns>結合された新しいパス文字列、または、指定された全てのパスが空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(string path1, string path2, string path3)
		{
			if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2) && string.IsNullOrEmpty(path3)) {
				return this;
			}
			return PathStringPool.Get(Path.Combine(_path, path1 ?? string.Empty, path2 ?? string.Empty, path3 ?? string.Empty));
		}

		/// <summary>
		///  複数のパスを一つのパスに結合します。
		/// </summary>
		/// <param name="paths">結合する複数のパス文字列を指定します。</param>
		/// <returns>結合された新しいパス文字列、または、<paramref name="paths"/>が空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(params string?[]? paths)
		{
			if (paths is null || paths.Length == 0) {
				return this;
			} else {
				var paths2 = new List<string>(paths.Length + 1);
				paths2.Add(_path);
				for (int i = 0; i < paths.Length; ++i) {
					if (!string.IsNullOrEmpty(paths[i])) {
						paths2.Add(paths[i]!);
					}
				}
				return PathStringPool.Get(Path.Combine(paths2.ToArray()));
			}
		}

		/// <summary>
		///  現在のパス文字列からディレクトリ情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列のディレクトリ情報を返します。
		///  ディレクトリ情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.Security.SecurityException" />
		public PathString? GetDirectoryName()
		{
			string? baseDir = Path.GetDirectoryName(_path);
			if (string.IsNullOrEmpty(baseDir)) {
				return null;
			}
			return PathStringPool.Get(baseDir);
		}

		/// <summary>
		///  現在のパス文字列からファイル名情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列の拡張子を含むファイル名を返します。
		///  ファイル名情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		public string? GetFileName()
		{
			string? fname = Path.GetFileName(_path);
			if (string.IsNullOrEmpty(fname)) {
				return null;
			}
			return fname;
		}

		/// <summary>
		///  現在のパス文字列から拡張子を除くファイル名情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列の拡張子を含まないファイル名を返します。
		///  ファイル名情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		public string? GetFileNameWithoutExtension()
		{
			string? fname = Path.GetFileNameWithoutExtension(_path);
			if (string.IsNullOrEmpty(fname)) {
				return null;
			}
			return fname;
		}

		/// <summary>
		///  現在のパス文字列から拡張子情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列のピリオド付きの拡張子情報を返します。
		///  拡張子情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		public string? GetExtension()
		{
			string? ext = Path.GetExtension(_path);
			if (string.IsNullOrEmpty(ext)) {
				return null;
			}
			return ext;
		}

		/// <summary>
		///  パス文字列の拡張子を含むファイル名を変更します。
		/// </summary>
		/// <param name="filename">変更後の拡張子を含むファイル名、または、親ディレクトリを取得する場合は空値を指定します。</param>
		/// <returns>
		///  ファイル名が変更されたパス文字列、または、
		///  ファイル名が変更されなかった場合は現在のインスタンスを返します。
		///  親ディレクトリの情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString? ChangeFileName(string? filename)
		{
			var dir = this.GetDirectoryName();
			if (dir is null) {
				return null;
			} else if (string.IsNullOrEmpty(filename)) {
				return dir;
			} else {
				var newpath = dir + filename;
				if (newpath == this) {
					return this;
				} else {
					return newpath;
				}
			}
		}

		/// <summary>
		///  パス文字列の拡張子を変更します。
		/// </summary>
		/// <param name="extension">変更後の拡張子、または、拡張子を削除する場合は空値を指定します。</param>
		/// <returns>
		///  拡張子が変更されたパス文字列、または、
		///  拡張子が変更されなかった場合は現在のインスタンスを返します。
		/// </returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.Security.SecurityException" />
		public PathString ChangeExtension(string? extension)
		{
			string newpath = Path.ChangeExtension(_path, extension);
			if (newpath == _path) {
				return this;
			}
			return PathStringPool.Get(newpath);
		}

		/// <summary>
		///  現在のパス文字列の拡張子を変更し実際に存在しないパス文字列を取得します。
		/// </summary>
		/// <returns>新しい実際にファイルまたはディレクトリが存在しないパス文字列です。</returns>
		/// <exception cref="System.Security.SecurityException" />
		public PathString EnsureNotFound()
		{
			int i = 0;
			var path = this;
			string? ext = this.GetExtension();
			while (path.Exists) {
				path = this.ChangeExtension(++i + ext);
			}
			return path;
		}

		/// <summary>
		///  現在のパス文字列からルートディレクトリ情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列のルートディレクトリ情報を返します。
		///  ルートディレクトリ情報が存在しない場合は<see langword="null"/>を返します。
		///  現在のパス文字列がルートディレクトリを指し示す場合は現在のインスタンスを返します。
		/// </returns>
		/// <exception cref="System.Security.SecurityException" />
		public PathString? GetRootPath()
		{
			string? root = Path.GetPathRoot(_path);
			if (string.IsNullOrEmpty(root)) {
				return null;
			}
			if (root == _path) {
				return this;
			}
			return PathStringPool.Get(root);
		}

		/// <summary>
		///  現在の作業ディレクトリを基にした相対パスを取得します。
		/// </summary>
		/// <returns>現在のパスへの相対パスを表す文字列です。</returns>
		public string? GetRelativePath()
		{
			return this.GetRelativePath(PathStringPool.Get());
		}

		/// <summary>
		///  指定したパスを基にした相対パスを取得します。
		/// </summary>
		/// <param name="relativeTo">相対パスの基底となる絶対パスを指定します。</param>
		/// <returns>現在のパスへの相対パスを表す文字列です。</returns>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="relativeTo"/>が<see langword="null"/>に設定されています。
		/// </exception>
		public string? GetRelativePath(PathString relativeTo)
		{
			relativeTo.EnsureNotNull();
			return Path.GetRelativePath(relativeTo._path, _path);
		}

		/// <summary>
		///  現在のディレクトリからディレクトリパスとファイルパスの配列を取得します。
		/// </summary>
		/// <returns>
		///  現在のパスが有効なディレクトリを指し示している場合は配列、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.UnauthorizedAccessException"/>
		/// <exception cref="System.Security.SecurityException"/>
		public PathString[]? GetEntryArray()
		{
			return this.GetEntries()?.ToArray();
		}

		/// <summary>
		///  現在のディレクトリからディレクトリパスとファイルパスの列挙体を取得します。
		/// </summary>
		/// <param name="searchPattern">ファイルとディレクトリの検索に利用するパターン文字列を指定します。</param>
		/// <returns>
		///  現在のパスが有効なディレクトリを指し示している場合は列挙体オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.UnauthorizedAccessException"/>
		/// <exception cref="System.Security.SecurityException"/>
		public FileSystemEntryEnumerator? GetEntries(string searchPattern = DEFAULT_SEARCH_PATTERN)
		{
			if (this.IsDirectory) {
				searchPattern ??= DEFAULT_SEARCH_PATTERN;
				return new(Directory.EnumerateFileSystemEntries(_path, searchPattern));
			} else {
				return null;
			}
		}

		/// <summary>
		///  現在のディレクトリからディレクトリパスとファイルパスの列挙体を取得します。
		/// </summary>
		/// <param name="searchPattern">ファイルとディレクトリの検索に利用するパターン文字列を指定します。</param>
		/// <param name="searchOption">検索設定を指定します。</param>
		/// <returns>
		///  現在のパスが有効なディレクトリを指し示している場合は列挙体オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.UnauthorizedAccessException"/>
		/// <exception cref="System.Security.SecurityException"/>
		public FileSystemEntryEnumerator? GetEntries(string searchPattern, SearchOption searchOption)
		{
			if (this.IsDirectory) {
				searchPattern ??= DEFAULT_SEARCH_PATTERN;
				return new(Directory.EnumerateFileSystemEntries(_path, searchPattern, searchOption));
			} else {
				return null;
			}
		}

		/// <summary>
		///  現在のディレクトリからディレクトリパスとファイルパスの列挙体を取得します。
		/// </summary>
		/// <param name="searchPattern">ファイルとディレクトリの検索に利用するパターン文字列を指定します。</param>
		/// <param name="enumerationOptions">検索設定を指定します。</param>
		/// <returns>
		///  現在のパスが有効なディレクトリを指し示している場合は列挙体オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.UnauthorizedAccessException"/>
		/// <exception cref="System.Security.SecurityException"/>
		public FileSystemEntryEnumerator? GetEntries(string searchPattern, EnumerationOptions enumerationOptions)
		{
			enumerationOptions.EnsureNotNull();
			if (this.IsDirectory) {
				searchPattern ??= DEFAULT_SEARCH_PATTERN;
				return new(Directory.EnumerateFileSystemEntries(_path, searchPattern, enumerationOptions));
			} else {
				return null;
			}
		}

		/// <summary>
		///  現在のパスのドライブ情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパスが有効なドライブを指し示している場合は<see cref="System.IO.DriveInfo"/>オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.IO.IOException"/>
		public DriveInfo? GetDriveInfo()
		{
			this.EnsureFileSystemInfo();
			return _dinfo;
		}

		/// <summary>
		///  現在のパスのディレクトリ情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパスが有効なディレクトリを指し示している場合は<see cref="System.IO.DirectoryInfo"/>オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.IO.IOException"/>
		public DirectoryInfo? GetDirectoryInfo()
		{
			this.EnsureFileSystemInfo();
			return _fsinfo as DirectoryInfo;
		}

		/// <summary>
		///  現在のパスのファイル情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパスが有効なファイルを指し示している場合は<see cref="System.IO.FileInfo"/>オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.IO.IOException"/>
		public FileInfo? GetFileInfo()
		{
			this.EnsureFileSystemInfo();
			return _fsinfo as FileInfo;
		}

		/// <summary>
		///  現在のパスのファイルシステム情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパスが有効なファイルまたはディレクトリを指し示している場合は<see cref="System.IO.FileSystemInfo"/>オブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.IO.IOException"/>
		public FileSystemInfo? GetFileSystemInfo()
		{
			this.EnsureFileSystemInfo();
			return _fsinfo;
		}

		private void EnsureFileSystemInfo()
		{
			try {
				if (_fsinfo is null) {
					if (this.IsDirectory) {
						_fsinfo = new DirectoryInfo(_path);
					} else if (this.IsFile) {
						_fsinfo = new FileInfo(_path);
					}
				}
				if (this.IsDrive && _dinfo is null) {
					_dinfo = new DriveInfo(_path);
				}
			} catch (Exception e) {
				throw new IOException(string.Format(Resources.PathString_EnsureFileSystemInfo_IOException, _path), e);
			}
		}

		/// <summary>
		///  コンストラクタに渡されたパス文字列を取得します。
		/// </summary>
		/// <returns>コンストラクタに渡されたパス文字列を返します。</returns>
		public string GetOriginalString()
		{
			return _org_path;
		}

		/// <summary>
		///  パス文字列をURIへ変換します。
		/// </summary>
		/// <returns><see cref="System.Uri"/>形式のオブジェクトを返します。</returns>
		public Uri AsUri()
		{
			return _uri;
		}

		/// <summary>
		///  パス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <returns>現在のパス文字列を表す可読な文字列を返します。</returns>
		public sealed override string ToString()
		{
			return _path;
		}

		/// <summary>
		///  書式設定を利用してパス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <remarks>
		///  書式設定文字列(<paramref name="format"/>)の指定方法は、
		///  <see cref="TakymLib.IO.PathStringFormatter.Format(string?, object?, IFormatProvider?)"/>の
		///  説明を確認してください。
		/// </remarks>
		/// <param name="format">書式設定文字列を指定します。</param>
		/// <returns>現在のパス文字列を表す可読な文字列を返します。</returns>
		public string ToString(string? format)
		{
			return this.ToString(format, null);
		}

		/// <summary>
		///  書式設定を利用してパス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <param name="formatProvider">書式設定サービスを提供する書式設定プロバイダを指定します。</param>
		/// <returns>現在のパス文字列を表す可読な文字列を返します。</returns>
		public string ToString(IFormatProvider? formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		/// <summary>
		///  書式設定を利用してパス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <remarks>
		///  書式設定文字列(<paramref name="format"/>)の指定方法は、
		///  <see cref="TakymLib.IO.PathStringFormatter.Format(string?, object?, IFormatProvider?)"/>の
		///  説明を確認してください。
		/// </remarks>
		/// <param name="format">書式設定文字列を指定します。</param>
		/// <param name="formatProvider">書式設定サービスを提供する書式設定プロバイダを指定します。</param>
		/// <returns>現在のパス文字列を表す可読な文字列を返します。</returns>
		public /* virtual */ string ToString(string? format, IFormatProvider? formatProvider)
		{
			switch (formatProvider) {
			case null:
				return PathStringFormatter._inst.Format(format, this, null);
			case PathStringFormatter psf1:
				return psf1.Format(format, this, psf1);
			default:
				if (formatProvider.GetFormat(typeof(PathStringFormatter)) is PathStringFormatter psf2) {
					return psf2.Format(format, this, psf2);
				} else if (formatProvider.GetFormat(typeof(ICustomFormatter)) is ICustomFormatter formatter) {
					return formatter.Format(format, this, formatProvider);
				} else {
					return this.ToString();
				}
			}
		}

		/// <summary>
		///  指定したオブジェクトインスタンスの値と現在のインスタンスの値が等価かどうか判定します。
		/// </summary>
		/// <param name="obj">判定対象のオブジェクトを指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(this, obj)) {
				return true;
			} else if (obj is PathString path) {
				return this.Equals(path);
			} else if (obj is string text) {
				return this.Equals(text);
			} else {
				return base.Equals(obj);
			}
		}

		/// <summary>
		///  指定したパス文字列と現在のパス文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="other">判定対象のパス文字列を指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public bool Equals(PathString? other)
		{
			return _path == other?._path;
		}

		/// <summary>
		///  指定した文字列と現在のパス文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="other">判定対象の文字列を指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public bool Equals(string? other)
		{
			return _path == other || _org_path == other;
		}

		/// <summary>
		///  指定したオブジェクトインスタンスの値と現在のインスタンスの値を比較します。
		/// </summary>
		/// <param name="obj">比較対象のオブジェクトを指定します。</param>
		/// <returns>
		///  等価の場合は<c>0</c>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(object? obj)
		{
			if (obj is PathString path) {
				return this.CompareTo(path);
			} else if (obj is string text) {
				return this.CompareTo(text);
			} else {
				return _path.CompareTo(null);
			}
		}

		/// <summary>
		///  指定したパス文字列と現在のパス文字列を比較します。
		/// </summary>
		/// <param name="other">比較対象のパス文字列を指定します。</param>
		/// <returns>
		///  等価の場合は<c>0</c>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(PathString? other)
		{
			return _path.CompareTo(other?._path);
		}

		/// <summary>
		///  指定した文字列と現在のパス文字列を比較します。
		/// </summary>
		/// <param name="other">比較対象の文字列を指定します。</param>
		/// <returns>
		///  等価の場合は<c>0</c>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(string? other)
		{
			return _path.CompareTo(other);
		}

		/// <summary>
		///  現在のパス文字列のハッシュコードを取得します。
		/// </summary>
		/// <returns>現在のパス文字列が格納している文字列のハッシュ値を返します。</returns>
		public override int GetHashCode()
		{
			return _path.GetHashCode();
		}

		/// <summary>
		///  指定された二つのパス文字列を結合します。
		/// </summary>
		/// <param name="left">基底パスを指定します。</param>
		/// <param name="right">相対パスを指定します。</param>
		/// <returns>結合されたパス文字列を返します。</returns>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public static PathString operator +(PathString? left, string? right)
			=> left?.Combine(right ?? string.Empty) ?? PathStringPool.Get(right ?? ROOT_PATH);

		/// <summary>
		///  <paramref name="right"/>を基にした<paramref name="left"/>の相対パスを計算します。
		/// </summary>
		/// <param name="left">絶対パスを指定します。</param>
		/// <param name="right">基底パスを指定します。</param>
		/// <returns><paramref name="left"/>への相対パスを返します。</returns>
		/// <exception cref="System.PlatformNotSupportedException" />
		public static string? operator -(PathString? left, PathString? right)
			=> left?.GetRelativePath(right ?? PathStringPool.Get());

		/// <summary>
		///  指定された二つのパス文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public static bool operator ==(PathString? left, PathString? right)
			=> left is null ? right is null : left.Equals(right);

		/// <summary>
		///  指定された二つのパス文字列が不等価かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>等しい場合は<see langword="false"/>、等しくない場合は<see langword="true"/>を返します。</returns>
		public static bool operator !=(PathString? left, PathString? right)
			=> left is null ? right is not null : !left.Equals(right);
			//=> !(left == right);

		/// <summary>
		///  左辺が右辺未満かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より小さい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator <(PathString? left, PathString? right)
			=> left is null ? right is not null : left.CompareTo(right) < 0;

		/// <summary>
		///  左辺が右辺以下かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より小さいか等しい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator <=(PathString? left, PathString? right)
			=> left is null || left.CompareTo(right) <= 0;

		/// <summary>
		///  左辺が右辺超過かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より大きい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator >(PathString? left, PathString? right)
			=> left is not null && left.CompareTo(right) > 0;
			//=> !(left <= right);

		/// <summary>
		///  左辺が右辺以上かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より大きいか等しい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator >=(PathString? left, PathString? right)
			=> left is null ? right is null : left.CompareTo(right) >= 0;
			//=> !(left < right);

		/// <summary>
		///  パス文字列を通常の文字列へ暗黙的に変換します。
		/// </summary>
		/// <param name="path">通常の文字列へ変換するパス文字列を指定します。</param>
		public static implicit operator string(PathString? path) => path?._path ?? string.Empty;

		/// <summary>
		///  通常の文字列をパス文字列へ明示的に変換します。
		/// </summary>
		/// <param name="path">パス文字列へ変換する通常の文字列を指定します。</param>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public static explicit operator PathString(string? path) => PathStringPool.Get(path ?? ROOT_PATH);
	}
}
