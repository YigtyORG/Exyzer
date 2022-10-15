/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

namespace TakymLibTests.TakymLib
{
	[TestClass()]
	public class ArgumentHelperTests
	{
		[TestMethod()]
		public void EnsureNotNullTest()
		{
			object? notnullobj = new();
			notnullobj.EnsureNotNull();

			object? nullobj = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNull());

			string? notnullstr = string.Empty;
			notnullstr.EnsureNotNull();

			string? nullstr = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullstr.EnsureNotNull());
		}

		[TestMethod()]
		public void EnsureNotNullTest2()
		{
			var e = Assert.ThrowsException<ArgumentNullException>(() => Function());
			Assert.AreEqual("nullObj", e.ParamName);

			static void Function(object? nullObj = null)
			{
				nullObj.EnsureNotNull();
			}
		}

		[TestMethod()]
		public void EnsureWithinClosedRangeTest()
		{
			IComparable? nullobj = null;
			nullobj.EnsureWithinClosedRange(0, 1);
			nullobj.EnsureWithinClosedRange(1, 0);

			int? nullval = default;
			nullval.EnsureWithinClosedRange(0, 1);
			nullval.EnsureWithinClosedRange(1, 0);

			int a = 123;
			a.EnsureWithinClosedRange( 50, 150);
			a.EnsureWithinClosedRange(  0, 200);
			a.EnsureWithinClosedRange(  0, 123);
			a.EnsureWithinClosedRange(123, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinClosedRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinClosedRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinClosedRange(200,   0));

			IComparable a2 = a;
			a2.EnsureWithinClosedRange( 50, 150);
			a2.EnsureWithinClosedRange(  0, 200);
			a2.EnsureWithinClosedRange(  0, 123);
			a2.EnsureWithinClosedRange(123, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinClosedRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinClosedRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinClosedRange(200,   0));
		}

		[TestMethod()]
		public void EnsureWithinOpenRangeTest()
		{
			IComparable? nullobj = null;
			nullobj.EnsureWithinOpenRange(0, 1);
			nullobj.EnsureWithinOpenRange(1, 0);

			int? nullval = default;
			nullval.EnsureWithinOpenRange(0, 1);
			nullval.EnsureWithinOpenRange(1, 0);

			int a = 123;
			a.EnsureWithinOpenRange(50, 150);
			a.EnsureWithinOpenRange( 0, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(  0, 123));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(123, 200));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(200,   0));

			IComparable a2 = a;
			a2.EnsureWithinOpenRange(50, 150);
			a2.EnsureWithinOpenRange( 0, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(  0, 123));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(123, 200));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(200,   0));
		}

		[TestMethod()]
		public void EnsureNotNullWithinClosedRangeTest()
		{
			IComparable? nullobj = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinClosedRange(0, 1));
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinClosedRange(1, 0));

			int? nullval = default;
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinClosedRange(0, 1));
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinClosedRange(1, 0));

			int a = 123;
			a.EnsureNotNullWithinClosedRange( 50, 150);
			a.EnsureNotNullWithinClosedRange(  0, 200);
			a.EnsureNotNullWithinClosedRange(  0, 123);
			a.EnsureNotNullWithinClosedRange(123, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinClosedRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinClosedRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinClosedRange(200,   0));

			IComparable a2 = a;
			a2.EnsureNotNullWithinClosedRange( 50, 150);
			a2.EnsureNotNullWithinClosedRange(  0, 200);
			a2.EnsureNotNullWithinClosedRange(  0, 123);
			a2.EnsureNotNullWithinClosedRange(123, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinClosedRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinClosedRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinClosedRange(200,   0));
		}

		[TestMethod()]
		public void EnsureNotNullWithinOpenRangeTest()
		{
			IComparable? nullobj = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinOpenRange(0, 1));
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinOpenRange(1, 0));

			int? nullval = default;
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinOpenRange(0, 1));
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinOpenRange(1, 0));

			int a = 123;
			a.EnsureNotNullWithinOpenRange(50, 150);
			a.EnsureNotNullWithinOpenRange( 0, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(  0, 123));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(123, 200));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(200,   0));

			IComparable a2 = a;
			a2.EnsureNotNullWithinOpenRange(50, 150);
			a2.EnsureNotNullWithinOpenRange( 0, 200);
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(  0, 123));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(123, 200));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(  0,  50));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(200, 300));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(200,   0));
		}
	}
}
