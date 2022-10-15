/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Logging;

namespace TakymLibTests.TakymLib.Logging
{
	[TestClass()]
	public class LogFileNameTests
	{
		[TestMethod()]
		public void CreateTest()
		{
			Assert.AreEqual(
				"000101010000000000000.[0].log",
				LogFileName.Create(new DateTime(0), null, null, false)
			);
			Assert.AreEqual(
				"0001-01-01_00-00-00+0000000.[0].log",
				LogFileName.Create(new DateTime(0), null, null, true)
			);
		}
	}
}
