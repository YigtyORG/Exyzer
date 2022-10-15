/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Exyzer.Apps.Web.Server
{
	internal static class Program
	{
		[STAThread()]
		private static int Main(string[] args)
		{
			try {
				Host.CreateDefaultBuilder(args)
					.ConfigureWebHostDefaults(builder => builder
						.UseStartup<Startup>())
					.Build()
					.Run();
				return 0;
			} catch (Exception e) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine();
				Console.Error.WriteLine(e.ToString());
				Console.ResetColor();
				return e.HResult;
			}
		}
	}
}
