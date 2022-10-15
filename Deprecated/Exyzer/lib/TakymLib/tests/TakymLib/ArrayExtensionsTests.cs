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
	public class ArrayExtensionsTests
	{
		[TestMethod()]
		public void JoinObjectArraysTest()
		{
			object[]  baseArray = new object[] { "abc", 123 };
			object?[] newArray  = baseArray.Combine(
				new object[] { 456, 789 },
				new object[] { true, false },
				new object[] { "qwerty", 123.456M },
				Array.Empty<object>(),
				new object?[] { null }
			);
			object?[] sampleArray = new object?[] {
				"abc", 123, 456, 789, true, false, "qwerty", 123.456M, null
			};
			Assert.AreEqual(sampleArray.Length, newArray.Length);
			for (int i = 0; i < newArray.Length; ++i) {
				Assert.AreEqual(sampleArray[i]?.GetType(), newArray[i]?.GetType());
				Assert.AreEqual(sampleArray[i],            newArray[i]);
			}
		}

		[TestMethod()]
		public void JoinGenericArraysTest()
		{
			string[]  baseArray = new[] { "this", "is", "an", "array" };
			string?[] newArray  = baseArray.Combine(
				new [] { ".", "あああ" },
				new [] { "いいい" },
				new [] { "１２３４５６", string.Empty, "@@@" },
				new string[0], new string[1]
			);
			string?[] sampleArray = new[] {
				"this", "is", "an", "array", ".", "あああ", "いいい", "１２３４５６", string.Empty, "@@@", null
			};
			Assert.AreEqual(sampleArray.Length, newArray.Length);
			for (int i = 0; i < newArray.Length; ++i) {
				Assert.AreEqual(sampleArray[i], newArray[i]);
			}
		}
	}
}
