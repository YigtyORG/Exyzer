/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using TakymLib.Aspect;
using TakymLib.Extensibility;
using TakymLib.Logging;
using TakymLib.UI.Internals;
using WinFormsApp = System.Windows.Forms.Application;
using WPFApp      = System.Windows.Application;

namespace TakymLib.UI
{
	/// <summary>
	///  アプリケーションを管理します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed partial class AppHost : WPFApp
	{
		private const    string         ENVIRONMENT_VARIABLES_PREFIX = "TAKYM_LIB_UI_APP_";
		private const    double         MIN_DELAY_FOR_SPLASH         = 1000.0D;
		private readonly IHost          _host;
		private readonly IConfiguration _config;
		private readonly ILogger        _logger;
		private readonly FormMain       _mwnd;

		/// <summary>
		///  型'<see cref="TakymLib.UI.AppHost"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="args">コマンド行引数を指定します。</param>
		/// <exception cref="ArgumentNullException"/>
		public AppHost(string[] args)
		{
			args.EnsureNotNull();

			WinFormsApp.SetHighDpiMode(HighDpiMode.SystemAware);
			WinFormsApp.EnableVisualStyles();
			WinFormsApp.SetCompatibleTextRenderingDefault(false);

			_host = Host
				.CreateDefaultBuilder(args)
				.UseContentRoot(AppContext.BaseDirectory)
				.ConfigureHostConfiguration(config => config
					.AddEnvironmentVariables(ENVIRONMENT_VARIABLES_PREFIX)
				)
				.ConfigureAppConfiguration(config => config
					.AddXmlFile(SystemSettings.Files.Generated, true, true)
					.AddIniFile(SystemSettings.Files.Custom,    true, true)
					.AddCommandLine(args) // コマンド行引数を最優先させる。
				)
				.ConfigureLogging(logging => logging
					.AddSimpleConsole(options => {
						string br = Environment.NewLine;
						options.ColorBehavior   = LoggerColorBehavior.Enabled;
						options.IncludeScopes   = false;
						options.SingleLine      = false;
						options.UseUtcTimestamp = false;
						options.TimestampFormat = br + "yyyy/MM/dd HH:mm:ss.fffffff" + br;
					})
				)
				.ConfigureServices((context, services) => services
					.AddSingleton<ModuleLoader>()
					.AddSingleton<ModuleInitializationContext, ModuleInitializationContextInternal>()
					.AddHostedService<MainWorker>()
				)
				.Build();

			_config = _host.Services.GetRequiredService<IConfiguration>();
			_logger = _host.Services.GetRequiredService<ILogger<AppHost>>();

#if DEBUG
			LoggableTask.Logger = new CallerLoggerAdapter(_host.Services.GetRequiredService<ILogger<LoggableTask>>());
#endif

			LoadComponent(this, new Uri("/TakymLib.UI;component/apphost.xaml", UriKind.Relative));
			Debug.Assert(this.ShutdownMode == ShutdownMode.OnExplicitShutdown);

			_mwnd = new FormMain(this, _host.Services);

			_logger.LogInformation("The application host is constructed.");
		}

		private AppHost() : this(Array.Empty<string>()) { }

		/// <inheritdoc/>
		protected override void OnStartup(StartupEventArgs e)
		{
			_logger.LogInformation("The application host is starting to run.");

			base.OnStartup(e);

			var task = _host.StartAsync();
			if (task.IsCompleted) {
				WinFormsApp.Run(_mwnd);
			} else if (_config.ShowSplash()) {
				var dtBegin = DateTime.Now;
				var splash  = new FormSplash();
				splash.Show();
				task.ContinueWith(async _ => {
					int delay = unchecked((int)(MIN_DELAY_FOR_SPLASH - (DateTime.Now - dtBegin).TotalMilliseconds));
					if (delay > 0) await Task.Delay(delay);
					splash.Invoke(() => {
						splash.Close();
						_mwnd.Show();
					});
				});
				WinFormsApp.Run();
			} else {
				task.ConfigureAwait(false).GetAwaiter().GetResult();
				WinFormsApp.Run(_mwnd);
			}
		}

		/// <inheritdoc/>
		protected override void OnExit(ExitEventArgs e)
		{
			_logger.LogInformation("The application host is stopping.");

			WinFormsApp.Exit();
			base.OnExit(e);

			_host.StopAsync().Wait();
			_host.Dispose();
		}

#pragma warning disable CA2254 // テンプレートは静的な式にする必要があります
		/// <summary>
		///  指定された例外を<see cref="Microsoft.Extensions.Logging.LogLevel.Critical"/>レベルでログ出力します。
		/// </summary>
		/// <param name="e">ログ出力する例外を指定します。</param>
		/// <param name="message">ログ出力するメッセージを指定します。</param>
		/// <param name="args">書式設定に使用されるオブジェクトの配列を指定します。</param>
		public void LogException(Exception? e, string? message, params object?[]? args)
		{
			_logger.LogCritical(e ?? new(), message ?? string.Empty, args ?? Array.Empty<object>());
		}
#pragma warning restore CA2254 // テンプレートは静的な式にする必要があります
	}
}
