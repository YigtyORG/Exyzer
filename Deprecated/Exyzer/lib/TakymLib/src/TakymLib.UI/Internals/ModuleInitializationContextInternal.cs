/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using TakymLib.Extensibility;
using TakymLib.UI.Models;

namespace TakymLib.UI.Internals
{
	internal sealed class ModuleInitializationContextInternal : ModuleInitializationContext
	{
		private IReadOnlyList<FeatureModule>?  _modules;
		private IReadOnlyList<PluginTreeNode>? _plugins;

		internal IReadOnlyList<FeatureModule>  Modules => _modules ?? Array.Empty<FeatureModule>();
		internal IReadOnlyList<PluginTreeNode> Plugins => _plugins ?? Array.Empty<PluginTreeNode>();

		internal void SetModulesAndPlugins(IReadOnlyList<FeatureModule>? modules, IReadOnlyList<PluginTreeNode>? plugins)
		{
			if (_modules is null) {
				_modules = modules;
			}
			if (_plugins is null) {
				_plugins = plugins;
			}
		}
	}
}
