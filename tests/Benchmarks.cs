/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BENCHMARK
using BenchmarkDotNet.Running;
#endif

namespace Exyzer.Tests
{
	[TestClass()]
	[MemoryDiagnoser()]
	[DisassemblyDiagnoser()]
	public class Benchmarks
	{
		private static readonly IConfig _config = DefaultConfig.Instance.AddJob(new Job()
			.Freeze()
			.WithCustomBuildConfiguration("Benchmark")
		);

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
		public void SetValue_Array()
		{
			byte[] values = CreateArray();
			for (int i = 0; i < values.Length; ++i) {
				values[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_Span()
		{
			var values = CreateArray().AsSpan();
			for (int i = 0; i < values.Length; ++i) {
				values[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_MemoryDevice_Data()
		{
			var values = new MemoryDeviceMock(true, true, CreateArray());
			for (int i = 0; i < values.Data.Length; ++i) {
				values.Data[i] = ((byte)(i & 0xFF));
			}
		}

		[Benchmark()]
		public void SetValue_MemoryDevice_Output()
		{
			var values = new MemoryDeviceMock(true, true, CreateArray());
			for (int i = 0; i < values.Data.Length; ++i) {
				values.Output(i, ((byte)(i & 0xFF)));
			}
		}

		private static byte[] CreateArray()
		{
			return new byte[0x10000];
		}
	}
}
