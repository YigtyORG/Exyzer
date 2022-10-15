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
	///  郵便番号を含む住所を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlAddressInfo : XrcdlContactInformation
	{
		/// <summary>
		///  郵便番号を取得または設定します。
		/// </summary>
		public abstract long? PostCode { get; set; }

		/// <summary>
		///  住所を表す文字列を取得または設定します。
		/// </summary>
		public abstract string Address { get; set; }

		internal XrcdlAddressInfo(XrcdlMetadata metadata) : base(metadata) { }

		/// <summary>
		///  この連絡先情報はURI形式で表現できないため常に<see langword="null"/>を返します。
		/// </summary>
		/// <returns><see langword="null"/>を返します。</returns>
		public sealed override Uri? AsUri()
		{
			return null;
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
			format ??= Resources.ResourceManager.GetString(nameof(Resources.XrcdlAddressInfo_Format), culture)!;
			string zipcode;
			if (this.PostCode.HasValue) {
				zipcode = this.PostCode.Value.ToString(formatProvider);
			} else {
				zipcode = Resources.ResourceManager.GetString(nameof(Resources.XrcdlAddressInfo_NoPostCode), culture)!;
			}
			return string.Format(format, this.Address, zipcode);
		}
	}
}
