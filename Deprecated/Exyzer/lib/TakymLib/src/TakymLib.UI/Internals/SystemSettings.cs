/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.Extensions.Configuration;

namespace TakymLib.UI.Internals
{
	internal static class SystemSettings
	{
		internal static bool DisallowExtensions(this IConfiguration? config)
		{
			return config?.GetValue(
				Keys.DisallowExtensions,
				DefaultValues.DisallowExtensions
			) ?? DefaultValues.DisallowExtensions;
		}

		internal static bool ShowSplash(this IConfiguration? config)
		{
			return config?.GetValue(
				Keys.ShowSplash,
				DefaultValues.ShowSplash
			) ?? DefaultValues.ShowSplash;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static PreservationContext BeginSave()
		{
			return PreservationContext.BeginSave();
		}

		internal static class Files
		{
			internal const string Generated = "appsettings.generated.xml";
			internal const string Custom    = "appsettings.custom.ini";
		}

		internal static class Keys
		{
			internal const string DisallowExtensions = nameof(DisallowExtensions);
			internal const string ShowSplash         = nameof(ShowSplash);
		}

		internal static class DefaultValues
		{
			internal const bool DisallowExtensions = false;
			internal const bool ShowSplash         = true;
		}

		internal sealed class PreservationContext
		{
			private  const           int                                MAX_CACHE_COUNT    = 4;
			private  static readonly ConcurrentBag<PreservationContext> _pc_cache          = new();
			private         readonly XmlDocument                        _xdoc;
			private                  bool                               _free;
			internal                 bool?                              DisallowExtensions { get; set; }
			internal                 bool?                              ShowSplash         { get; set; }

			private PreservationContext()
			{
				_xdoc = new();
				_free = false;
				this.DisallowExtensions = null;
				this.ShowSplash = null;
			}

			internal static PreservationContext BeginSave()
			{
				while (!_pc_cache.IsEmpty) {
					if (_pc_cache.TryTake(out var result) && result._free) {
						result._free = false;
						return result;
					}
				}
				return new();
			}

			internal static void ClearCache()
			{
				_pc_cache.Clear();
			}

			internal PreservationContext SetDisallowExtensions(bool value)
			{
				this.DisallowExtensions = value;
				return this;
			}

			internal PreservationContext ClearDisallowExtensions()
			{
				this.DisallowExtensions = null;
				return this;
			}

			internal PreservationContext SetShowSplash(bool value)
			{
				this.ShowSplash = value;
				return this;
			}

			internal PreservationContext ClearShowSplash()
			{
				this.ShowSplash = null;
				return this;
			}

			internal void EndSave()
			{
				Save();
				Reset();

				void Save()
				{
					var root = _xdoc.DocumentElement;
					if (root is null) {
						root = _xdoc.CreateElement("configuration");
						_xdoc.AppendChild(root);
					} else {
						root.RemoveAll();
					}
					AddNullableValue(root, Keys.DisallowExtensions, this.DisallowExtensions);
					AddNullableValue(root, Keys.ShowSplash,         this.ShowSplash);
					_xdoc.Save(Files.Generated);
				}

				void AddNullableValue<T>(XmlElement root, string key, T? value)
					where T: struct
				{
					if (value.HasValue) {
						AddValue(root, key, value.Value);
					}
				}

				void AddValue<T>(XmlElement root, string key, T value)
				{
					var elem = _xdoc.CreateElement(key);
					elem.AppendChild(_xdoc.CreateTextNode(value?.ToString()));
					root.AppendChild(elem);
				}

				void Reset()
				{
					this.DisallowExtensions = null;
					this.ShowSplash         = null;
					if (_pc_cache.Count < MAX_CACHE_COUNT) _pc_cache.Add(this);
					_free = true;
				}
			}
		}
	}
}
