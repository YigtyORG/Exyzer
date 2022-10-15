/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ywando.Properties;

namespace Ywando.Globalization
{
	/// <summary>
	///  言語情報を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class LanguageData : DisposableBase
	{
		#region 静的

		/// <summary>
		///  既定の言語情報ファイルを格納しているディレクトリへのパスを取得します。
		/// </summary>
		public static readonly string DefaultLanguageDataPath = Path.Combine(AppContext.BaseDirectory, nameof(Resources), "Translations");

		private static readonly Dictionary<string, LanguageData> _langs;

		/// <summary>
		///  型'<see cref="Ywando.Globalization.LanguageData"/>'の静的インスタンスを生成します。
		/// </summary>
		static LanguageData()
		{
			_langs = new();
			_      = DefaultLanguageData.Instance;
		}

		/// <summary>
		///  現在のカルチャの言語情報を取得します。
		/// </summary>
		/// <returns>言語情報を表すオブジェクトを含む非同期操作を返します。</returns>
		/// <exception cref="System.IO.IOException"/>
		public static ValueTask<LanguageData> GetAsync()
		{
			return GetAsync(CultureInfo.CurrentCulture);
		}

		/// <summary>
		///  言語情報を取得します。
		/// </summary>
		/// <param name="cultureInfo">取得する言語情報のカルチャです。</param>
		/// <returns>言語情報を表すオブジェクトを含む非同期操作を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.IO.IOException"/>
		public static async ValueTask<LanguageData> GetAsync(CultureInfo cultureInfo)
		{
			cultureInfo.EnsureNotNull(nameof(cultureInfo));

			LanguageData? ld;
			string name = cultureInfo.Name;
			lock (_langs) {
				_langs.TryGetValue(name, out ld);
			}
			if (ld is null) {
				ld = await JsonLanguageData.LoadFromDirectoryAsync(DefaultLanguageDataPath, cultureInfo);
				if (ld is null) {
					return DefaultLanguageData.Instance;
				}
				if (!string.IsNullOrEmpty(ld.ParentLanguage)) {
					await GetAsync(CultureInfo.GetCultureInfo(ld.ParentLanguage));
				}
				return ld;
			} else {
				return ld;
			}
		}

		/// <summary>
		///  メモリ上に読み込まれている全ての言語情報を取得します。
		/// </summary>
		/// <returns>言語情報の配列です。</returns>
		public static LanguageData[] GetLoadedLanguages()
		{
			lock (_langs) {
				return _langs.Values.ToArray();
			}
		}

		#endregion

		#region 動的

		private          ConcurrentDictionary<string, string>? _cache;
		private readonly object                                _cache_lock;

		/// <summary>
		///  この言語のカルチャを取得します。
		/// </summary>
		public CultureInfo CultureInfo { get; }

		/// <summary>
		///  親言語のカルチャ名を取得します。
		/// </summary>
		public virtual string ParentLanguage => this.CultureInfo.Parent.Name;

		/// <summary>
		///  文字列のキャッシュを行うかどうかを表す値を取得または設定します。
		/// </summary>
		/// <remarks>
		///  書式設定オブジェクトが指定された場合は常に無効になります。
		/// </remarks>
		/// <returns>
		///  文字列のキャッシュが有効である場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		/// <value>
		///  文字列のキャッシュを有効化する場合は<see langword="true"/>を指定します。
		///  無効化にする場合は<see langword="false"/>を指定します。
		/// </value>
		/// <exception cref="System.ObjectDisposedException"/>
		protected bool EnableCache
		{
			get => _cache is not null;
			set
			{
				this.EnsureNotDisposed();
				if (value) {
					lock (_cache_lock) {
						if (_cache is null) {
							_cache = new();
						}
					}
				} else {
					lock (_cache_lock) {
						_cache?.Clear();
						_cache = null;
					}
				}
			}
		}

		/// <summary>
		///  型'<see cref="Ywando.Globalization.LanguageData"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="cultureInfo">新しい言語情報のカルチャです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected LanguageData(CultureInfo cultureInfo)
		{
			cultureInfo.EnsureNotNull(nameof(cultureInfo));
			_cache_lock      = new();
			this.CultureInfo = cultureInfo;

			string name = cultureInfo.Name;
			lock (_langs) {
				if (!_langs.ContainsKey(name)) {
					_langs.Add(name, this);
				}
			}
		}

		/// <summary>
		///  翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="key">文字列に関連付けられたキー名です。</param>
		/// <param name="args">書式設定オブジェクトの配列です。</param>
		/// <returns>翻訳済みの文字列を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.FormatException"/>
		public string GetLocalizedText(string key, params object[] args)
		{
			this.EnsureNotDisposed();
			key .EnsureNotNull(nameof(key));
			args.EnsureNotNull(nameof(args));

			var cache = _cache;

			if (cache is null || args.Length != 0) {
				return this.GetLocalizedTextInternal(key, args);
			} else {
				return cache.GetOrAdd(key, key => this.GetLocalizedTextInternal(key, Array.Empty<object>()));
			}
		}

		private string GetLocalizedTextInternal(string key, params object[] args)
		{
			if (this.TryGetLocalizedTextCore(key, out string? result, args)) {
				return result;
			} else {
				LanguageData? ld;
				string name = this.ParentLanguage;
				lock (_langs) {
					_langs.TryGetValue(name, out ld);
				}
				if (ld is null) {
					return MakeDefaultText(key, args);
				} else {
					return ld.GetLocalizedText(key, args);
				}
			}
		}

		/// <summary>
		///  上書きされた場合、翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="key">文字列に関連付けられたキー名です。</param>
		/// <param name="result">翻訳済みの文字列を返します。</param>
		/// <param name="args">書式設定オブジェクトの配列です。</param>
		/// <returns>文字列が存在する場合は<see langword="true"/>、存在しない場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.FormatException">
		///  翻訳済みの文字列の書式が誤っている場合に発生させる事ができます。
		/// </exception>
		protected abstract bool TryGetLocalizedTextCore(string key, [NotNullWhen(true)] out string? result, params object[] args);

		private static string MakeDefaultText(string key, params object[] args)
		{
			var sb = new StringBuilder();
			sb.Append(key);
			for (int i = 0; i < args.Length; ++i) {
				sb.Append(' ').Append(args[i]);
			}
			return sb.ToString();
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}

			lock (_cache_lock) {
				_cache?.Clear();
				_cache = null;
			}

			string name = this.CultureInfo.Name;
			lock (_langs) {
				if (_langs.TryGetValue(name, out var ld) && this == ld) {
					_langs.Remove(name);
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region 子クラス

		private sealed class DefaultLanguageData : LanguageData
		{
			internal static readonly DefaultLanguageData Instance = new();

			private DefaultLanguageData() : base(CultureInfo.GetCultureInfo(string.Empty))
			{
				this.EnableCache = true;
			}

			protected override bool TryGetLocalizedTextCore(string key, [NotNullWhen(true)] out string? result, params object[] args)
			{
				result = MakeDefaultText(key, args);
				return true;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing) {
					throw new InvalidOperationException(Resources.DefaultLanguageData_Dispose_InvalidOperationException);
				}
			}
		}

		#endregion
	}
}
