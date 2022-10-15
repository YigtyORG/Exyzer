/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Exyzer.Tests.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BENCHMARK
using BenchmarkDotNet.Running;
#endif

namespace Exyzer.Tests
{
	[TestClass()]
	[MemoryDiagnoser()]
	[DisassemblyDiagnoser(5, true, true, true, true, true, true)]
	public class Benchmarks
	{
		[TestMethod()]
		public void Run()
		{
#if BENCHMARK
			BenchmarkSwitcher.FromTypes(new[] { this.GetType() }).RunAllJoined();
#else
			Console.WriteLine(nameof(Benchmarks) + " are only supported in the benchmark build.");
#endif
		}

		[Benchmark()]
		public byte CreateArrayAndGet()
		{
			var array = new byte[0x100];
			return array[0];
		}

		[Benchmark()]
		public byte CreateArrayAsSpanAndGet()
		{
			var span = new byte[0x100].AsSpan();
			return span[0];
		}

		[Benchmark()]
		public byte CreateSpanAndGet()
		{
			Span<byte> span = stackalloc byte[0x100];
			return span[0];
		}

		[Benchmark()]
		public byte CreateMemoryDeviceAndGet()
		{
			var memory = new MemoryDeviceMock(true, true, new byte[0x100]);
			return memory.Data[0];
		}

		[Benchmark()]
		public void SetValue_Array()
		{
			byte[] values = CreateArray();
			for (int i = 0; i < values.Length; ++i) {
				values[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_Span1()
		{
			var values = CreateArray().AsSpan();
			for (int i = 0; i < values.Length; ++i) {
				values[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_Span2()
		{
			var values = CreateArray().AsSpan();
			int i = 0;
			while (values.Length > 0) {
				values[0] = ((byte)(i & 0xFF));
				values = values[1..];
				++i;
			}
		}

		[Benchmark()]
		public void SetValue_MemoryDevice_Data1()
		{
			var values = new MemoryDeviceMock(true, true, CreateArray());
			for (int i = 0; i < values.Data.Length; ++i) {
				values.Data[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_MemoryDevice_Data2()
		{
			var values = new MemoryDeviceMock(true, true, CreateArray());
			var span   = values.Data;
			for (int i = 0; i < span.Length; ++i) {
				span[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_MemoryDevice_Output()
		{
			var values = new MemoryDeviceMock(true, true, CreateArray());
			for (int i = 0; i < values.Data.Length; ++i) {
				values.Output(i, i & 0xFF);
			}
		}

		[Benchmark()]
		public void SetValue_MemoryDevice_Output1()
		{
			var values = new MemoryDeviceMock(true, true, CreateArray());
			for (int i = 0; i < values.Data.Length; ++i) {
				int address = i;
				int data    = i & 0xFF;
				if (values.CanWrite) {
					if (address < 0) {
						_ = IOResult.OutOfRange;
					}
					var span = values.Data;
					if (address >= span.Length - 4) {
						_ = IOResult.OutOfRange;
					}
					if (BitConverter.TryWriteBytes(span[address..], data)) {
						_ = IOResult.Success;
					} else {
						_ = IOResult.Failed;
					}
				} else {
					_ = IOResult.AccessDenied;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte[] CreateArray()
		{
			return new byte[0x0100];
		}
	}
}
