/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using TakymLib;

namespace Exrecodel.Extensions
{
	/// <summary>
	///  型'<see cref="Exrecodel.XrcdlMetadata"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class XrcdlMetadataExtensions
	{
		/// <summary>
		///  指定されたメタ情報から文書のバージョン情報を取得します。
		/// </summary>
		/// <param name="metadata">メタ情報を表すオブジェクトです。</param>
		/// <returns>
		///  この文書のバージョン情報を表す型'<see cref="System.Version"/>'のオブジェクトです。
		///  <see cref="Exrecodel.XrcdlMetadata.VersionString"/>を変換する事ができなかった場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static Version? GetVersion(this XrcdlMetadata metadata)
		{
			metadata.EnsureNotNull(nameof(metadata));
			if (Version.TryParse(metadata.VersionString, out var result)) {
				return result;
			} else {
				return null;
			}
		}

		/// <summary>
		///  指定されたメタ情報から文書のバージョン情報を設定します。
		/// </summary>
		/// <param name="metadata">メタ情報を表すオブジェクトです。</param>
		/// <param name="version">バージョン情報を格納しているオブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public static void SetVersion(this XrcdlMetadata metadata, Version version)
		{
			metadata.EnsureNotNull(nameof(metadata));
			version .EnsureNotNull(nameof(version));
			metadata.VersionString = version.ToString();
		}
	}
}
