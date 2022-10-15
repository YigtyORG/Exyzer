/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.IO;
using TakymLib.Text;

namespace TakymLib.IO
{
	/// <summary>
	///  <see cref="TakymLib.IO.PathString"/>の書式設定を行います。
	/// </summary>
	public class PathStringFormatter : IFormatProvider, ICustomFormatter
	{
		internal static readonly PathStringFormatter _inst     = new();
		private         readonly IFormatProvider?    _provider;

		/// <summary>
		///  既定のカルチャを使用して、
		///  型'<see cref="TakymLib.IO.PathStringFormatter"/>'の新しいインスタンスを生成します。
		/// </summary>
		public PathStringFormatter()
		{
			_provider = CultureInfo.CurrentCulture;
		}

		/// <summary>
		///  既定の書式設定サービスを提供する事ができるオブジェクトを指定して、
		///  型'<see cref="TakymLib.IO.PathStringFormatter"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="formatProvider">書式設定プロバイダを指定します。</param>
		public PathStringFormatter(IFormatProvider formatProvider)
		{
			_provider = formatProvider;
		}

		/// <summary>
		///  指定された型の書式設定サービスを取得します。
		/// </summary>
		/// <param name="formatType">書式設定サービスの種類を指定します。</param>
		/// <returns>書式設定サービスを表すオブジェクトを返します。</returns>
		public virtual object? GetFormat(Type? formatType)
		{
			if (formatType?.IsAssignableFrom(this.GetType()) ?? false) {
				return this;
			} else {
				return _provider?.GetFormat(formatType);
			}
		}

		/// <summary>
		///  指定されたオブジェクトを書式設定し文字列へ変換します。
		/// </summary>
		/// <remarks>
		///  記号対応表：
		///  <list type="bullet">
		///   <listheader>
		///    <term>記号</term>
		///    <description>変換結果</description>
		///   </listheader>
		///   <item>
		///    <term>B</term>
		///    <description>基底パス</description>
		///   </item>
		///   <item>
		///    <term>D</term>
		///    <description>親ディレクトリ名</description>
		///   </item>
		///   <item>
		///    <term>F</term>
		///    <description>拡張子を含むファイル名</description>
		///   </item>
		///   <item>
		///    <term>N</term>
		///    <description>拡張子を除くファイル名</description>
		///   </item>
		///   <item>
		///    <term>O</term>
		///    <description>コンストラクタに渡されたパス文字列</description>
		///   </item>
		///   <item>
		///    <term>P</term>
		///    <description>絶対パス</description>
		///   </item>
		///   <item>
		///    <term>R</term>
		///    <description>ルートディレクトリ</description>
		///   </item>
		///   <item>
		///    <term>U</term>
		///    <description>URI形式のパス文字列</description>
		///   </item>
		///   <item>
		///    <term>X</term>
		///    <description>拡張子</description>
		///   </item>
		///   <item>
		///    <term>/</term>
		///    <description>パス区切り記号</description>
		///   </item>
		///   <item>
		///    <term>\</term>
		///    <description>次の記号文字を無視</description>
		///   </item>
		///  </list>
		/// </remarks>
		/// <param name="format">書式設定文字列を指定します。</param>
		/// <param name="arg">文字列へ変換するオブジェクトを指定します。</param>
		/// <param name="formatProvider">書式設定サービスを提供する書式設定プロバイダを指定します。</param>
		/// <returns>現在のパス文字列を表す可読な文字列を返します。</returns>
		public virtual string Format(string? format, object? arg, IFormatProvider? formatProvider)
		{
			if (arg is PathString path) {
				format ??= string.Empty;
				if (format.Length == 0) {
					return path.ToString();
				}
				var  formatted = BuildString.Begin();
				bool ignore    = false;
				for (int i = 0; i < format.Length; ++i) {
					char ch = format[i];
					if (ignore) {
						formatted.Append(ch);
						ignore = false;
					} else if (ch == '\\') {
						ignore = true;
					} else {
						formatted.Append(ch switch {
							'B' => path.BasePath,
							'D' => path.GetDirectoryName()?.GetFileName(),
							'F' => path.GetFileName(),
							'N' => path.GetFileNameWithoutExtension(),
							'O' => path.GetOriginalString(),
							'P' => path,
							'R' => path.GetRootPath(),
							'U' => path.AsUri().AbsoluteUri,
							'X' => path.GetExtension(),
							'/' => Path.DirectorySeparatorChar,
							_   => ch
						});
					}
				}
				return BuildString.End(formatted);
			} else {
				if (arg is IFormattable formattable) {
					return formattable.ToString(format, formatProvider ?? _provider) ?? string.Empty;
				} else {
					return arg?.ToString() ?? string.Empty;
				}
			}
		}
	}
}
