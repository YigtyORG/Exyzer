/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;

namespace TakymLib
{
	/// <summary>
	///  型'<see cref="System.IFormatProvider"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class FormatProviderExtensions
	{
		/// <summary>
		///  指定された<paramref name="provider"/>から<see cref="System.Globalization.CultureInfo"/>を取得します。
		/// </summary>
		/// <param name="provider">書式設定プロバイダを指定します。</param>
		/// <returns><see cref="System.Globalization.CultureInfo"/>オブジェクトを返します。</returns>
		public static CultureInfo GetCultureInfo(this IFormatProvider? provider)
		{
			if (provider is null) {
				return CultureInfo.CurrentCulture;
			}
			if (provider is CultureInfo cinfo1) {
				return cinfo1;
			}
			if (provider.GetFormat(typeof(CultureInfo)) is CultureInfo cinfo2) {
				return cinfo2;
			}
			return CultureInfo.CurrentCulture;
		}

#if false
		private sealed class NullFormatProvider : IFormatProvider
		{
			internal static readonly NullFormatProvider _inst = new();

			private NullFormatProvider() { }

			public object? GetFormat(Type? formatType)
			{
				if (formatType == typeof(CultureInfo)) {
					return CultureInfo.CurrentCulture;
				}
				if (formatType == typeof(NullFormatProvider)) {
					return this;
				}
				return null;
			}
		}
#endif
	}
}
