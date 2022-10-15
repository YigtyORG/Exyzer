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
using TakymLibTests.Properties;

namespace TakymLibTests.TakymLib
{
	[TestClass()]
	public class StringExtensionsTests
	{
		[TestMethod()]
		public void ToBooleanTest()
		{
			Assert.IsTrue ("true"    .ToBoolean());
			Assert.IsTrue ("yes"     .ToBoolean());
			Assert.IsTrue ("ot"      .ToBoolean());
			Assert.IsTrue ("on"      .ToBoolean());
			Assert.IsTrue ("enable"  .ToBoolean());
			Assert.IsTrue ("enabled" .ToBoolean());
			Assert.IsTrue ("allow"   .ToBoolean());
			Assert.IsTrue ("high"    .ToBoolean());
			Assert.IsTrue ("pos"     .ToBoolean());
			Assert.IsTrue ("positive".ToBoolean());
			Assert.IsTrue ("one"     .ToBoolean());
			Assert.IsTrue ("1"       .ToBoolean());
			Assert.IsTrue ("t"       .ToBoolean());
			Assert.IsTrue ("y"       .ToBoolean());
			Assert.IsTrue ("+"       .ToBoolean());
			Assert.IsFalse("false"   .ToBoolean());
			Assert.IsFalse("no"      .ToBoolean());
			Assert.IsFalse("not"     .ToBoolean());
			Assert.IsFalse("off"     .ToBoolean());
			Assert.IsFalse("disable" .ToBoolean());
			Assert.IsFalse("disabled".ToBoolean());
			Assert.IsFalse("deny"    .ToBoolean());
			Assert.IsFalse("low"     .ToBoolean());
			Assert.IsFalse("neg"     .ToBoolean());
			Assert.IsFalse("negative".ToBoolean());
			Assert.IsFalse("zero"    .ToBoolean());
			Assert.IsFalse("0"       .ToBoolean());
			Assert.IsFalse("f"       .ToBoolean());
			Assert.IsFalse("n"       .ToBoolean());
			Assert.IsFalse("-"       .ToBoolean());
			Assert.ThrowsException<FormatException>(() => "abc".ToBoolean());
		}

		[TestMethod()]
		public void AbridgeTest()
		{
			Assert.AreEqual("abcd",     "abcd"   .Abridge(4));
			Assert.AreEqual("abcd    ", "abcd"   .Abridge(8));
			Assert.AreEqual("xyz ",     "xyz"    .Abridge(4));
			Assert.AreEqual("xyz     ", "xyz"    .Abridge(8));
			Assert.AreEqual("123...",   "1234567".Abridge(6));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => string.Empty.Abridge(0));
		}

		[Obsolete()]
		[TestMethod()]
		public void FitToLineTest()
		{
			Assert.AreEqual("abcxyz123",        "abcxyz123".FitToLine());
			Assert.AreEqual("[CR][LF][TB][SP]", "\r\n\t　" .FitToLine());
		}

		[TestMethod()]
		public void RemoveControlCharsTests()
		{
			char[] chars = new char[0xA0];
			for (int i = 0; i < chars.Length; ++i) {
				chars[i] = ((char)(i));
			}

			Assert.AreEqual(
				Resources.RemoveControlCharsTests_0,
				new(chars.RemoveControlChars(ControlCharsReplaceMode.RemoveAll))
			);

			Assert.AreEqual(
				Resources.RemoveControlCharsTests_1,
				new(chars.RemoveControlChars(ControlCharsReplaceMode.ConvertToText))
			);

			Assert.AreEqual(
				Resources.RemoveControlCharsTests_2,
				new(chars.RemoveControlChars(ControlCharsReplaceMode.ConvertToIcon))
			);

			Assert.AreEqual(
				Resources.RemoveControlCharsTests_3,
				new(chars.RemoveControlChars(ControlCharsReplaceMode.ConvertToText, true, true, true))
			);

			Assert.AreEqual(
				Resources.RemoveControlCharsTests_4,
				new(chars.RemoveControlChars(ControlCharsReplaceMode.ConvertToIcon, true, true, true))
			);
		}
	}
}
