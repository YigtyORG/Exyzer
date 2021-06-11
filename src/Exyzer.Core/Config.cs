/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

namespace Exyzer
{
	public static class Config
	{
		public static string Get()
		{
#if DEBUG
			return "Debug";
#elif RELEASE
			return "Release";
#elif BENCHMARK
			return "Benchmark";
#else
			return "Unknown";
#endif
		}
	}
}
