/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Diagnostics.CodeAnalysis;

namespace Exyzer
{
	/// <summary>
	///  <see cref="Exyzer"/>の処理装置を提供します。
	/// </summary>
	public interface IProcessor
	{
		/// <summary>
		///  現在の処理装置に含まれるレジスタの数を取得します。
		/// </summary>
		public int RegisterCount { get; }

		/// <summary>
		///  実行を開始します。
		/// </summary>
		public void RunStart();

		/// <summary>
		///  次の処理を一つだけ実行します。
		/// </summary>
		public void RunNext();

		/// <summary>
		///  書式設定されたレジスタの値を取得します。
		/// </summary>
		/// <param name="index">レジスタ番号を指定します。</param>
		/// <param name="format">値の書式の種類を指定します。</param>
		/// <param name="value">レジスタ値の文字列表現を返します。</param>
		/// <returns>成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TryGetFormattedRegisterValue(int index, ValueFormat format, [NotNullWhen(true)] out string? value);

		/// <summary>
		///  書式設定された文字列をレジスタに設定します。
		/// </summary>
		/// <param name="index">レジスタ番号を指定します。</param>
		/// <param name="format">値の書式の種類を指定します。</param>
		/// <param name="value">設定する値の文字列表現を指定します。</param>
		/// <returns>成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TrySetFormattedRegisterValue(int index, ValueFormat format, string? value);
	}
}
