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
	///  電子メールアドレスを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlEmailInfo : XrcdlContactInformation
	{
		/// <summary>
		///  電子メールアドレスを表す文字列を取得または設定します。
		/// </summary>
		public abstract string Address { get; set; }

		/// <summary>
		///  既定の件名を取得または設定します。
		/// </summary>
		public abstract string? Subject { get; set; }

		/// <summary>
		///  既定の本文を取得または設定します。
		/// </summary>
		public abstract string? Body { get; set; }

		internal XrcdlEmailInfo(XrcdlMetadata metadata) : base(metadata) { }

		/// <summary>
		///  この連絡先情報をURI形式で表現します。
		/// </summary>
		/// <returns><see cref="System.Uri"/>オブジェクトを返します。</returns>
		public sealed override Uri AsUri()
		{
			if (!string.IsNullOrEmpty(this.Subject) && !string.IsNullOrEmpty(this.Body)) {
				return new Uri($"mailto:{this.Address}?subject={Uri.EscapeDataString(this.Subject)}&body={Uri.EscapeDataString(this.Body)}");
			} else if (!string.IsNullOrEmpty(this.Subject)) {
				return new Uri($"mailto:{this.Address}?subject={Uri.EscapeDataString(this.Subject)}");
			} else if (!string.IsNullOrEmpty(this.Body)) {
				return new Uri($"mailto:{this.Address}?body={Uri.EscapeDataString(this.Body)}");
			} else {
				return new Uri($"mailto:{this.Address}");
			}
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
			format ??= Resources.ResourceManager.GetString(nameof(Resources.XrcdlEmailInfo_Format), culture)!;
			return string.Format(format, this.Address);
		}
	}
}
