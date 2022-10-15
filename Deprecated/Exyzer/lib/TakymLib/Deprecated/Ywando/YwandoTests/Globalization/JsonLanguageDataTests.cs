/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ywando.Globalization.Tests
{
	[TestClass()]
	public class JsonLanguageDataTests
	{
		[TestMethod()]
		public void JsonLanguageDataTest()
		{
			using var ld1 = new JsonLanguageData(@"
{
	""abcd"": ""あいうえお"",
	""qwerty"": { ""text"": ""いろは"" },
	""arg"": ""{0}""
}
			", CultureInfo.GetCultureInfo("ja-JP"));

			using var ld2 = new JsonLanguageData(@"
{
	""good"": ""That's good!""
}
			", CultureInfo.GetCultureInfo("en"));

			using var ld3 = new JsonLanguageData(@"
{
	""metadata"": ""####"",
	""hello"": ""world""
}
			", CultureInfo.GetCultureInfo("zh"));

			using var ld4 = new JsonLanguageData(@"
{
	""metadata"": { ""parent"": ""fr"" }
}
			", CultureInfo.GetCultureInfo("zh"));

			Assert.AreEqual("ja",           ld1.ParentLanguage);
			Assert.AreEqual("あいうえお",   ld1.GetLocalizedText("abcd"));
			Assert.AreEqual("いろは",       ld1.GetLocalizedText("qwerty"));
			Assert.AreEqual("123455",       ld1.GetLocalizedText("arg", "123455"));
			Assert.AreEqual("ja",           ld2.ParentLanguage);
			Assert.AreEqual("That's good!", ld2.GetLocalizedText("good"));
			Assert.AreEqual("en",           ld3.ParentLanguage);
			Assert.AreEqual("####",         ld3.GetLocalizedText("metadata"));
			Assert.AreEqual("world",        ld3.GetLocalizedText("hello"));
			Assert.AreEqual("fr",           ld4.ParentLanguage);
			Assert.AreEqual("metadata",     ld4.GetLocalizedText("metadata"));

		}
	}
}
