/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ywando.Properties;

namespace Ywando.Globalization
{
	/// <summary>
	///  JSON形式の言語情報を読み込みます。
	/// </summary>
	public class JsonLanguageData : LanguageData
	{
		private static readonly JsonDocumentOptions _jdo = new() {
			AllowTrailingCommas = true,
			CommentHandling     = JsonCommentHandling.Skip,
			MaxDepth            = 64
		};

		private readonly JsonDocument _jdoc;
		private readonly JsonElement  _root;

		/// <inheritdoc/>
		public sealed override string ParentLanguage { get; }

		/// <summary>
		///  型'<see cref="Ywando.Globalization.JsonLanguageData"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="jsonData">JSON形式の言語情報です。</param>
		/// <param name="cultureInfo">新しい言語情報のカルチャです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Text.Json.JsonException"/>
		/// <exception cref="System.IO.InvalidDataException"/>
		public JsonLanguageData(string jsonData, CultureInfo cultureInfo) : base(cultureInfo)
		{
			this.EnableCache = true;
			_jdoc = JsonDocument.Parse(jsonData, _jdo);
			_root = _jdoc.RootElement;
			if (_root.ValueKind != JsonValueKind.Object) {
				throw new InvalidDataException(Resources.JsonLanguageData_InvalidDataException);
			}

			string? parentLang = null;
			if (_root.TryGetProperty("metadata", out var meta) &&
				meta.ValueKind == JsonValueKind.Object         &&
				meta.TryGetProperty("parent", out var parent)) {
				parentLang = parent.ToString();
			}
			this.ParentLanguage = parentLang ?? base.ParentLanguage;
			if (string.IsNullOrEmpty(this.ParentLanguage)) {
				this.ParentLanguage = this.CultureInfo.Name switch {
					"ja" => string.Empty,
					"en" => "ja",
					_    => "en"
				};
			}
		}

		/// <summary>
		///  JSON形式の言語情報ファイルを非同期的に読み込みます。
		/// </summary>
		/// <param name="path">JSON形式の言語情報ファイルへのパスです。</param>
		/// <param name="cultureInfo">JSON形式の言語情報ファイルのカルチャです。</param>
		/// <returns>
		///  ファイルが存在する場合は言語情報ファイルを含む非同期操作オブジェクトを返します。
		///  存在しない場合は<see langword="null"/>を含む非同期操作オブジェクトを返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.IO.IOException"/>
		public static async ValueTask<JsonLanguageData?> LoadFileAsync(string path, CultureInfo cultureInfo)
		{
			path       .EnsureNotNull(nameof(path));
			cultureInfo.EnsureNotNull(nameof(cultureInfo));

			if (File.Exists(path)) {
				try {
					var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
					await using (fs.ConfigureAwait(false)) {
						using (var sr = new StreamReader(fs, Encoding.UTF8, true)) {
							return new(await sr.ReadToEndAsync(), cultureInfo);
						}
					}
				} catch (Exception e) {
					throw new IOException(Resources.JsonLanguageData_LoadFileAsync_IOException, e);
				}
			} else {
				return null;
			}
		}

		/// <summary>
		///  JSON形式の言語情報ファイルを指定されたディレクトリから非同期的に読み込みます。
		/// </summary>
		/// <remarks>
		///  ディレクトリ内の「<c>カルチャ名.json</c>」を読み込みます。
		/// </remarks>
		/// <param name="path">JSON形式の言語情報ファイルを格納しているディレクトリへのパスです。</param>
		/// <param name="cultureInfo">JSON形式の言語情報ファイルのカルチャです。</param>
		/// <returns>
		///  ファイルが存在する場合は言語情報ファイルを含む非同期操作オブジェクトを返します。
		///  存在しない場合は<see langword="null"/>を含む非同期操作オブジェクトを返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.IO.IOException"/>
		public static ValueTask<JsonLanguageData?> LoadFromDirectoryAsync(string path, CultureInfo cultureInfo)
		{
			path       .EnsureNotNull(nameof(path));
			cultureInfo.EnsureNotNull(nameof(cultureInfo));

			if (Directory.Exists(path)) {
				try {
					return LoadFileAsync(Path.Combine(path, $"{cultureInfo.Name}.json"), cultureInfo);
				} catch (IOException e) {
					throw new IOException(Resources.JsonLanguageData_LoadFromDirectoryAsync_IOException, e.InnerException);
				} catch (Exception e) {
					throw new IOException(Resources.JsonLanguageData_LoadFromDirectoryAsync_IOException, e);
				}
			} else {
				return ValueTask.FromResult<JsonLanguageData?>(null);
			}
		}

		/// <inheritdoc/>
		protected sealed override bool TryGetLocalizedTextCore(string key, [NotNullWhen(true)] out string? result, params object[] args)
		{
			if (_root.TryGetProperty(key, out var elem)) {
				if (elem.ValueKind == JsonValueKind.Object) {
					if (elem.TryGetProperty("text", out var text)) {
						result = text.ToString();
					} else {
						result = null;
					}
				} else if (elem.ValueKind == JsonValueKind.Null || elem.ValueKind == JsonValueKind.Undefined) {
					result = null;
				} else {
					result = elem.ToString();
				}
				if (result is null) {
					return false;
				} else {
					if (args.Length > 0) {
						result = string.Format(result, args);
					}
					return true;
				}
			} else {
				result = null;
				return false;
			}
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				_jdoc.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <inheritdoc/>
		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			_jdoc.Dispose();
			await base.DisposeAsyncCore();
		}
	}
}
