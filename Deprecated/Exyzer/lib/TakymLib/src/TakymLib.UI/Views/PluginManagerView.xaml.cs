/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Windows;
using System.Windows.Controls;
using TakymLib.UI.Models;
using Res = TakymLib.UI.Properties.Resources;

namespace TakymLib.UI.Views
{
	/// <summary>
	///  <see cref="TakymLib.Extensibility.IPlugin"/>を管理する為の UI を表します。
	/// </summary>
	public partial class PluginManagerView : UserControl, IElementWindowTitleProvider
	{
		private readonly PluginManagerModel _view_model;

		/// <inheritdoc/>
		public string Title { get; }

		/// <summary>
		///  型'<see cref="TakymLib.UI.Views.PluginManagerView"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="viewModel"><see cref="TakymLib.UI.Models.PluginManagerModel"/>オブジェクトを指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public PluginManagerView(PluginManagerModel viewModel)
		{
			viewModel.EnsureNotNull();
			_view_model = viewModel;

			this.InitializeComponent();
			this.DataContext = viewModel;
			this.Title       = Res.PluginManagerView_Title;
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			_view_model.SelectedPluginTreeNode = e.NewValue as PluginTreeNode;
		}
	}
}
