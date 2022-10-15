/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

namespace TakymLibTests.TakymLib
{
	[TestClass()]
	public class SecureStringExtensionsTests
	{
		private SecureString? sstr1, sstr2, sstr3;

		[TestInitialize()]
		public void Initialize()
		{
			sstr1 = new SecureString();
			sstr2 = new SecureString();
			sstr3 = new SecureString();

			sstr1.AppendChar('a');
			sstr1.AppendChar('b');
			sstr1.AppendChar('c');
			sstr2.AppendChar('a');
			sstr2.AppendChar('b');
			sstr2.AppendChar('c');
			sstr3.AppendChar('x');
			sstr3.AppendChar('y');
			sstr3.AppendChar('z');
		}

		[TestMethod()]
		public void IsEqualWithTests()
		{
			if (sstr1 is null || sstr2 is null || sstr3 is null) {
				Assert.Fail("Does not initialized.");
				return;
			}

			Assert.IsTrue (sstr1.IsEqualWith(sstr2));
			Assert.IsTrue (sstr2.IsEqualWith(sstr1));
			Assert.IsFalse(sstr1.IsEqualWith(sstr3));
			Assert.IsFalse(sstr3.IsEqualWith(sstr1));
		}

		[TestCleanup()]
		public void Cleanup()
		{
			sstr1?.Dispose();
			sstr2?.Dispose();
			sstr3?.Dispose();
			sstr1 = sstr2 = sstr3 = null;
		}
	}
}
