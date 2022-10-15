/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Linq;
using Exyzer.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exyzer.Tests.Devices
{
	[TestClass()]
	public class MemoryDeviceTests
	{
		[TestMethod()]
		public void ConstructorTest()
		{
			ConstructorTestCore(true,  true);
			ConstructorTestCore(true,  false);
			ConstructorTestCore(false, true);
			ConstructorTestCore(false, false);
		}

		private static void ConstructorTestCore(bool read, bool write)
		{
			var md = new MemoryDeviceMock(read, write);
			Assert.AreEqual(nameof(MemoryDevice), md.Name);
			Assert.AreEqual(Guid.Empty,           md.Guid);
			Assert.AreEqual(0,                    md.Data.Length);
			Assert.AreEqual(read,                 md.CanRead);
			Assert.AreEqual(write,                md.CanWrite);
		}

		[TestMethod()]
		public void InputTest()
		{
			InputTestCore(true,  true,  InputTest_32bits);
			InputTestCore(true,  false, InputTest_32bits);
			InputTestCore(false, true,  InputTest_32bits);
			InputTestCore(false, false, InputTest_32bits);
			InputTestCore(true,  true,  InputTest_64bits);
			InputTestCore(true,  false, InputTest_64bits);
			InputTestCore(false, true,  InputTest_64bits);
			InputTestCore(false, false, InputTest_64bits);
		}

		private static void InputTestCore(bool read, bool write, Action<bool, bool, byte[]> test_action)
		{
			test_action(read, write, Array.Empty<byte>());
			test_action(read, write, new byte[] { 0 });
			test_action(read, write, new byte[] { 0, 1 });
			test_action(read, write, new byte[] { 0, 1, 2 });
			test_action(read, write, new byte[] { 0, 1, 2, 3 });
			test_action(read, write, new byte[] { 0, 1, 2, 3, 4 });
			test_action(read, write, new byte[] { 0, 1, 2, 3, 4, 5 });
			test_action(read, write, new byte[] { 0, 1, 2, 3, 4, 5, 6 });
			test_action(read, write, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
			test_action(read, write, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
			test_action(read, write, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
			test_action(read, write, new byte[] { 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF });
		}

		private static void InputTest_32bits(bool read, bool write, byte[] data)
		{
			var md = new MemoryDeviceMock(read, write, data);

			var result = md.Input(-1, out int _);
			if (read) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			result = md.Input(data.Length, out int _);
			if (read) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			for (int i = 0; i < data.Length; ++i) {
				result = md.Input(i, out int x);
				if (read) {
					if (i < data.Length - 4) {
						Assert.AreEqual(IOResult.Success, result);
						Assert.AreEqual(BitConverter.ToInt32(data, i), x);
					} else {
						Assert.AreEqual(IOResult.OutOfRange, result);
					}
				} else {
					Assert.AreEqual(IOResult.AccessDenied, result);
				}
			}
		}

		private static void InputTest_64bits(bool read, bool write, byte[] data)
		{
			var md = new MemoryDeviceMock(read, write, data);

			var result = md.Input(-1, out long _);
			if (read) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			result = md.Input(data.Length, out int _);
			if (read) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			for (int i = 0; i < data.Length; ++i) {
				result = md.Input(i, out long x);
				if (read) {
					if (i < data.Length - 8) {
						Assert.AreEqual(IOResult.Success, result);
						Assert.AreEqual(BitConverter.ToInt64(data, i), x);
					} else {
						Assert.AreEqual(IOResult.OutOfRange, result);
					}
				} else {
					Assert.AreEqual(IOResult.AccessDenied, result);
				}
			}
		}

		[TestMethod()]
		public void OutputTest()
		{
			OutputTestCore(true,  true,  OutputTest_32bits);
			OutputTestCore(true,  false, OutputTest_32bits);
			OutputTestCore(false, true,  OutputTest_32bits);
			OutputTestCore(false, false, OutputTest_32bits);
			OutputTestCore(true,  true,  OutputTest_64bits);
			OutputTestCore(true,  false, OutputTest_64bits);
			OutputTestCore(false, true,  OutputTest_64bits);
			OutputTestCore(false, false, OutputTest_64bits);
		}

		private static void OutputTestCore(bool read, bool write, Action<bool, bool, int, byte> test_action)
		{
			for (int i = 0; i <= 16; ++i) {
				test_action(read, write, i, ((byte)(i)));
			}
		}

		private static void OutputTest_32bits(bool read, bool write, int size, byte value)
		{
			int value32 = value;
			var md      = new MemoryDeviceMock(read, write, new byte[size]);

			var result = md.Output(-1, value32);
			if (write) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			result = md.Output(size, value32);
			if (write) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			for (int i = 0; i < size; ++i) {
				result = md.Output(i, value32);
				if (write) {
					if (i < size - 4) {
						Assert.AreEqual(IOResult.Success, result);
						Assert.AreEqual(md.Data[i], value32);
					} else {
						Assert.AreEqual(IOResult.OutOfRange, result);
					}
				} else {
					Assert.AreEqual(IOResult.AccessDenied, result);
				}
			}
		}

		private static void OutputTest_64bits(bool read, bool write, int size, byte value)
		{
			long value64 = value;
			var  md      = new MemoryDeviceMock(read, write, new byte[size]);

			var result = md.Output(-1, value64);
			if (write) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			result = md.Output(size, value64);
			if (write) {
				Assert.AreEqual(IOResult.OutOfRange, result);
			} else {
				Assert.AreEqual(IOResult.AccessDenied, result);
			}
			for (int i = 0; i < size; ++i) {
				result = md.Output(i, value64);
				if (write) {
					if (i < size - 8) {
						Assert.AreEqual(IOResult.Success, result);
						Assert.AreEqual(md.Data[i], value64);
					} else {
						Assert.AreEqual(IOResult.OutOfRange, result);
					}
				} else {
					Assert.AreEqual(IOResult.AccessDenied, result);
				}
			}
		}

		[TestMethod()]
		public void GetEnumeratorTest()
		{
			GetEnumeratorTestCore(true,  true);
			GetEnumeratorTestCore(true,  false);
			GetEnumeratorTestCore(false, true);
			GetEnumeratorTestCore(false, false);
		}

		private static void GetEnumeratorTestCore(bool read, bool write, int size = 100)
		{
			var    md    = new MemoryDeviceMock(read, write, new byte[size]);
			byte[] array = md.ToArray();

			if (read) {
				Assert.AreEqual(array.Length, size);
			} else {
				Assert.AreEqual(array.Length, 0);
			}

			foreach (byte b in md) {
				Assert.AreEqual(0, b);
			}
		}
	}
}
