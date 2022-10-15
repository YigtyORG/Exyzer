/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Aspect;

namespace TakymLib.Extensibility
{
	/// <summary>
	///  拡張機能を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class FeatureModule : IPlugin
	{
		private  readonly Assembly    _asm;
		private  readonly VersionInfo _ver;
		internal volatile int         _init_state;
		internal const    int         INIT_STATE_NOT_YET    = 0;
		internal const    int         INIT_STATE_IN_PROCESS = 1;
		internal const    int         INIT_STATE_COMPLETED  = 2;

		/// <summary>
		///  この拡張機能のアセンブリ情報を取得します。
		/// </summary>
		public Assembly Assembly => _asm;

		/// <summary>
		///  この拡張機能の翻訳済みの表示名を取得します。
		/// </summary>
		/// <remarks>
		///  上書きしない場合、<see cref="TakymLib.VersionInfo.DisplayName"/>の値を返します。
		/// </remarks>
		public virtual string? DisplayName => this.Version.DisplayName;

		/// <summary>
		///  この拡張機能の翻訳済みの説明を取得します。
		/// </summary>
		/// <remarks>
		///  上書きしない場合、<see cref="TakymLib.VersionInfo.Description"/>の値を返します。
		/// </remarks>
		public virtual string? Description => this.Version.Description;

		/// <summary>
		///  この拡張機能のバージョン情報を取得します。
		/// </summary>
		public virtual VersionInfo Version => _ver;

		/// <summary>
		///  現在の<see cref="TakymLib.Extensibility.FeatureModule"/>オブジェクトの状態を取得します。
		/// </summary>
		public FeatureModuleState State => _init_state switch {
			INIT_STATE_NOT_YET    => FeatureModuleState.NotInitializedYet,
			INIT_STATE_IN_PROCESS => FeatureModuleState.Initializing,
			INIT_STATE_COMPLETED  => FeatureModuleState.Initialized,
			_                     => FeatureModuleState.Invalid
		};

		/// <summary>
		///  型'<see cref="TakymLib.Extensibility.FeatureModule"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected FeatureModule()
		{
			_asm        = this.GetType().Assembly;
			_ver        = new VersionInfo(_asm);
			_init_state = INIT_STATE_NOT_YET;
		}

		protected private FeatureModule(Assembly asm)
		{
			_asm        = asm;
			_ver        = new VersionInfo(asm);
			_init_state = INIT_STATE_NOT_YET;
		}

		/// <summary>
		///  拡張機能を非同期的に初期化します。
		/// </summary>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。この引数は省略可能です。</param>
		/// <returns>この処理の非同期操作を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.OperationCanceledException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public async LoggableTask InitializeAsync(ModuleInitializationContext context, CancellationToken cancellationToken = default)
		{
			context.EnsureNotNull();
			cancellationToken.ThrowIfCancellationRequested();
			await this.InitializeAsyncCore(context, cancellationToken);
		}

		/// <summary>
		///  この拡張機能に含まれる追加機能を列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IEnumerable{T}"/>オブジェクトを返します。</returns>
		public IEnumerable<IPlugin> EnumerateChildren()
		{
			return this.EnumerateChildrenAsyncCore()?.ToEnumerable() ?? Enumerable.Empty<IPlugin>();
		}

		/// <summary>
		///  この拡張機能に含まれる追加機能を非同期的に列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		public IAsyncEnumerable<IPlugin> EnumerateChildrenAsync()
		{
			return this.EnumerateChildrenAsyncCore() ?? AsyncEnumerable.Empty<IPlugin>();
		}

		/// <summary>
		///  上書きされた場合、この拡張機能に含まれる追加機能を非同期的に列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		protected virtual IAsyncEnumerable<IPlugin>? EnumerateChildrenAsyncCore()
		{
			return null;
		}

		/// <summary>
		///  上書きされた場合、拡張機能を非同期的に初期化します。
		/// </summary>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。</param>
		/// <returns>この処理の非同期操作を返します。</returns>
		protected abstract ValueTask InitializeAsyncCore(ModuleInitializationContext context, CancellationToken cancellationToken);

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override bool Equals(object? obj)
		{
			return base.Equals(obj);
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
