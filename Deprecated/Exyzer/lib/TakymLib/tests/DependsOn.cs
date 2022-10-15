/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using TakymLib;

namespace TakymLibTests
{
	public static class DependsOn
	{
		public static void Language(string? locale = null)
		{
			LanguageUtility.SetCulture(locale);
		}

		public static void DefaultCulture(string? locale = null)
		{
			CultureInfo.DefaultThreadCurrentCulture = LanguageUtility.GetCulture(locale);
		}

		public static void DefaultUICulture(string? locale = null)
		{
			CultureInfo.DefaultThreadCurrentUICulture = LanguageUtility.GetCulture(locale);
		}

		public static void CurrentCulture(string? locale = null)
		{
			CultureInfo.CurrentCulture = LanguageUtility.GetCulture(locale);
		}

		public static void CurrentUICulture(string? locale = null)
		{
			CultureInfo.CurrentUICulture = LanguageUtility.GetCulture(locale);
		}

		public static void Windows(Action test, [CallerMemberName()] string? name = null)
		{
			if (OperatingSystem.IsWindows()) {
				test();
			} else {
				if (string.IsNullOrEmpty(name)) {
					name = "This test";
				}
				Console.WriteLine(name + " is only supported on Windows.");
			}
		}
	}
}
