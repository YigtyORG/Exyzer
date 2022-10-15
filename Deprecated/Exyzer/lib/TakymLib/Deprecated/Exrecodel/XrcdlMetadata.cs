/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.IO;

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書のメタ情報を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlMetadata : IXrcdlElement
	{
		/// <summary>
		///  この文書の名前を取得または設定します。
		/// </summary>
		public abstract string Name { get; set; }

		/// <summary>
		///  このメタ情報と関連付けられている文書を取得します。
		/// </summary>
		public XrcdlDocument Document { get; }

		/// <summary>
		///  この文書の種類を取得または設定します。
		/// </summary>
		public abstract XrcdlDocumentType Type { get; set; }

		/// <summary>
		///  この文書の言語を取得または設定します。
		/// </summary>
		public abstract CultureInfo Language { get; set; }

		/// <summary>
		///  この文書の基底となる文書への絶対パスまたは相対パスを取得または設定します。
		/// </summary>
		public abstract string? BaseDocumentPath { get; set; }

		/// <summary>
		///  この文書の題名を取得または設定します。
		/// </summary>
		public abstract string? Title { get; set; }

		/// <summary>
		///  この文書の作成者の名前を取得または設定します。
		/// </summary>
		public abstract string? Author { get; set; }

		/// <summary>
		///  この文書の著作権情報を取得または設定します。
		/// </summary>
		public abstract string? Copyright { get; set; }

		/// <summary>
		///  この文書のバージョン情報の文字列形式を取得または設定します。
		/// </summary>
		public abstract string? VersionString { get; set; }

		/// <summary>
		///  この文書の新規作成日時を取得または設定します。
		/// </summary>
		public abstract DateTime? Creation { get; set; }

		/// <summary>
		///  この文書の最終更新日時を取得または設定します。
		/// </summary>
		public abstract DateTime? LastModified { get; set; }

		/// <summary>
		///  この文書の作成者の連絡先情報を格納したリストを取得します。
		/// </summary>
		public abstract IXrcdlContactInformationList Contacts { get; }

		internal XrcdlMetadata(XrcdlDocument document)
		{
			this.Document = document;
		}

		/// <summary>
		///  この文書の表示名を取得します。
		/// </summary>
		/// <returns>
		///  <see cref="Exrecodel.XrcdlMetadata.Title"/>に値が格納されている場合はその値を返し、
		///  値が格納されていなかった場合は<see cref="Exrecodel.XrcdlMetadata.Name"/>を返します。
		/// </returns>
		public string GetDisplayName()
		{
			return this.Title ?? this.Name;
		}

		/// <summary>
		///  この文書の基底となる文書を読み込みます。
		/// </summary>
		/// <returns>基底の文書を表す新しい型'<see cref="Exrecodel.XrcdlDocument"/>'のインスタンスです。</returns>
		public XrcdlDocument? GetBaseDocument()
		{
			if (string.IsNullOrEmpty(this.BaseDocumentPath)) {
				return null;
			} else {
				var result = XrcdlDocument.Create();
				result.Load(Path.Combine(
					Path.GetDirectoryName(this.Document.FilePath) ?? Environment.CurrentDirectory,
					this.BaseDocumentPath)
				);
				return result;
			}
		}

		/// <summary>
		///  現在のメタ情報の変換処理を行うオブジェクトを取得します。
		/// </summary>
		/// <returns><see cref="Exrecodel.IXrcdlConverter"/>へ変換可能なオブジェクトです。</returns>
		public abstract IXrcdlConverter GetConverter();
	}
}
