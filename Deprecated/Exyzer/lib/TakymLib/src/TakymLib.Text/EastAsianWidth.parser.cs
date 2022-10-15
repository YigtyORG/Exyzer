/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TakymLib.Text
{
	partial class EastAsianWidth
	{
		/// <summary>
		///  最新の東アジアの文字幅の定義データをダウンロードのダウンロード元のアドレスを取得します。
		/// </summary>
		/// <remarks>
		///  The Unicode Consortium が定める<see href="http://www.unicode.org/terms_of_use.html">利用規約</see>に従って利用してください。
		/// </remarks>
		public const string LatestDefinitionUrl = "https://www.unicode.org/Public/UCD/latest/ucd/EastAsianWidth.txt";

		/// <summary>
		///  最新の東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <remarks>
		///  The Unicode Consortium が定める<see href="http://www.unicode.org/terms_of_use.html">利用規約</see>に従って利用してください。
		/// </remarks>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		public static CustomEastAsianWidth DownloadLatestDefinition()
		{
			return DownloadDefinitionFrom(LatestDefinitionUrl);
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す文字列を指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクト返します。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(string url)
		{
			using (var hc = new HttpClient()) {
				return DownloadDefinitionFrom(new Uri(url), hc);
			}
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す文字列を指定します。</param>
		/// <param name="client">ダウンロードに利用するクライアントを指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		[Obsolete("WebClient は廃止されました。", DiagnosticId = "TakymLib_WebClient")]
		public static CustomEastAsianWidth DownloadDefinitionFrom(string url, WebClient client)
		{
			return DownloadDefinitionFrom(new Uri(url), client);
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す<see cref="System.Uri"/>オブジェクトを指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(Uri url)
		{
			using (var hc = new HttpClient()) {
				return DownloadDefinitionFrom(url, hc);
			}
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す<see cref="System.Uri"/>オブジェクトを指定します。</param>
		/// <param name="client">ダウンロードに使用するクライアントを指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		[Obsolete("WebClient は廃止されました。", DiagnosticId = "TakymLib_WebClient")]
		public static CustomEastAsianWidth DownloadDefinitionFrom(Uri url, WebClient client)
		{
			url   .EnsureNotNull();
			client.EnsureNotNull();
			return Parse(client.DownloadString(url));
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す<see cref="System.Uri"/>オブジェクトを指定します。</param>
		/// <param name="client">ダウンロードに利用するクライアントを指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(Uri url, HttpClient client)
		{
			url   .EnsureNotNull();
			client.EnsureNotNull();
			return Parse(client.GetStringAsync(url).GetAwaiter().GetResult());
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データを非同期的にダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す<see cref="System.Uri"/>オブジェクトを指定します。</param>
		/// <param name="client">ダウンロードに利用するクライアントを指定します。</param>
		/// <returns>
		///  解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを含む、
		///  この処理の非同期操作を返します。
		/// </returns>
		public static async ValueTask<CustomEastAsianWidth> DownloadDefinitionFromAsync(Uri url, HttpClient client)
		{
			url   .EnsureNotNull();
			client.EnsureNotNull();

			var stream = await client.GetStreamAsync(url).ConfigureAwait(false);
			await using (stream.ConfigureAwait(false)) {
				return await ParseAsync(stream);
			}
		}

		/// <summary>
		///  指定された文字列を解析します。
		/// </summary>
		/// <param name="s">解析する文字列を指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		public static CustomEastAsianWidth Parse(string? s)
		{
			using (var sr = new StringReader(s ?? string.Empty)) {
				return Parse(sr);
			}
		}

		/// <summary>
		///  指定されたストリームから入力される文字列を解析します。
		/// </summary>
		/// <param name="stream">ストリームを指定します。</param>
		/// <param name="encoding">文字コードを指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static CustomEastAsianWidth Parse(Stream stream, Encoding? encoding = null)
		{
			stream.EnsureNotNull();
			using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8, true, -1, true)) {
				return Parse(sr);
			}
		}

		/// <summary>
		///  指定されたテキストリーダーから入力される文字列を解析します。
		/// </summary>
		/// <param name="tr">テキストリーダーを指定します。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static CustomEastAsianWidth Parse(TextReader tr)
		{
			tr.EnsureNotNull();
			var ranges = new List<(int Start, int End, EastAsianWidthType Type)>();
			while (tr.ReadLine() is not null and string line) {
				ranges.Add(ParseLine(line));
			}
			return new(ranges.AsReadOnly());
		}

		/// <summary>
		///  指定されたストリームから入力される文字列を非同期的に解析します。
		/// </summary>
		/// <param name="stream">ストリームを指定します。</param>
		/// <param name="encoding">文字コードを指定します。</param>
		/// <returns>
		///  解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトを含む、
		///  この処理の非同期操作を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async ValueTask<CustomEastAsianWidth> ParseAsync(Stream stream, Encoding? encoding = null)
		{
			stream.EnsureNotNull();
			using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8, true, -1, true)) {
				var ranges = new List<(int Start, int End, EastAsianWidthType Type)>();
				while (await sr.ReadLineAsync().ConfigureAwait(false) is not null and string line) {
					ranges.Add(ParseLine(line));
				}
				return new(ranges.AsReadOnly());
			}
		}

		/// <summary>
		///  指定された行を解析し、範囲で表された東アジアの文字幅の列挙値へ変換します。
		/// </summary>
		/// <param name="line">解析する一行の文字列を指定します。</param>
		/// <returns>範囲で表された東アジアの文字幅の列挙値を返します。</returns>
		public static (int Start, int End, EastAsianWidthType Type) ParseLine(string? line)
		{
			if (string.IsNullOrEmpty(line)) {
				return default;
			}
			int start = 0, end = 0, flags = 0;
			var type = EastAsianWidthType.Invalid;
			for (int i = 0; i < line!.Length; ++i) {
				char ch = line[i];
				if (ch == ' ' || ch == '\t') {
					continue;
				}
				if ((flags & 2) == 2) {
					switch (ch) {
					case 'A':
						type = EastAsianWidthType.Ambiguous;
						break;
					case 'F':
						type = EastAsianWidthType.FullWidth;
						break;
					case 'H':
						type = EastAsianWidthType.HalfWidth;
						break;
					case 'N':
						type = EastAsianWidthType.Neutral;
						break;
					case 'a':
						type = EastAsianWidthType.Narrow;
						break;
					case 'W':
						type = EastAsianWidthType.Wide;
						break;
					case '#':
						goto end;
					}
				} else {
					switch (ch) {
					case >= '0' and <= '9':
						if ((flags & 1) == 1) {
							end <<= 4;
							end  |= ch - '0';
						} else {
							start <<= 4;
							start  |= ch - '0';
						}
						break;
					case >= 'A' and <= 'F':
						if ((flags & 1) == 1) {
							end <<= 4;
							end  |= (ch - 'A') + 0x0A;
						} else {
							start <<= 4;
							start  |= (ch - 'A') + 0x0A;
						}
						break;
					case >= 'a' and <= 'f':
						if ((flags & 1) == 1) {
							end <<= 4;
							end  |= (ch - 'a') + 0x0A;
						} else {
							start <<= 4;
							start  |= (ch - 'a') + 0x0A;
						}
						break;
					case '.':
						flags |= 1;
						break;
					case ';':
						flags |= 2;
						break;
					case '#':
						goto end;
					}
				}
			}
end:
			if ((flags & 1) == 1) {
				return (start, end, type);
			} else {
				return (start, start, type);
			}
		}
	}
}
