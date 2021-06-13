/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Exyzer.Apps.Web.Server
{
	internal sealed class Startup : StartupBase
	{
		private readonly IWebHostEnvironment _env;

		public Startup(IWebHostEnvironment env)
		{
			_env = env ?? throw new ArgumentNullException(nameof(env));
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);
			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		public override void Configure(IApplicationBuilder app)
		{
			if (_env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			} else {
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseEndpoints(endpoints => {
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
