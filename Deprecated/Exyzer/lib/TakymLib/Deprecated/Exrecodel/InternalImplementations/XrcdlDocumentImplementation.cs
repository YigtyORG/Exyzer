/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Exrecodel.Properties;
using TakymLib;

namespace Exrecodel.InternalImplementations
{
	internal sealed class XrcdlDocumentImplementation : XrcdlDocument
	{
		private static readonly XmlSchema                   _schema;
		private        readonly XmlDocument                 _xmldoc;
		private        readonly XrcdlMetadataImplementation _metadata;
		private        readonly XrcdlRootImplementation     _root;

		static XrcdlDocumentImplementation()
		{
			using (var sr = new StringReader(Resources.XmlSchema)) {
				_schema = XmlSchema.Read(sr, (sender, e) => { /* do nothing */ }) ?? new XmlSchema();
			}
		}

		internal XrcdlDocumentImplementation()
		{
			_xmldoc   = new XmlDocument();
			_metadata = new XrcdlMetadataImplementation(this);
			_root     = new XrcdlRootImplementation(this);
		}

		protected private override void LoadCore(XmlReader reader)
		{
			_xmldoc.Load(reader);
			if (_xmldoc.DocumentType   ?.Name != Constants.Document ||
				_xmldoc.DocumentElement?.Name != Constants.Document) {
				throw new XmlSchemaException(Resources.XrcdlDocumentImplementation_LoadCore_XmlSchemaException);
			}
			this.EnsureSchema();
			_xmldoc.Validate((sender, e) => { /* do nothing */ });
		}

		protected private override void SaveCore(XmlWriter writer)
		{
			_xmldoc.Save(writer);
			this.EnsureSchema();
		}

		public override XrcdlMetadata GetMetadata()
		{
			return _metadata;
		}

		public override XrcdlRoot GetRootNode()
		{
			return _root;
		}

		private void EnsureSchema()
		{
			if (!_xmldoc.Schemas.Contains(_schema)) {
				_xmldoc.Schemas.Add(_schema);
			}
		}

		private void EnsureRootElement()
		{
			if (_xmldoc.DocumentElement is null) {
				if (_xmldoc.DocumentType is null) {
					_xmldoc.AppendChild(_xmldoc.CreateDocumentType(Constants.Document, string.Empty, string.Empty, string.Empty));
				}
				_xmldoc.AppendChild(_xmldoc.CreateElement(Constants.Document));
			}
		}

		internal XmlElement GetElement(params string[] names)
		{
			this.EnsureRootElement();
			return Utils.GetElement(this.CreateElement, _xmldoc.DocumentElement!, names);
		}

		internal XmlElement CreateElement(string name)
		{
			return _xmldoc.CreateElement(name);
		}

		public override IXrcdlConverter GetConverter()
		{
			return new XrcdlConverter(this);
		}

		private readonly struct XrcdlConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlDocumentImplementation _doc;

			internal XrcdlConverter(XrcdlDocumentImplementation doc)
			{
				_doc = doc;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				using (var convr = _doc.GetRootNode().GetConverter())
				using (var convm = _doc.GetMetadata().GetConverter()) {
					convr.ConvertToHtml(sb);
					convm.ConvertToHtml(sb);
				}
			}

			public async Task ConvertToHtmlAsync(StringBuilder sb)
			{
				sb.EnsureNotNull(nameof(sb));
				await this.ConvertToHtmlAsyncCore(sb, _doc.GetRootNode().GetConverter());
				await this.ConvertToHtmlAsyncCore(sb, _doc.GetMetadata().GetConverter());
			}

			private async ValueTask ConvertToHtmlAsyncCore(StringBuilder sb, IXrcdlConverter conv)
			{
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
