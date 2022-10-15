/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Exrecodel.ContactInfo;
using TakymLib;

namespace Exrecodel.InternalImplementations.ContactInfo
{
	internal sealed class XrcdlEmailInfoImplementation : XrcdlEmailInfo
	{
		private readonly XmlElement _email_elem;

		public override string Address
		{
			get => _email_elem.InnerText ?? string.Empty;
			set => _email_elem.InnerText = value;
		}

		public override string? Subject
		{
			get => _email_elem.GetAttribute(Constants.Subject);
			set => _email_elem.SetAttribute(Constants.Subject, value);
		}

		public override string? Body
		{
			get => _email_elem.GetAttribute(Constants.Body);
			set => _email_elem.SetAttribute(Constants.Body, value);
		}

		public XrcdlEmailInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement emailElement) : base(metadata)
		{
			_email_elem = emailElement;
		}

		public override string GetContactType()
		{
			return Constants.EmailInfo;
		}

		public override IXrcdlConverter GetConverter()
		{
			return new XrcdlConverter(this);
		}

		private readonly struct XrcdlConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlEmailInfoImplementation _info;

			internal XrcdlConverter(XrcdlEmailInfoImplementation info)
			{
				_info = info;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				sb.AppendStartContactInfo(_info);
				sb.Append($"<p><a href=\"{_info.AsUri()}\">{_info.Address}</a></p>");
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
