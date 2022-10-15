/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exyzer.Tests
{
	[TestClass()]
	public class ValueFormatterTests
	{
		[TestMethod()]
		public void TryToStringTests()
		{
			// ValueFormatter は自動生成されているので byte, sbyte のみ検証している。
			// ただし、本来は ushort, short, uint, int, ulong, long も省略せず検証すべきである。

			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0x00)), ValueFormat.Binary, out string? result));
				Assert.AreEqual("00000000", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0x00)), ValueFormat.Octal, out string? result));
				Assert.AreEqual("000", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0x00)), ValueFormat.Decimal, out string? result));
				Assert.AreEqual("0", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0x00)), ValueFormat.Hexadecimal, out string? result));
				Assert.AreEqual("00", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0xFF)), ValueFormat.Binary, out string? result));
				Assert.AreEqual("11111111", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0xFF)), ValueFormat.Octal, out string? result));
				Assert.AreEqual("377", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0xFF)), ValueFormat.Decimal, out string? result));
				Assert.AreEqual("255", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0xFF)), ValueFormat.Hexadecimal, out string? result));
				Assert.AreEqual("FF", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0x30)), ValueFormat.Utf8Chars, out string? result));
				Assert.AreEqual("0", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((byte)(0x40)), ValueFormat.Utf8Chars, out string? result));
				Assert.AreEqual("@", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((sbyte)(0x00)), ValueFormat.Binary, out string? result));
				Assert.AreEqual("00000000", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((sbyte)(0x00)), ValueFormat.Octal, out string? result));
				Assert.AreEqual("000", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((sbyte)(0x00)), ValueFormat.Decimal, out string? result));
				Assert.AreEqual("0", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((sbyte)(0x00)), ValueFormat.Hexadecimal, out string? result));
				Assert.AreEqual("00", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(unchecked((sbyte)(0xFF)), ValueFormat.Binary, out string? result));
				Assert.AreEqual("11111111", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(unchecked((sbyte)(0xFF)), ValueFormat.Octal, out string? result));
				Assert.AreEqual("377", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(unchecked((sbyte)(0xFF)), ValueFormat.Decimal, out string? result));
				Assert.AreEqual("-1", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(unchecked((sbyte)(0xFF)), ValueFormat.Hexadecimal, out string? result));
				Assert.AreEqual("FF", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((sbyte)(0x30)), ValueFormat.Utf8Chars, out string? result));
				Assert.AreEqual("0", result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToString(((sbyte)(0x40)), ValueFormat.Utf8Chars, out string? result));
				Assert.AreEqual("@", result);
			}
		}

		[TestMethod()]
		public void TryToIntNTests()
		{
			// こちらは byte, sbyte ではなく uint, int で検証を行っている。

			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("0", ValueFormat.Binary, out uint result));
				Assert.AreEqual<uint>(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("11111111", ValueFormat.Binary, out uint result));
				Assert.AreEqual<uint>(255, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("0", ValueFormat.Octal, out uint result));
				Assert.AreEqual<uint>(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("777", ValueFormat.Octal, out uint result));
				Assert.AreEqual<uint>(511, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("0", ValueFormat.Decimal, out uint result));
				Assert.AreEqual<uint>(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("123", ValueFormat.Decimal, out uint result));
				Assert.AreEqual<uint>(123, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("0", ValueFormat.Hexadecimal, out uint result));
				Assert.AreEqual<uint>(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("AB", ValueFormat.Hexadecimal, out uint result));
				Assert.AreEqual<uint>(171, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("0000", ValueFormat.Utf8Chars, out uint result));
				Assert.AreEqual<uint>(0x30303030, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToUInt32("abcd", ValueFormat.Utf8Chars, out uint result));
				Assert.AreEqual<uint>(0x64636261, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("0", ValueFormat.Binary, out int result));
				Assert.AreEqual(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("11111111", ValueFormat.Binary, out int result));
				Assert.AreEqual(255, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("0", ValueFormat.Octal, out int result));
				Assert.AreEqual(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("777", ValueFormat.Octal, out int result));
				Assert.AreEqual(511, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("0", ValueFormat.Decimal, out int result));
				Assert.AreEqual(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("123", ValueFormat.Decimal, out int result));
				Assert.AreEqual(123, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("-123", ValueFormat.Decimal, out int result));
				Assert.AreEqual(-123, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("0", ValueFormat.Hexadecimal, out int result));
				Assert.AreEqual(0, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("AB", ValueFormat.Hexadecimal, out int result));
				Assert.AreEqual(171, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("0000", ValueFormat.Utf8Chars, out int result));
				Assert.AreEqual(0x30303030, result);
			}
			{
				Assert.IsTrue(ValueFormatter.TryToSInt32("abcd", ValueFormat.Utf8Chars, out int result));
				Assert.AreEqual(0x64636261, result);
			}
		}
	}
}
