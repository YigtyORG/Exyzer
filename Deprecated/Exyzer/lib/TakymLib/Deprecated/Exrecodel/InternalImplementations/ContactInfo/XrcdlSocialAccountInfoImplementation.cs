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
using Exrecodel.Properties;
using TakymLib;

namespace Exrecodel.InternalImplementations.ContactInfo
{
	internal sealed class XrcdlSocialAccountInfoImplementation : XrcdlSocialAccountInfo
	{
		private readonly XmlElement _account_elem;
		private          Uri?       _uri_prefix;

		public override string AccountName
		{
			get => _account_elem.InnerText ?? string.Empty;
			set => _account_elem.InnerText = value;
		}

		public override Uri UriPrefix
		{
			get
			{
				if (_uri_prefix is null) {
					_uri_prefix = new Uri(_account_elem.GetAttribute(Constants.UriPrefix));
				}
				return _uri_prefix;
			}
			set
			{
				_uri_prefix = value;
				_account_elem.SetAttribute(Constants.UriPrefix, _uri_prefix.OriginalString);
			}
		}

		public override string? ServiceName
		{
			get => _account_elem.GetAttribute(Constants.Name);
			set => _account_elem.SetAttribute(Constants.Name, value);
		}

		internal XrcdlSocialAccountInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement socialAccountElement) : base(metadata)
		{
			_account_elem = socialAccountElement;
		}

		public override string GetContactType()
		{
			return Constants.SocialAccountInfo;
		}

		public override IXrcdlConverter GetConverter()
		{
			return new XrcdlConverter(this);
		}

		private readonly struct XrcdlConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlSocialAccountInfoImplementation _info;

			internal XrcdlConverter(XrcdlSocialAccountInfoImplementation info)
			{
				_info = info;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				sb.AppendStartContactInfo(_info);
				sb.Append($"<p><a href=\"{_info.AsUri()}\">@{_info.AccountName}</a></p>");
				if (!string.IsNullOrEmpty(_info.ServiceName)) {
					sb.Append("<p>");
					sb.AppendFormat(HtmlTexts.ContactInfo_sns_Provider, $"<span>{_info.ServiceName}</span>");
					sb.Append("</p>");
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
