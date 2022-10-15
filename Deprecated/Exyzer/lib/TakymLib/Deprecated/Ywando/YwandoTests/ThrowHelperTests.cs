/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ywando.Tests
{
	[TestClass()]
	public class ThrowHelperTests
	{
		[TestMethod()]
		public void EnsureNotNullTest()
		{
			const string argName = "hello";

			var argNullEx = Assert.ThrowsException<ArgumentNullException>(
				() => ThrowHelper.EnsureNotNull<object?>(null, argName)
			);

			Assert.AreEqual(argName, argNullEx.ParamName);
		}
	}
}
