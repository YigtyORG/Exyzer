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
	///  国際電話番号を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlPhoneNumberInfo : XrcdlContactInformation
	{
		/// <summary>
		///  電話番号を表す文字列を取得または設定します。
		/// </summary>
		public abstract string Number { get; set; }

		/// <summary>
		///  この電話番号の種類を取得します。
		/// </summary>
		public abstract XrcdlPhoneNumberType Type { get; }

		internal XrcdlPhoneNumberInfo(XrcdlMetadata metadata) : base(metadata) { }

		/// <summary>
		///  現在の電話番号が正しい書式かどうか判定します。
		/// </summary>
		/// <returns>正しい書式を持つ場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。</returns>
		public bool Validate()
		{
			if (this.Type == XrcdlPhoneNumberType.Telephone ||
				this.Type == XrcdlPhoneNumberType.Facsimile ||
				this.Type == XrcdlPhoneNumberType.ShortMessageService) {
				string num = this.Number;
				for (int i = 0; i < num.Length; ++i) {
					if (!char.IsWhiteSpace(num[i]) &&
						!char.IsNumber(num[i]) &&
						num[i] != '+' && num[i] != '-' && num[i] != '(' && num[i] != ')') {
						return false;
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>
		///  現在の電話番号文字列を正規化します。
		/// </summary>
		/// <exception cref="System.FormatException"/>
		public void Normalize()
		{
			if (this.Validate()) {
				this.Number = this.Number
					.Replace('\r', '-')
					.Replace('\n', '-')
					.Replace('\t', '-')
					.Replace(' ',  '-');
			} else {
				throw new FormatException(string.Format(Resources.XrcdlPhoneNumberInfo_Normalize_FormatException, this.Number));
			}
		}

		/// <summary>
		///  この連絡先情報をURI形式で表現します。
		///  この関数を呼び出すと正規化処理も実行されます。
		/// </summary>
		/// <returns><see cref="System.Uri"/>オブジェクトを返します。</returns>
		/// <exception cref="System.FormatException"/>
		public sealed override Uri AsUri()
		{
			this.Normalize();
			return new Uri($"{this.GetContactType()}:{this.Number}");
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
			if (format is null) {
				switch (this.Type) {
				case XrcdlPhoneNumberType.Telephone:
					format = Resources.ResourceManager.GetString(nameof(Resources.XrcdlPhoneNumberInfo_Format_Telephone), culture)!;
					break;
				case XrcdlPhoneNumberType.Facsimile:
					format = Resources.ResourceManager.GetString(nameof(Resources.XrcdlPhoneNumberInfo_Format_Facsimile), culture)!;
					break;
				case XrcdlPhoneNumberType.ShortMessageService:
					format = Resources.ResourceManager.GetString(nameof(Resources.XrcdlPhoneNumberInfo_Format_ShortMessageService), culture)!;
					break;
				default:
					format = Resources.ResourceManager.GetString(nameof(Resources.XrcdlPhoneNumberInfo_Format), culture)!;
					break;
				}
			}
			return string.Format(format, this.Number);
		}
	}
}
