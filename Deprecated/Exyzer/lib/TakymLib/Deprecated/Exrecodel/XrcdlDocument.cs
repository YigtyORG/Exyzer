/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Exrecodel.InternalImplementations;
using Exrecodel.Properties;
using TakymLib;

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlDocument : IXrcdlElement
	{
		private static readonly Encoding _enc = Encoding.UTF8;

		private static readonly XmlReaderSettings _reader_settings = new XmlReaderSettings() {
			CloseInput       = false,
			IgnoreWhitespace = true,
			IgnoreComments   = true
		};

		private static readonly XmlWriterSettings _writer_settings = new XmlWriterSettings() {
			CloseOutput = false,
			Indent      = true,
			IndentChars = "\t",
		};

		private static XmlWriterSettings GetWriterSettings(TextWriter writer)
		{
			var result = _writer_settings.Clone();
			result.Encoding     = writer.Encoding;
			result.NewLineChars = writer.NewLine;
			return result;
		}

		/// <summary>
		///  型'<see cref="Exrecodel.XrcdlDocument"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <returns>新しい型'<see cref="Exrecodel.XrcdlDocument"/>'のオブジェクトです。</returns>
		public static XrcdlDocument Create()
		{
			return new XrcdlDocumentImplementation();
		}

		/// <summary>
		///  この文書のファイルへの絶対パスを取得します。
		/// </summary>
		public string? FilePath { get; private set; }

		/// <summary>
		///  この文書の名前を取得または設定します。
		/// </summary>
		public string Name
		{
			get => this.GetMetadata().Name;
			set => this.GetMetadata().Name = value;
		}

		/// <summary>
		///  現在のインスタンスを返します。
		/// </summary>
		public XrcdlDocument Document => this;

		internal XrcdlDocument() { }

		/// <summary>
		///  拡張可能な規則/規約記述情報を読み込みます。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		/// <exception cref="System.IO.InvalidDataException"/>
		/// <exception cref="System.IO.IOException"/>
		public void Load()
		{
			if (this.FilePath is null) {
				throw new InvalidOperationException(Resources.XrcdlDocument_Load_InvalidOperationException);
			}
			try {
				// this.FilePath の再設定を防ぐ
				using (var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					this.Load(fs);
				}
			} catch (Exception xe) when (xe is XmlException || xe is XmlSchemaException) {
				throw new InvalidDataException(Resources.XrcdlDocument_Load_InvalidDataException, xe);
			} catch (Exception e) {
				throw new IOException(Resources.XrcdlDocument_Load_IOException, e);
			}
		}

		/// <summary>
		///  指定されたファイルから拡張可能な規則/規約記述情報を読み込みます。
		/// </summary>
		/// <param name="filename">読み込み元のファイルの名前です。</param>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.Xml.XmlException"/>
		/// <exception cref="System.Xml.Schema.XmlSchemaException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.IO.PathTooLongException"/>
		/// <exception cref="System.IO.FileNotFoundException"/>
		/// <exception cref="System.IO.DirectoryNotFoundException"/>
		/// <exception cref="System.NotSupportedException"/>
		/// <exception cref="System.UnauthorizedAccessException"/>
		/// <exception cref="System.Security.SecurityException"/>
		public void Load(string filename)
		{
			filename.EnsureNotNull(nameof(filename));
			string path = Path.GetFullPath(filename);
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				this.Load(fs);
				this.FilePath = path;
			}
		}

		/// <summary>
		///  指定されたストリームから拡張可能な規則/規約記述情報を読み込みます。
		/// </summary>
		/// <param name="stream">読み込み元のストリームです。</param>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.Xml.XmlException"/>
		/// <exception cref="System.Xml.Schema.XmlSchemaException"/>
		public void Load(Stream stream)
		{
			stream.EnsureNotNull(nameof(stream));
			using (var sr = new StreamReader(stream, _enc, true, -1, true)) {
				this.Load(sr);
			}
		}

		/// <summary>
		///  指定されたテキストリーダーから拡張可能な規則/規約記述情報を読み込みます。
		/// </summary>
		/// <param name="reader">読み込み元のテキストリーダーです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.Xml.XmlException"/>
		/// <exception cref="System.Xml.Schema.XmlSchemaException"/>
		public void Load(TextReader reader)
		{
			reader.EnsureNotNull(nameof(reader));
			using (var xr = XmlReader.Create(reader, _reader_settings)) {
				this.Load(xr);
			}
		}

		/// <summary>
		///  指定されたXMLリーダーから拡張可能な規則/規約記述情報を読み込みます。
		/// </summary>
		/// <param name="reader">読み込み元のXMLリーダーです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Xml.XmlException"/>
		/// <exception cref="System.Xml.Schema.XmlSchemaException"/>
		public void Load(XmlReader reader)
		{
			reader.EnsureNotNull(nameof(reader));
			this.LoadCore(reader);
		}

		protected private abstract void LoadCore(XmlReader reader);

		/// <summary>
		///  拡張可能な規則/規約記述情報を書き込みます。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		/// <exception cref="System.IO.InvalidDataException"/>
		/// <exception cref="System.IO.IOException"/>
		public void Save()
		{
			if (this.FilePath is null) {
				throw new InvalidOperationException(Resources.XrcdlDocument_Save_InvalidOperationException);
			}
			try {
				// this.FilePath の再設定を防ぐ
				using (var fs = new FileStream(this.FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
					this.Save(fs);
				}
			} catch (XmlException xe) {
				throw new InvalidDataException(Resources.XrcdlDocument_Save_InvalidDataException, xe);
			} catch (Exception e) {
				throw new IOException(Resources.XrcdlDocument_Save_IOException, e);
			}
		}

		/// <summary>
		///  指定されたファイルから拡張可能な規則/規約記述情報を書き込みます。
		/// </summary>
		/// <param name="filename">書き込み先のファイルの名前です。</param>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.Xml.XmlException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.IO.PathTooLongException"/>
		/// <exception cref="System.IO.FileNotFoundException"/>
		/// <exception cref="System.IO.DirectoryNotFoundException"/>
		/// <exception cref="System.NotSupportedException"/>
		/// <exception cref="System.UnauthorizedAccessException"/>
		/// <exception cref="System.Security.SecurityException"/>
		public void Save(string filename)
		{
			filename.EnsureNotNull(nameof(filename));
			string path = Path.GetFullPath(filename);
			using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
				this.Save(fs);
				this.FilePath = path;
			}
		}

		/// <summary>
		///  指定されたストリームから拡張可能な規則/規約記述情報を書き込みます。
		/// </summary>
		/// <param name="stream">書き込み先のストリームです。</param>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.Xml.XmlException"/>
		public void Save(Stream stream)
		{
			stream.EnsureNotNull(nameof(stream));
			using (var sw = new StreamWriter(stream, _enc, -1, true)) {
				this.Save(sw);
			}
		}

		/// <summary>
		///  指定されたテキストライターへ拡張可能な規則/規約記述情報を書き込みます。
		/// </summary>
		/// <param name="writer">書き込み先のテキストライターです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.Xml.XmlException"/>
		public void Save(TextWriter writer)
		{
			writer.EnsureNotNull(nameof(writer));
			using (var xw = XmlWriter.Create(writer, GetWriterSettings(writer))) {
				this.Save(xw);
			}
		}

		/// <summary>
		///  指定されたXMLライターへ拡張可能な規則/規約記述情報を書き込みます。
		/// </summary>
		/// <param name="writer">書き込み先のXMLライターです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Xml.XmlException"/>
		public void Save(XmlWriter writer)
		{
			writer.EnsureNotNull(nameof(writer));
			this.SaveCore(writer);
		}

		protected private abstract void SaveCore(XmlWriter writer);

		/// <summary>
		///  現在の文書のメタ情報を取得します。
		/// </summary>
		/// <returns>メタ情報を格納した<see cref="Exrecodel.XrcdlMetadata"/>オブジェクトです。</returns>
		public abstract XrcdlMetadata GetMetadata();

		/// <summary>
		///  現在の文書の根要素を取得します。
		/// </summary>
		/// <returns>根要素を表す<see cref="Exrecodel.XrcdlRoot"/>オブジェクトです。</returns>
		public abstract XrcdlRoot GetRootNode();

		/// <summary>
		///  現在の文書の変換処理を行うオブジェクトを取得します。
		/// </summary>
		/// <returns><see cref="Exrecodel.IXrcdlConverter"/>へ変換可能なオブジェクトです。</returns>
		public abstract IXrcdlConverter GetConverter();
	}
}
