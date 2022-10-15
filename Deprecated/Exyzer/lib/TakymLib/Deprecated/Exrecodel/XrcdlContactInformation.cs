/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Exrecodel.Properties;

namespace Exrecodel
{
	/// <summary>
	///  連絡先情報を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlContactInformation : IXrcdlElement, IFormattable
	{
		/// <summary>
		///  この連絡先情報の種類を表す文字列を取得します。
		///  このプロパティは設定できません。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		public string Name
		{
			get => this.GetContactType();

			[Obsolete("連絡先情報の名前は変更できません。", true)]
			set => throw new InvalidOperationException(Resources.XrcdlContactInformation_Name_InvalidOperationException);
		}

		/// <summary>
		///  この連絡先情報と関連付けられている文書を取得します。
		/// </summary>
		public XrcdlDocument Document => this.Metadata.Document;

		/// <summary>
		///  この連絡先情報と関連付けられているメタ情報を取得します。
		/// </summary>
		public XrcdlMetadata Metadata { get; }

		internal XrcdlContactInformation(XrcdlMetadata metadata)
		{
			this.Metadata = metadata;
		}

		/// <summary>
		///  この連絡先情報をURI形式で表現します。
		/// </summary>
		/// <returns>
		///  <see cref="System.Uri"/>オブジェクトを返します。
		///  この連絡先情報をURI形式に変換できない場合は<see langword="null"/>を返します。
		/// </returns>
		public abstract Uri? AsUri();

		/// <summary>
		///  この連絡先情報の種類を表す文字列を取得します。
		/// </summary>
		/// <returns>連絡先情報の種類を表す文字列です。</returns>
		public abstract string GetContactType();

		/// <summary>
		///  現在の連絡先情報の変換処理を行うオブジェクトを取得します。
		/// </summary>
		/// <returns><see cref="Exrecodel.IXrcdlConverter"/>へ変換可能なオブジェクトです。</returns>
		public abstract IXrcdlConverter GetConverter();

		/// <summary>
		///  この連絡先情報を可読な文字列へ変換します。
		/// </summary>
		/// <returns>連絡先情報を表す可読な文字列です。</returns>
		public sealed override string ToString()
		{
			return this.ToString(null, null);
		}

		/// <summary>
		///  この連絡先情報を可読な文字列へ変換します。
		/// </summary>
		/// <returns>連絡先情報を表す可読な文字列です。</returns>
		public string ToString(string? format)
		{
			return this.ToString(format, null);
		}

		/// <summary>
		///  この連絡先情報を可読な文字列へ変換します。
		/// </summary>
		/// <returns>連絡先情報を表す可読な文字列です。</returns>
		public string ToString(IFormatProvider? formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		/// <summary>
		///  この連絡先情報を可読な文字列へ変換します。
		/// </summary>
		/// <param name="format">書式設定文字列です。</param>
		/// <param name="formatProvider">書式設定を制御するオブジェクトです。</param>
		/// <returns>連絡先情報を表す可読な文字列です。</returns>
		public abstract string ToString(string? format, IFormatProvider? formatProvider);
	}
}
