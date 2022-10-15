/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TakymLib.Aspect;

namespace TakymLib.Extensibility
{
	/// <summary>
	///  <see cref="TakymLib.Extensibility.FeatureModule"/>を読み込みます。
	/// </summary>
	public class ModuleLoader
	{
		private static readonly ConcurrentDictionary<Assembly, FeatureModule?> _cache = new();

		private static readonly EnumerationOptions _eo = new() {
			AttributesToSkip         = FileAttributes.Hidden | FileAttributes.System,
			BufferSize               = 0,
			IgnoreInaccessible       = true,
			MatchCasing              = MatchCasing.PlatformDefault,
			MatchType                = MatchType.Win32,
			RecurseSubdirectories    = true,
			ReturnSpecialDirectories = false
		};

		/// <summary>
		///  ログの出力先を取得します。
		/// </summary>
		protected ILogger Logger { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Extensibility.ModuleLoader"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="logger">ログの出力先を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public ModuleLoader(ILogger<ModuleLoader> logger)
		{
			logger.EnsureNotNull();
			this.Logger = logger;
		}

		/// <summary>
		///  指定されたディレクトリから<see cref="TakymLib.Extensibility.FeatureModule"/>を非同期的に読み込みます。
		/// </summary>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="path">読み込み元のディレクトリへのパスを指定します。</param>
		/// <param name="pattern">検索文字列を指定します。既定値は「<c>*.dll</c>」です。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。この引数は省略可能です。</param>
		/// <returns>
		///  読み込んだ<see cref="TakymLib.Extensibility.FeatureModule"/>を非同期的に列挙する<see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.OperationCanceledException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public IAsyncEnumerable<FeatureModule> LoadFromDirectoryAsync(
			ModuleInitializationContext context,
			string                      path,
			string?                     pattern           = "*.dll",
			CancellationToken           cancellationToken = default)
		{
			if (context is null) {
				throw new ArgumentNullException(nameof(context));
			}
			if (string.IsNullOrEmpty(path)) {
				throw new ArgumentNullException(nameof(path));
			}
			if (string.IsNullOrEmpty(pattern)) {
				pattern = "*.dll";
			}

			return LoadFromDirectoryAsyncCore(this, context, path, pattern, cancellationToken);

			static async IAsyncEnumerable<FeatureModule> LoadFromDirectoryAsyncCore(
				                           ModuleLoader                self,
				                           ModuleInitializationContext context,
				                           string                      path,
				                           string                      pattern,
				[EnumeratorCancellation()] CancellationToken           cancellationToken)
			{
				self.Logger.LogInformation("Loading from the directory \"{path}\", \"{pattern}\"", path, pattern);

				IEnumerable<string> files;
				try {
					files = Directory.EnumerateFiles(path, pattern, _eo);
				} catch (Exception e) {
					context.AddErrorOnLoad(e);
					self.Logger.LogError(e, "Failed to load from the directory \"{path}\", \"{pattern}\"", path, pattern);
					yield break;
				}

				await Task.Yield();

				foreach (string fname in files) {
					cancellationToken.ThrowIfCancellationRequested();
					self.Logger.LogDebug("Path = {path}", fname);

					FeatureModule? module = null;
					try {
						module = await self.LoadAsync(fname, context, cancellationToken).ConfigureAwait(false);
					} catch (OperationCanceledException oce) when (oce.CancellationToken == cancellationToken) {
						self.Logger.LogWarning(oce, "Canceled to load from the directory \"{path}\", \"{pattern}\"", path, pattern);
						throw;
					} catch (Exception e) {
						context.AddErrorOnLoad(e);
						self.Logger.LogError(e, "Failed to load from the file \"{path}\"", fname);
						continue;
					}

					if (module is not null) {
						yield return module;
					}
				}

				self.Logger.LogInformation("Loaded from the directory \"{path}\", \"{pattern}\"", path, pattern);
			}
		}

