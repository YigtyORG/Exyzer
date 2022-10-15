/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ywando.Globalization.Tests
{
	[TestClass()]
	public class LanguageDataTests
	{
		[TestMethod()]
		public void GetLocalizedTextTest()
		{
			using var ld1 = new LanguageDataMock1();
			Assert.AreEqual("ABCD",           ld1.GetLocalizedText("ABCD"));
			Assert.AreEqual("ABCD @@",        ld1.GetLocalizedText("ABCD", "@@"));
			Assert.AreEqual("ABCD 1234",      ld1.GetLocalizedText("ABCD", 1234));
			Assert.AreEqual("ABCD 1234 True", ld1.GetLocalizedText("ABCD", 1234, true));

			using var ld2 = new LanguageDataMock2();
			Assert.AreEqual("hello", ld2.GetLocalizedText("ABCD"));
			Assert.AreEqual("hello", ld2.GetLocalizedText("ABCD", "@@"));
			Assert.AreEqual("hello", ld2.GetLocalizedText("ABCD", 1234));
			Assert.AreEqual("hello", ld2.GetLocalizedText("ABCD", 1234, true));

			using var ld3 = new LanguageDataMock3();
			Assert.AreEqual("hello", ld3.GetLocalizedText("ABCD"));
			Assert.AreEqual("hello", ld3.GetLocalizedText("ABCD", "@@"));
			Assert.AreEqual("hello", ld3.GetLocalizedText("ABCD", 1234));
			Assert.AreEqual("hello", ld3.GetLocalizedText("ABCD", 1234, true));
		}

		private sealed class LanguageDataMock1 : LanguageData
		{
			internal LanguageDataMock1() : base(CultureInfo.GetCultureInfo("ja")) { }

			protected override bool TryGetLocalizedTextCore(string key, [NotNullWhen(true)] out string? result, params object[] args)
			{
				result = null;
				return false;
			}
		}

		private sealed class LanguageDataMock2 : LanguageData
		{
			internal LanguageDataMock2() : base(CultureInfo.GetCultureInfo("en")) { }

			protected override bool TryGetLocalizedTextCore(string key, [NotNullWhen(true)] out string? result, params object[] args)
			{
				result = "hello";
				return true;
			}
		}

		private sealed class LanguageDataMock3 : LanguageData
		{
			public override string ParentLanguage => "en";

			internal LanguageDataMock3() : base(CultureInfo.GetCultureInfo("zh"))
			{
				this.EnableCache = true;
			}

			protected override bool TryGetLocalizedTextCore(string key, [NotNullWhen(true)] out string? result, params object[] args)
			{
				result = "qwerty";
				return false;
			}
		}
	}
}
