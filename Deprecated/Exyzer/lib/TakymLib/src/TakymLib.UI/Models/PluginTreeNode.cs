/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using TakymLib.Extensibility;

namespace TakymLib.UI.Models
{
	/// <summary>
	///  <see cref="TakymLib.Extensibility.IPlugin"/>階層の節を表します。
	/// </summary>
	public class PluginTreeNode
	{
		/// <summary>
		///  この節の<see cref="TakymLib.Extensibility.IPlugin"/>を取得します。
		/// </summary>
		public IPlugin Plugin { get; }

		/// <summary>
		///  子機能を含む読み取り専用のリストを取得します。
		/// </summary>
		public IReadOnlyList<PluginTreeNode> Children { get; }

		/// <summary>
		///  <see cref="TakymLib.UI.Models.PluginTreeNode.Plugin"/>を
		///  <see cref="TakymLib.Extensibility.FeatureModule"/>として取得します。
		/// </summary>
		/// <returns>
		///  <see cref="TakymLib.Extensibility.FeatureModule"/>オブジェクトを返します。
		///  変換できなかった場合は<see langword="null"/>を返します。
		/// </returns>
		public FeatureModule? FeatureModule => this.Plugin as FeatureModule;

		/// <summary>
		///  型'<see cref="TakymLib.UI.Models.PluginTreeNode"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="plugin"><see cref="TakymLib.Extensibility.IPlugin"/>オブジェクトを指定します。</param>
		/// <param name="children"> 子機能を含む読み取り専用のリストを指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public PluginTreeNode(IPlugin plugin, IReadOnlyList<PluginTreeNode> children)
		{
			plugin  .EnsureNotNull();
			children.EnsureNotNull();
			this.Plugin   = plugin;
			this.Children = children;
		}
	}
}
