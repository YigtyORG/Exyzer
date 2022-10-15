/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Exrecodel.ContactInfo;
using TakymLib;

namespace Exrecodel.InternalImplementations.ContactInfo
{
	internal sealed class XrcdlPhoneNumberInfoImplementation : XrcdlPhoneNumberInfo
	{
		private readonly XmlElement _num_elem;
		private readonly string     _type;

		public override string Number
		{
			get => _num_elem.InnerText ?? string.Empty;
			set => _num_elem.InnerText = value;
		}

		public override XrcdlPhoneNumberType Type
		{
			get => _type switch {
				Constants.PhoneNumberInfoTelephone           => XrcdlPhoneNumberType.Telephone,
				Constants.PhoneNumberInfoFacsimile           => XrcdlPhoneNumberType.Facsimile,
				Constants.PhoneNumberInfoShortMessageService => XrcdlPhoneNumberType.ShortMessageService,
				_                                            => XrcdlPhoneNumberType.Unknown
			};
		}

		public XrcdlPhoneNumberInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement phoneNumberElement) : base(metadata)
		{
			_num_elem = phoneNumberElement;
			_type     = _num_elem.LocalName;
		}

		public override string GetContactType()
		{
			return _type;
		}

		public override IXrcdlConverter GetConverter()
		{
			return new XrcdlConverter(this);
		}

		private readonly struct XrcdlConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlPhoneNumberInfoImplementation _info;

			internal XrcdlConverter(XrcdlPhoneNumberInfoImplementation info)
			{
				_info = info;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				sb.AppendStartContactInfo(_info);
				try {
					sb.Append($"<p><a href=\"{_info.AsUri().OriginalString}\">{_info.Number}</a></p>");
				} catch (FormatException e) {
					sb.Append($"<p class=\"error\">{e.Message}</p>");
				}
				sb.AppendEndContactInfo();
			}

			public Task ConvertToHtmlAsync(StringBuilder sb)
			{
				this.ConvertToHtml(sb);
				return Task.CompletedTask;
			}

			public void Dispose()
			{
				// do nothing
			}

			public ValueTask DisposeAsync()
			{
				// do nothing
				return default;
			}
		}
	}
}