		/// <summary>
		///  ファイルから<see cref="TakymLib.Extensibility.FeatureModule"/>を非同期的に読み込みます。
		/// </summary>
		/// <param name="fileName">読み込み元のアセンブリのファイル名を指定します。</param>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。この引数は省略可能です。</param>
		/// <returns>
		///  読み込んだ<see cref="TakymLib.Extensibility.FeatureModule"/>を含むこの処理の非同期操作を返します。
		///  アセンブリに<see cref="TakymLib.Extensibility.FeatureModule"/>が含まれない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.OperationCanceledException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.IO.FileNotFoundException"/>
		/// <exception cref="System.IO.FileLoadException"/>
		/// <exception cref="System.BadImageFormatException"/>
		/// <exception cref="System.Security.SecurityException"/>
		/// <exception cref="System.IO.PathTooLongException"/>
		public LoggableTask<FeatureModule?> LoadAsync(
			string                      fileName,
			ModuleInitializationContext context,
			CancellationToken           cancellationToken = default)
		{
			if (string.IsNullOrEmpty(fileName)) {
				throw new ArgumentNullException(nameof(fileName));
			}
			Assembly asm;
			try {
				asm = Assembly.LoadFrom(fileName);
			} catch (ArgumentException ae) {
				throw new ArgumentException(ae.Message, nameof(fileName), ae);
			}
			return this.LoadAsyncInternal(asm, context, cancellationToken);
		}

		/// <summary>
		///  アセンブリから<see cref="TakymLib.Extensibility.FeatureModule"/>を非同期的に読み込みます。
		/// </summary>
		/// <param name="assembly">読み込み元のアセンブリを指定します。</param>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。この引数は省略可能です。</param>
		/// <returns>
		///  読み込んだ<see cref="TakymLib.Extensibility.FeatureModule"/>を含むこの処理の非同期操作を返します。
		///  アセンブリに<see cref="TakymLib.Extensibility.FeatureModule"/>が含まれない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.OperationCanceledException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public LoggableTask<FeatureModule?> LoadAsync(
			Assembly                    assembly,
			ModuleInitializationContext context,
			CancellationToken           cancellationToken = default)
		{
			return this.LoadAsyncInternal(assembly, context, cancellationToken);
		}

		private async LoggableTask<FeatureModule?> LoadAsyncInternal(
			Assembly                    assembly,
			ModuleInitializationContext context,
			CancellationToken           cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (assembly is null) {
				throw new ArgumentNullException(nameof(assembly));
			}
			if (context is null) {
				throw new ArgumentNullException(nameof(context));
			}
			string name = assembly.FullName ?? assembly.GetName().FullName;
			this.Logger.LogInformation("Loading a module from \"{name}\"...", name);
			try {
				var module = await this.LoadAsyncCore(assembly, context, cancellationToken).ConfigureAwait(false);
				if (module is null) {
					this.Logger.LogWarning("\"{name}\" is not a module.", name);
				} else {
					this.Logger.LogInformation("Loaded a module from \"{name}\", {displayName}", name, module.DisplayName);
				}
				return module;
			} catch (OperationCanceledException oce) when (oce.CancellationToken == cancellationToken) {
				this.Logger.LogWarning(oce, "Canceled to load a module from \"{name}\"", name);
				throw;
			} catch (Exception e) {
				context.AddErrorOnLoad(e);
				this.Logger.LogError(e, "Failed to load a module from \"{name}\"", name);
				return null;
			}
		}

		/// <summary>
		///  アセンブリから<see cref="TakymLib.Extensibility.FeatureModule"/>を非同期的に読み込みます。
		/// </summary>
		/// <param name="assembly">読み込み元のアセンブリを指定します。</param>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。この引数は省略可能です。</param>
		/// <returns>
		///  読み込んだ<see cref="TakymLib.Extensibility.FeatureModule"/>を含むこの処理の非同期操作を返します。
		///  アセンブリに<see cref="TakymLib.Extensibility.FeatureModule"/>が含まれない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.OperationCanceledException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		protected virtual async ValueTask<FeatureModule?> LoadAsyncCore(
			Assembly                    assembly,
			ModuleInitializationContext context,
			CancellationToken           cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var module = _cache.GetOrAdd(assembly, (assembly, self) => {
				var attr = assembly.GetCustomAttribute<FeatureAddonAttribute>();
				if (attr is null) {
					return null;
				}
				switch (Activator.CreateInstance(attr.ModuleType)) {
				case FeatureModule module:
					return module;
				case not null and var obj:
					self.Logger.LogWarning("Converting the native object as a feature module...");
					return new DefaultFeatureModule(obj);
				default:
					return null;
				};
			}, this);
			if (module is not (null or DefaultFeatureModule) &&
				Interlocked.CompareExchange(
					ref module._init_state,
					FeatureModule.INIT_STATE_IN_PROCESS,
					FeatureModule.INIT_STATE_NOT_YET) == FeatureModule.INIT_STATE_NOT_YET) {
				await module.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
				module._init_state = FeatureModule.INIT_STATE_COMPLETED;
			}
			return module;
		}
	}
}
