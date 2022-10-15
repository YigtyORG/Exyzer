/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Exrecodel.ContactInfo;
using Exrecodel.InternalImplementations.ContactInfo;
using Exrecodel.Properties;
using TakymLib;

namespace Exrecodel.InternalImplementations
{
	internal sealed class XrcdlContactInformationList : IXrcdlContactInformationList
	{
		private readonly XrcdlMetadataImplementation   _metadata;
		private readonly XmlElement                    _contact_elem;
		private readonly List<XrcdlContactInformation> _cache;
		private readonly List<XmlElement>              _elem_cache;
		private          ulong                         _version;
		public           XrcdlDocument                 Document => _metadata.Document;
		public           int                           Count    => _cache.Count;

		public XrcdlContactInformation this[int index]
		{
			get => _cache[index];
		}

		public string Name
		{
			get => _metadata.Name;
			set => _metadata.Name = value;
		}

		internal XrcdlContactInformationList(XrcdlMetadataImplementation metadata, XmlElement contactElement)
		{
			_metadata     = metadata;
			_contact_elem = contactElement;
			_cache        = new List<XrcdlContactInformation>();
			_elem_cache   = new List<XmlElement>();
			for (int i = 0; i < _contact_elem.ChildNodes.Count; ++i) {
				if (_contact_elem.ChildNodes[i] is XmlElement elem) {
					switch (elem.LocalName) {
					case Constants.AddressInfo:
						_cache     .Add(new XrcdlAddressInfoImplementation(_metadata, elem));
						_elem_cache.Add(elem);
						break;
					case Constants.EmailInfo:
						_cache     .Add(new XrcdlEmailInfoImplementation(_metadata, elem));
						_elem_cache.Add(elem);
						break;
					case Constants.PhoneNumberInfoTelephone:
					case Constants.PhoneNumberInfoFacsimile:
					case Constants.PhoneNumberInfoShortMessageService:
						_cache     .Add(new XrcdlPhoneNumberInfoImplementation(_metadata, elem));
						_elem_cache.Add(elem);
						break;
					}
				}
			}
		}

		public XrcdlAddressInfo AppendAddressInfo(string address, long? postcode = null)
		{
			return this.AppendInfoCore(Constants.AddressInfo, (elem) =>
				new XrcdlAddressInfoImplementation(_metadata, elem) {
					Address  = address,
					PostCode = postcode
				}
			);
		}

		public XrcdlEmailInfo AppendEmailInfo(string email, string? subject = null, string? body = null)
		{
			return this.AppendInfoCore(Constants.EmailInfo, (elem) =>
				new XrcdlEmailInfoImplementation(_metadata, elem) {
					Address = email,
					Subject = subject,
					Body    = body
				}
			);
		}

		public XrcdlPhoneNumberInfo AppendTelInfo(string telNumber)
		{
			return this.AppendPhoneNumberInfoCore(Constants.PhoneNumberInfoTelephone, telNumber);
		}

		public XrcdlPhoneNumberInfo AppendFaxInfo(string faxNumber)
		{
			return this.AppendPhoneNumberInfoCore(Constants.PhoneNumberInfoFacsimile, faxNumber);
		}

		public XrcdlPhoneNumberInfo AppendSmsInfo(string smsNumber)
		{
			return this.AppendPhoneNumberInfoCore(Constants.PhoneNumberInfoShortMessageService, smsNumber);
		}

		public XrcdlSocialAccountInfo AppendSocialAccountInfo(string accountName, Uri uriPrefix, string? serviceName = null)
		{
			return this.AppendInfoCore(Constants.SocialAccountInfo, (elem) =>
				new XrcdlSocialAccountInfoImplementation(_metadata, elem) {
					AccountName = accountName,
					UriPrefix   = uriPrefix,
					ServiceName = serviceName
				}
			);
		}

		public XrcdlLinkInfo AppendLinkInfo(Uri link, string? title = null)
		{
			return this.AppendInfoCore(Constants.LinkInfo, (elem) =>
				new XrcdlLinkInfoImplementation(_metadata, elem) {
					Link  = link,
					Title = title
				}
			);
		}

