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

#if RELEASE
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
	}
}
