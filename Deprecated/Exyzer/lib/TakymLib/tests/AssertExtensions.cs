/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TakymLibTests
{
	public static class AssertExtensions
	{
		public static void Contains(this Assert? _, string actual, string expectedToContain)
		{
			Assert.IsTrue(actual.Contains(expectedToContain), $"Expected to contain: {expectedToContain}, Actual: {actual}");
		}
	}
}
