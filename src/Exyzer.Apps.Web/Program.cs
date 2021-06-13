/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Exyzer.Apps.Web
{
	internal static class Program
	{
		[STAThread()]
		private static async Task<int> Main(string[] args)
		{
			try {
				var builder = WebAssemblyHostBuilder.CreateDefault(args);
				builder.RootComponents.Add<App>("#app");
				builder.Services.AddScoped(_ => new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
				await builder.Build().RunAsync();
				return 0;
			} catch (Exception e) {
				Console.Error.WriteLine(e.ToString());
				return e.HResult;
			}
		}
	}
}
