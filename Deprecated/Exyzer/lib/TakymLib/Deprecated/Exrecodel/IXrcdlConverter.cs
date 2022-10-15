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

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書の変換機能を提供します。
	/// </summary>
	public interface IXrcdlConverter : IDisposable
	{
		/// <summary>
		///  HTML文書へ変換し、指定された文字列バッファへ書き込みます。
		/// </summary>
		/// <param name="sb">変換後のHTML文書を格納するオブジェクトです。</param>
		public void ConvertToHtml(StringBuilder sb);
	}

	/// <summary>
	///  <see cref="Exrecodel"/>文書の変換を非同期的に行います。
	/// </summary>
	public interface IXrcdlAsyncConverter : IXrcdlConverter, IAsyncDisposable
	{
		/// <summary>
		///  非同期でHTML文書へ変換し、指定された文字列バッファへ書き込みます。
		/// </summary>
		/// <param name="sb">変換後のHTML文書を格納するオブジェクトです。</param>
		/// <returns>非同期操作を表すオブジェクトです。</returns>
		public Task ConvertToHtmlAsync(StringBuilder sb);
	}
}
