/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;
using System.Globalization;

namespace TakymLib.IO
{
	internal sealed class PathStringConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
		{
			return typeof(string) == sourceType || base.CanConvertFrom(context, sourceType);
		}

		public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
		{
			if (value is PathString path) {
				return path;
			} else if (value is string text) {
				return PathStringPool.Get(text);
			} else {
				return base.ConvertFrom(context, culture, value);
			}
		}

		public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
		{
			return typeof(PathString) == destinationType || base.CanConvertTo(context, destinationType);
		}

		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
		{
			if (typeof(PathString) == destinationType) {
				if (value is PathString path) {
					return path;
				} else if (value is string text) {
					return PathStringPool.Get(text);
				} else {
					return base.ConvertTo(context, culture, value, destinationType);
				}
			} else if (typeof(string) == destinationType) {
				return value?.ToString() ?? string.Empty;
			} else {
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
