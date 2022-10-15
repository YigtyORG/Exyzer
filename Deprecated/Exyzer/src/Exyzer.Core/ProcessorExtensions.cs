/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Exyzer.Properties;
using TakymLib;

namespace Exyzer
{
	/// <summary>
	///  型'<see cref="Exyzer.IProcessor"/>'の機能を拡張します。
	/// </summary>
	public static class ProcessorExtensions
	{
		/// <summary>
		///  指定された処理装置から指定された番号のレジスタの名前を取得します。
		/// </summary>
		/// <param name="processor">取得元の処理装置を指定します。</param>
		/// <param name="index">レジスタ番号を指定します。</param>
		/// <returns>レジスタ名を表す文字列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		public static string GetRegisterName(this IProcessor processor, int index)
		{
			processor.EnsureNotNull(nameof(processor));
			if (processor.TryGetFormattedRegisterValue(index, ValueFormat.DisplayName, out string? result)) {
				return result;
			}
			throw new ArgumentException(
				string.Format(
					Resources.ProcessorExtensions_GetRegisterName_ArgumentException,
					index
				),
				nameof(index)
			);
		}

		/// <summary>
		///  指定された処理装置から書式設定されたレジスタの値を取得します。
		/// </summary>
		/// <param name="processor">取得元の処理装置を指定します。</param>
		/// <param name="index">レジスタ番号を指定します。</param>
		/// <param name="format">値の書式の種類を指定します。</param>
		/// <returns>レジスタ値の文字列表現を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		public static string GetFormattedRegisterValue(this IProcessor processor, int index, ValueFormat format)
		{
			processor.EnsureNotNull(nameof(processor));
			if (processor.TryGetFormattedRegisterValue(index, format, out string? result)) {
				return result;
			}
			throw new ArgumentException(string.Format(
				Resources.ProcessorExtensions_GetFormattedRegisterValue_ArgumentException,
				index, format
			));
		}

		/// <summary>
		///  指定された処理装置へ書式設定された文字列をレジスタに設定します。
		/// </summary>
		/// <param name="processor">設定先の処理装置を指定します。</param>
		/// <param name="index">レジスタ番号を指定します。</param>
		/// <param name="format">値の書式の種類を指定します。</param>
		/// <param name="value">設定する値の文字列表現を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		public static void SetFormattedRegisterValue(this IProcessor processor, int index, ValueFormat format, string? value)
		{
			processor.EnsureNotNull(nameof(processor));
			if (!processor.TrySetFormattedRegisterValue(index, format, value)) {
				throw new ArgumentException(string.Format(
					Resources.ProcessorExtensions_SetFormattedRegisterValue_ArgumentException,
					index, format, value
				));
			}
		}
	}
}
