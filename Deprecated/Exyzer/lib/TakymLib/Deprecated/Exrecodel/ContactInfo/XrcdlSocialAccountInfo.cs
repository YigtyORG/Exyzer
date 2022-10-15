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
	///  社会ネットワークアカウント情報を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlSocialAccountInfo : XrcdlContactInformation
	{
		/// <summary>
		///  アカウント名を取得または設定します。
		/// </summary>
		public abstract string AccountName { get; set; }

		/// <summary>
		///  SNSサービスへのURLの接頭辞を取得または設定します。
		/// </summary>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.UriFormatException"/>
		public abstract Uri UriPrefix { get; set; }

		/// <summary>
		///  SNSサービスの名前を取得または設定します。
		/// </summary>
		public abstract string? ServiceName { get; set; }

		internal XrcdlSocialAccountInfo(XrcdlMetadata metadata) : base(metadata) { }

		/// <summary>
		///  この連絡先情報をURI形式で表現します。
		/// </summary>
		/// <returns><see cref="System.Uri"/>オブジェクトを返します。</returns>
		public sealed override Uri AsUri()
		{
			return new Uri($"{this.UriPrefix.OriginalString}{this.AccountName}");
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
			format ??= Resources.ResourceManager.GetString(nameof(Resources.XrcdlSocialAccountInfo_Format), culture)!;
			string sname;
			if (string.IsNullOrEmpty(this.ServiceName)) {
				sname = Resources.ResourceManager.GetString(nameof(Resources.XrcdlSocialAccountInfo_NoServiceName), culture)!;
			} else {
				sname = this.ServiceName;
			}
			return string.Format(format, this.AccountName, sname);
		}
	}
}
