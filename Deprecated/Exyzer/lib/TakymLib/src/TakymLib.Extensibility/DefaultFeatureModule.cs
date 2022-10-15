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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Extensibility
{
	internal sealed class DefaultFeatureModule : FeatureModule
	{
		private readonly ChildEnumerable _plugins;

		internal DefaultFeatureModule(object module) : base(module.GetType().Assembly)
		{
			Debug.Assert(module is not (null or FeatureModule));
			_plugins = module is IPlugin p ? new(p) : new(ObjectAdapter.Wrap(module));
		}

		protected override ValueTask InitializeAsyncCore(ModuleInitializationContext context, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) {
				return ValueTask.FromCanceled(cancellationToken);
			} else {
				return ValueTask.CompletedTask;
			}
		}

		protected override IAsyncEnumerable<IPlugin>? EnumerateChildrenAsyncCore()
		{
			return _plugins;
		}

		private sealed class ChildEnumerable : IAsyncEnumerable<IPlugin>
		{
			private readonly IPlugin _plugin;

			internal ChildEnumerable(IPlugin plugin)
			{
				if (plugin is null) {
					throw new ArgumentNullException(nameof(plugin));
				}
				_plugin = plugin;
			}

			public IAsyncEnumerator<IPlugin> GetAsyncEnumerator(CancellationToken cancellationToken = default)
			{
				return new ChildEnumerator(this, cancellationToken);
			}

			private sealed class ChildEnumerator : IAsyncEnumerator<IPlugin>
			{
				private readonly ChildEnumerable   _owner;
				private readonly CancellationToken _token;
				private          bool              _has_value;
				private          bool              _disposed;

				public IPlugin Current => _owner._plugin;

				internal ChildEnumerator(ChildEnumerable owner, CancellationToken token)
				{
					_owner     = owner;
					_token     = token;
					_has_value = true;
					_disposed  = false;
				}

				public ValueTask<bool> MoveNextAsync()
				{
					if (_token.IsCancellationRequested) {
						return ValueTask.FromCanceled<bool>(_token);
					}
					if (!_disposed && _has_value) {
						_has_value = false;
						return ValueTask.FromResult(true);
					}
					return ValueTask.FromResult(false);
				}

				public ValueTask DisposeAsync()
				{
					if (_token.IsCancellationRequested) {
						return ValueTask.FromCanceled(_token);
					}
					_disposed = true;
					return ValueTask.CompletedTask;
				}
			}
		}

		private sealed class ObjectAdapter : IPlugin
		{
			private readonly static ConcurrentDictionary<object, ObjectAdapter> _cache = new();
			private readonly        object                                      _obj;

			public string? DisplayName => _obj is IPlugin p ? p.DisplayName : _obj.GetType().Name;
			public string? Description => _obj is IPlugin p ? p.Description : _obj.ToString();

			private ObjectAdapter(object obj)
			{
				Debug.Assert(obj is not null);
				_obj = obj;
			}

			internal static ObjectAdapter Wrap(object obj)
			{
				if (obj is null) {
					throw new ArgumentNullException(nameof(obj));
				}
				return _cache.GetOrAdd(obj, key => new(key));
			}

			public IEnumerable<IPlugin> EnumerateChildren()
			{
				return _obj switch {
					IPlugin                   p => p.EnumerateChildren(),
					IEnumerable<IPlugin>      e => e,
					IAsyncEnumerable<IPlugin> e => e.ToEnumerable(),
					_                           => Enumerable.Empty<IPlugin>()
				};
			}

			public IAsyncEnumerable<IPlugin> EnumerateChildrenAsync()
			{
				return _obj switch {
					IPlugin                   p => p.EnumerateChildrenAsync(),
					IEnumerable<IPlugin>      e => e.ToAsyncEnumerable(),
					IAsyncEnumerable<IPlugin> e => e,
					_                           => AsyncEnumerable.Empty<IPlugin>()
				};
			}

			public override bool Equals(object? obj)
			{
				if (ReferenceEquals(this, obj)) {
					return true;
				}
				return _obj.Equals(obj);
			}

			public override int GetHashCode()
			{
				return _obj.GetHashCode();
			}

			public override string ToString()
			{
				return _obj.ToString() ?? string.Empty;
			}
		}
	}
}
