/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exrecodel.Tests
{
	[TestClass()]
	public class XrcdlDocumentTests
	{
		[TestMethod()]
		public void CreateTest()
		{
			var obj = XrcdlDocument.Create();
			Assert.IsNotNull(obj);
		}
	}
}
