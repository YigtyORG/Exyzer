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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.CommandLine
{
	/// <summary>
	///  独自仕様のコマンド行を字句解析し、引数に分解します。
	/// </summary>
	/// <remarks>
	///  <para>
	///   構文解析と意味解析をし逆直列化を行う場合は、
	///   <see cref="TakymLib.CommandLine.CommandLineParser"/>
	///   または<see cref="TakymLib.CommandLine.CommandLineConverter"/>を使用してください。
	///  </para>
	///  <para>
	///   コマンド行の書式を変更する場合は<see cref="TakymLib.CommandLine.CommandLineLexer.ScanAsyncCore(TextReader)"/>を上書きしてください。
	///  </para>
	/// </remarks>
	public class CommandLineLexer
	{
		/// <summary>
		///  指定された文字列をコマンド行として解析します。
		/// </summary>
		/// <param name="s">解析する文字列を指定します。</param>
		/// <returns><see cref="TakymLib.CommandLine.CommandLineLexer.ScanAsyncResult"/>オブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public ScanAsyncResult ScanAsync(string? s)
		{
			return new(this, s);
		}

		/// <summary>
		///  指定された<see cref="System.IO.TextReader"/>から文字列を読み取り解析します。
		/// </summary>
		/// <param name="reader">解析する文字列を格納したリーダーを指定します。</param>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public IAsyncEnumerable<string> ScanAsync(TextReader reader)
		{
			reader.EnsureNotNull();
			return this.ScanAsyncCore(reader);
		}

		/// <summary>
		///  上書きされた場合、コマンド行の解析を行います。
		/// </summary>
		/// <param name="tr">解析する文字列を格納したリーダーを指定します。</param>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		protected virtual async IAsyncEnumerable<string> ScanAsyncCore(TextReader tr)
		{
			// TODO: リダイレクトとパイプを実装する
			int depth = 0;
			var sb    = new StringBuilder();

read_next_line:
			string? line = await tr.ReadLineAsync();
			if (line is null) {
				yield break;
			}
			bool escaping = false;
			for (int i = 0; i < line.Length; ++i) {
				char ch = line[i];
				switch (ch) {
				case ' ':
				case '\t':
				case '\v':
				case '\r':
				case '\n':
					if (escaping) {
						sb.Append(ch);
					} else if (sb.Length > 0) {
						yield return sb.ToString();
						sb.Clear();
					}
					break;
				case '[':
				case ']':
					if (escaping) {
						sb.Append(ch);
					} else {
						depth += ch == '[' ? 1 : -1;
					}
					break;
				case '\\':
					if (escaping) {
						sb.Append(ch);
					} else if ((i + 1) < line.Length && line[i + 1] == '\\') {
						sb.Append(ch);
						++i;
					} else {
						sb.Append(Environment.NewLine);
					}
					break;
				case '\"':
					if ((i + 1) < line.Length && line[i + 1] == '\"') {
						sb.Append(ch);
						++i;
					} else {
						escaping = !escaping;
					}
					break;
				case '\0':
					yield break;
				default:
					sb.Append(ch);
					break;
				}
			}
			if (sb.Length > 0) {
				yield return sb.ToString();
			}
			if (depth > 0) {
				sb.Clear();
				goto read_next_line;
			}
		}

		/// <summary>
		///  <see cref="TakymLib.CommandLine.CommandLineLexer.ScanAsync(string?)"/>の戻り値を表します。
		///  このクラスは読み取り専用構造体です。
		/// </summary>
		public readonly struct ScanAsyncResult : IAsyncEnumerable<string>
		{
			private readonly CommandLineLexer? _owner;
			private readonly string?           _text;

			internal ScanAsyncResult(CommandLineLexer owner, string? text)
			{
				_owner = owner;
				_text  = text;
			}

			/// <summary>
			///  コマンド行引数を非同期的に反復処理する列挙子を取得します。
			/// </summary>
			/// <param name="cancellationToken">
			///  処理を中断する為の<see cref="System.Threading.CancellationToken"/>を指定します。
			///  この引数は省略可能です。
			/// </param>
			/// <returns><see cref="TakymLib.CommandLine.CommandLineLexer.ScanAsyncResult.Enumerator"/>オブジェクトを返します。</returns>
			public Enumerator GetAsyncEnumerator(CancellationToken cancellationToken = default)
			{
				return new(_owner, _text, cancellationToken);
			}

			IAsyncEnumerator<string> IAsyncEnumerable<string>.GetAsyncEnumerator(CancellationToken cancellationToken)
			{
				return this.GetAsyncEnumerator(cancellationToken);
			}

			/// <summary>
			///  コマンド行引数を非同期的に反復処理する列挙子を表します。
			///  このクラスは読み取り専用構造体です。
			/// </summary>
			public readonly struct Enumerator : IAsyncEnumerator<string>
			{
				private readonly IAsyncEnumerator<string>? _enumerator;
				private readonly StringReader?             _sr;
				private readonly CancellationToken         _ct;

				/// <inheritdoc/>
				public string Current => _enumerator!.Current;

				internal Enumerator(CommandLineLexer? owner, string? text, CancellationToken cancellationToken)
				{
					_sr         = new StringReader(text ?? string.Empty);
					_ct         = cancellationToken;
					_enumerator = owner?.ScanAsyncCore(_sr).GetAsyncEnumerator(cancellationToken);
				}

				/// <inheritdoc/>
				public ValueTask<bool> MoveNextAsync()
				{
					if (_ct.IsCancellationRequested) {
						return ValueTask.FromCanceled<bool>(_ct);
					}
					if (_enumerator is null) {
						return ValueTask.FromResult(false);
					}
					return _enumerator.MoveNextAsync();
				}

				/// <inheritdoc/>
				public async ValueTask DisposeAsync()
				{
					if (_enumerator is not null) {
						await _enumerator.DisposeAsync();
					}
					_sr?.Dispose();
				}
			}
		}
	}
}