		private XrcdlPhoneNumberInfo AppendPhoneNumberInfoCore(string type, string number)
		{
			return this.AppendInfoCore(type, (elem) =>
				new XrcdlPhoneNumberInfoImplementation(_metadata, elem) {
					Number = number
				}
			);
		}

		private T AppendInfoCore<T>(string type, Func<XmlElement, T> createInfo) where T: XrcdlContactInformation
		{
			++_version;
			var elem     = _metadata.GetDocument().CreateElement(type);
			var result   = createInfo(elem);
			_elem_cache  .Add(elem);
			_cache       .Add(result);
			_contact_elem.AppendChild(elem);
			return result;
		}

		public bool Remove(XrcdlContactInformation contactInfo)
		{
			++_version;
			int i = _cache.IndexOf(contactInfo);
			if (0 <= i) {
				_contact_elem.RemoveChild(_elem_cache[i]);
				_cache       .RemoveAt(i);
				_elem_cache  .RemoveAt(i);
				return true;
			} else {
				return false;
			}
		}

		public bool Contains(XrcdlContactInformation contactInfo)
		{
			return _cache.Contains(contactInfo);
		}

		public int IndexOf(XrcdlContactInformation contactInfo)
		{
			return _cache.IndexOf(contactInfo);
		}

		public IEnumerator<XrcdlContactInformation> GetEnumerator()
		{
			return new XrcdlContactInformationEnumerator(this);
		}

		public IAsyncEnumerator<XrcdlContactInformation> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return new XrcdlContactInformationEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IXrcdlConverter GetConverter()
		{
			return new XrcdlContactInformationConverter(this);
		}

		private struct XrcdlContactInformationEnumerator : IEnumerator<XrcdlContactInformation>, IAsyncEnumerator<XrcdlContactInformation>
		{
			private readonly XrcdlContactInformationList _list;
			private          int                         _index;
			private readonly ulong                       _version;
			private          bool                        _is_disposed;

			public XrcdlContactInformation Current
			{
				get
				{
					this.EnsureValid();
					return _list[_index];
				}
			}

			object? IEnumerator.Current => this.Current;

			internal XrcdlContactInformationEnumerator(XrcdlContactInformationList list)
			{
				_list        = list;
				_index       = -1;
				_version     = list._version;
				_is_disposed = false;
			}

			public void Reset()
			{
				this.EnsureValid();
				_index = 0;
			}

			public bool MoveNext()
			{
				this.EnsureValid();
				++_index;
				return _index < _list.Count;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				return new ValueTask<bool>(this.MoveNext());
			}

			public void Dispose()
			{
				if (!_is_disposed) {
					_is_disposed = true;
					GC.SuppressFinalize(this);
				}
			}

			public ValueTask DisposeAsync()
			{
				this.Dispose();
				return default;
			}

			private readonly void EnsureValid()
			{
				if (_is_disposed) {
					throw new ObjectDisposedException(nameof(XrcdlContactInformationEnumerator));
				}
				if (_version != _list._version) {
					throw new InvalidOperationException(Resources.XrcdlContactInformationEnumerator_InvalidOperationException);
				}
			}
		}

		private readonly struct XrcdlContactInformationConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlContactInformationList _list;

			internal XrcdlContactInformationConverter(XrcdlContactInformationList list)
			{
				_list = list;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				sb.AppendStartContactList();
				foreach (var item in _list) {
					using (var conv = item.GetConverter()) {
						conv.ConvertToHtml(sb);
					}
				}
				sb.AppendEndContactList();
			}

			public async Task ConvertToHtmlAsync(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				sb.AppendStartContactList();
				await foreach (var item in _list) {
					var conv = item.GetConverter();
					switch (conv) {
					case IXrcdlAsyncConverter asyncConv:
						await using (asyncConv.ConfigureAwait(false)) {
							await asyncConv.ConvertToHtmlAsync(sb);
						}
						break;
					default:
						using (conv) {
							conv.ConvertToHtml(sb);
						}
						break;
					}
				}
				sb.AppendEndContactList();
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
