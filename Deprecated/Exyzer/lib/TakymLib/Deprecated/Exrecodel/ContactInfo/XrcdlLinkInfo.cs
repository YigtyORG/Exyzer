/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Exrecodel.InternalImplementations;
using Exrecodel.Properties;

namespace Exrecodel.ContactInfo
{
	/// <summary>
	///  Webサイト等のその他のURI形式のリンクを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlLinkInfo : XrcdlContactInformation
	{
		/// <summary>
		///  リンク情報を取得または設定します。
		/// </summary>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.UriFormatException"/>
		public abstract Uri Link { get; set; }

		/// <summary>
		///  リンク先のオブジェクトの名前または概要を取得または設定します。
		/// </summary>
		public abstract string? Title { get; set; }

		internal XrcdlLinkInfo(XrcdlMetadata metadata) : base(metadata) { }

		/// <summary>
		///  この連絡先情報をURI形式で表現します。
		/// </summary>
		/// <returns><see cref="System.Uri"/>オブジェクトを返します。</returns>
		public sealed override Uri AsUri()
		{
			return this.Link;
		}

		/// <summary>
		///  この連絡先情報を可読な文字列へ変換します。
		/// </summary>
		/// <param name="format">書式設定文字列です。</param>
		/// <param name="formatProvider">書式設定を制御するオブジェクトです。</param>
		/// <returns>連絡先情報を表す可読な文字列です。</returns>
		public sealed override string ToString(string? format, IFormatProvider? formatProvider)
		{
			var culture = Utils.FormatProviderToCultureInfo(formatProvider);
			format ??= Resources.ResourceManager.GetString(nameof(Resources.XrcdlLinkInfo_Format), culture)!;
			string title;
			if (string.IsNullOrEmpty(this.Title)) {
				title = Resources.ResourceManager.GetString(nameof(Resources.XrcdlLinkInfo_NoTitle), culture)!;
			} else {
				title = this.Title;
			}
			return string.Format(format, this.Link, title);
		}
	}
}
