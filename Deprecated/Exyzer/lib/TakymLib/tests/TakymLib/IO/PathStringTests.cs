/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.IO;
using TakymLibTests.Properties;

namespace TakymLibTests.TakymLib.IO
{
	[TestClass()]
	public class PathStringTests
	{
		internal const string Path = "C:\\aaa\\bbb\\ccc\\123.xyz";

		[TestMethod()]
		public void ToStringTest()
		{
			DependsOn.Windows(() => {
				string formatted1 = PathStringPool.Get(Path).ToString(PathStringFormatterTests.Format);
				Assert.AreEqual(Resources.PathStringToStringResult, formatted1);
				string formatted2 = PathStringPool.Get(Path).ToString("");
				Assert.AreEqual(Path, formatted2);
			});
		}
	}
}
