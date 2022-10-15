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
using TakymLib;

namespace Exrecodel.Extensions
{
	/// <summary>
	///  型'<see cref="Exrecodel.IXrcdlElement"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class XrcdlElementExtensions
	{
		/// <summary>
		///  指定された要素をHTML文書へ変換します。
		/// </summary>
		/// <param name="element">HTML文書へ変換する要素オブジェクトです。</param>
		/// <returns>HTML文書を表す文字列です。</returns>
		public static string ConvertToHtml(this IXrcdlElement element)
		{
			element.EnsureNotNull(nameof(element));
			var sb = new StringBuilder();
			using (var conv = element.GetConverter()) {
				conv.ConvertToHtml(sb);
			}
			return sb.ToString();
		}

		/// <summary>
		///  指定された要素からHTML文書への変換処理を非同期的に実行します。
		/// </summary>
		/// <param name="element">HTML文書へ変換する要素オブジェクトです。</param>
		/// <returns>HTML文書を表す文字列を格納した非同期操作です。</returns>
		public static async Task<string> ConvertToHtmlAsync(this IXrcdlElement element)
		{
			element.EnsureNotNull(nameof(element));
			var sb   = new StringBuilder();
			var conv = element.GetConverter();
			if (conv is IXrcdlAsyncConverter convAsync) {
				await using (convAsync.ConfigureAwait(false)) {
					await convAsync.ConvertToHtmlAsync(sb);
				}
			} else {
				using (conv) {
					conv.ConvertToHtml(sb);
				}
			}
			return sb.ToString();
		}
	}
}
