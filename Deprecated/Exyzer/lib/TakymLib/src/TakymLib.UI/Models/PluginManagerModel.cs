/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace TakymLib.UI.Models
{
	/// <summary>
	///  型'<see cref="TakymLib.UI.Views.PluginManagerView"/>'の内部機能を提供します。
	/// </summary>
	public class PluginManagerModel : ViewModelBase
	{
		private IReadOnlyList<PluginTreeNode>? _plugin_tree;
		private PluginTreeNode?                _selected;

		/// <summary>
		///  木構造化された<see cref="TakymLib.Extensibility.IPlugin"/>オブジェクトを取得します。
		/// </summary>
		public IReadOnlyList<PluginTreeNode> PluginTree
		{
			get => _plugin_tree ?? Array.Empty<PluginTreeNode>();
			internal set => this.RaisePropertyChanged(ref _plugin_tree, value, nameof(this.PluginTree));
		}

		/// <summary>
		///  選択された<see cref="TakymLib.UI.Models.PluginTreeNode"/>オブジェクトを取得または設定します。
		/// </summary>
		public PluginTreeNode? SelectedPluginTreeNode
		{
			get => _selected;
			set => this.RaisePropertyChanged(ref _selected, value, nameof(this.SelectedPluginTreeNode));
		}
	}
}
