/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.IO
{
	/// <summary>
	///  ファイルシステム内に保持される項目を列挙します。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	public readonly struct FileSystemEntryEnumerator : IEnumerable<PathString>, IEnumerator<PathString>, IAsyncEnumerable<PathString>, IAsyncEnumerator<PathString>
	{
		private readonly IEnumerator<string> _entries;

		/// <summary>
		///  現在選択されている項目を取得します。
		/// </summary>
		public PathString Current => PathStringPool.Get(_entries.Current);

		object? IEnumerator.Current => this.Current;

		internal FileSystemEntryEnumerator(IEnumerable<string> entries)
		{
			_entries = entries.GetEnumerator();
		}

		/// <summary>
		///  次の項目を選択します。
		/// </summary>
		/// <returns>
		///  次の項目が存在する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public bool MoveNext()
		{
			return _entries.MoveNext();
		}

		/// <summary>
		///  次の項目を非同期で選択します。
		/// </summary>
		/// <returns>
		///  次の項目が存在するかどうかを表す論理値を含むこの処理の非同期操作を返します。
		/// </returns>
		public async ValueTask<bool> MoveNextAsync()
		{
			if (_entries is IAsyncEnumerator<string> asyncEnumerator) {
				return await asyncEnumerator.MoveNextAsync();
			} else {
				return _entries.MoveNext();
			}
		}

		/// <summary>
		///  列挙しを初期化します。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		public void Reset()
		{
			_entries.Reset();
		}

		/// <summary>
		///  現在のオブジェクトを破棄します。
		/// </summary>
		public void Dispose()
		{
			_entries.Dispose();
		}

		/// <summary>
		///  現在のオブジェクトを破棄します。
		/// </summary>
		/// <returns>この処理の非同期操作です。</returns>
		public async ValueTask DisposeAsync()
		{
			if (_entries is IAsyncDisposable asyncDisposable) {
				await asyncDisposable.ConfigureAwait(false).DisposeAsync();
			} else {
				_entries.Dispose();
			}
		}

		/// <summary>
		///  反復処理を行う列挙子を取得します。
		/// </summary>
		/// <returns>現在のインスタンスを返します。</returns>
		public FileSystemEntryEnumerator GetEnumerator()
		{
			return this;
		}

		/// <summary>
		///  非同期で反復処理を行う列挙子を取得します。
		/// </summary>
		/// <param name="cancellationToken">利用されません。</param>
		/// <returns>現在のインスタンスを返します。</returns>
		public FileSystemEntryEnumerator GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return this;
		}

		IEnumerator<PathString> IEnumerable<PathString>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IAsyncEnumerator<PathString> IAsyncEnumerable<PathString>.GetAsyncEnumerator(CancellationToken cancellationToken)
		{
			return this.GetAsyncEnumerator(cancellationToken);
		}
	}
}
