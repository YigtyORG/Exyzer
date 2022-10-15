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
	internal sealed class XrcdlLinkInfoImplementation : XrcdlLinkInfo
	{
		private const    string     _link_example = "https://example.com";
		private readonly XmlElement _link_elem;
		private          Uri?       _link;

		public override Uri Link
		{
			get
			{
				if (_link is null) {
					_link = new Uri(_link_elem.InnerText ?? _link_example);
				}
				return _link;
			}
			set
			{
				_link = value;
				_link_elem.InnerText = _link.OriginalString;
			}
		}

		public override string? Title
		{
			get => _link_elem.GetAttribute(Constants.Name);
			set => _link_elem.SetAttribute(Constants.Name, value);
		}

		internal XrcdlLinkInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement linkElement) : base(metadata)
		{
			_link_elem = linkElement;
		}

		public override string GetContactType()
		{
			return Constants.LinkInfo;
		}

		public override IXrcdlConverter GetConverter()
		{
			return new XrcdlConverter(this);
		}

		private readonly struct XrcdlConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlLinkInfoImplementation _info;

			internal XrcdlConverter(XrcdlLinkInfoImplementation info)
			{
				_info = info;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				sb.AppendStartContactInfo(_info);
				if (string.IsNullOrEmpty(_info.Title)) {
					sb.Append($"<p><a href=\"{_info._link?.OriginalString}\">{_info._link?.OriginalString}</a></p>");
				} else {
					sb.Append($"<p><a href=\"{_info._link?.OriginalString}\">{_info.Title}</a></p>");
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
