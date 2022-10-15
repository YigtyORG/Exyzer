/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TakymLib.Extensibility;
using TakymLib.Threading.Tasks;
using TakymLib.UI.Models;
using MICI      = TakymLib.UI.Internals.ModuleInitializationContextInternal;
using MIContext = TakymLib.Extensibility.ModuleInitializationContext;

namespace TakymLib.UI.Internals
{
	internal sealed class MainWorker : IHostedService
	{
		private readonly ILogger        _logger;
		private readonly ModuleLoader   _loader;
		private readonly MIContext      _context;
		private readonly IConfiguration _config;

		public MainWorker(
			ILogger<MainWorker>      logger,
			ModuleLoader             loader,
			MIContext                context,
			IConfiguration           config,
			IHostApplicationLifetime lifetime)
		{
			logger .EnsureNotNull();
			loader .EnsureNotNull();
			context.EnsureNotNull();
			config .EnsureNotNull();

			_logger  = logger;
			_loader  = loader;
			_context = context;
			_config  = config;

			if (logger.IsEnabled(LogLevel.Trace)) {
				lifetime.EnsureNotNull();
				lifetime.ApplicationStarted .Register(this.ApplicationStarted);
				lifetime.ApplicationStopping.Register(this.ApplicationStopping);
				lifetime.ApplicationStopped .Register(this.ApplicationStopped);
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(StartAsync)}");

			bool disallowExtensions = _config.DisallowExtensions();
			if (_logger.IsEnabled(LogLevel.Debug)) {
				bool showSplash = _config.ShowSplash();
				_logger.LogDebug("{Key} = {Value}", SystemSettings.Keys.DisallowExtensions, disallowExtensions);
				_logger.LogDebug("{Key} = {Value}", SystemSettings.Keys.ShowSplash,         showSplash);
			}

			return disallowExtensions
				? Task.CompletedTask
				: WaitForLoadModules(_logger, _loader, _context, cancellationToken);

			static async Task WaitForLoadModules(ILogger logger, ModuleLoader loader, MIContext context, CancellationToken ct)
			{
				await Task.Yield();
				if (context is MICI mici) {
					var modules = await LoadModules   (loader, context, ct).ConfigureAwait(false);
					var plugins = await MakePluginTree(modules,         ct).ConfigureAwait(false);
					mici.SetModulesAndPlugins(modules.AsReadOnly(), plugins);
				} else {
					logger.LogWarning("The specified module initialization context does not support loading modules.");
				}
			}

			static async ValueTask<List<FeatureModule>> LoadModules(ModuleLoader loader, MIContext context, CancellationToken ct)
			{
				var result  = new List<FeatureModule>();
				var modules = loader
					.LoadFromDirectoryAsync(context, AppContext.BaseDirectory, cancellationToken: ct)
					.OrderBy(m => m.DisplayName);

				await foreach (var module in modules.ConfigureAwait(false).WithCancellation(ct)) {
					if (!result.Contains(module)) {
						result.Add(module);
					}
					ct.ThrowIfCancellationRequested();
					await TaskUtility.Yield();
				}

				return result;
			}

			static async ValueTask<IReadOnlyList<PluginTreeNode>> MakePluginTree(List<FeatureModule> modules, CancellationToken ct)
			{
				int count = modules.Count;
				var nodes = new PluginTreeNode[count];
				for (int i = 0; i < count; ++i) {
					nodes[i] = await MakePluginTreeCore(modules[i], ct).ConfigureAwait(false);
					await TaskUtility.Yield(i);
				}
				return Array.AsReadOnly(nodes);

				static async ValueTask<PluginTreeNode> MakePluginTreeCore(IPlugin plugin, CancellationToken ct)
				{
					var result  = new List<PluginTreeNode>();
					var plugins = plugin
						.EnumerateChildrenAsync()
						.OrderBy(p => p.DisplayName);

					await foreach (var item in plugins.ConfigureAwait(false).WithCancellation(ct)) {
						result.Add(await MakePluginTreeCore(item, ct).ConfigureAwait(false));
						ct.ThrowIfCancellationRequested();
						await TaskUtility.Yield();
					}

					return new(plugin, result.AsReadOnly());
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(StopAsync)}");
			return Task.CompletedTask;
		}

		private void ApplicationStarted()
		{
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(ApplicationStarted)}");
		}

		private void ApplicationStopping()
		{
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(ApplicationStopping)}");
		}

		private void ApplicationStopped()
		{
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(ApplicationStopped)}");
		}
	}
}
