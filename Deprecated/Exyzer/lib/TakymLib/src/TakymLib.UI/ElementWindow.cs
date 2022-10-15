/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TakymLib.UI
{
	/// <summary>
	///  <see cref="System.Windows.UIElement"/>を<see cref="System.Windows.Forms.Form"/>として扱います。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class ElementWindow : Form
	{
		private readonly ElementHost _eh;

		/// <summary>
		///  <see cref="System.Windows.UIElement"/>オブジェクトを取得します。
		/// </summary>
		public UIElement UIElement => _eh.Child;

		/// <summary>
		///  型'<see cref="ElementWindow"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="uiElement"><see cref="System.Windows.UIElement"/>オブジェクトを指定します。</param>
		/// <exception cref="ArgumentNullException"/>
		public ElementWindow(UIElement uiElement)
		{
			uiElement.EnsureNotNull();
			this.SuspendLayout();
			_eh        = new();
			_eh.Dock   = DockStyle.Fill;
			_eh.Child  = uiElement;
			_eh.Parent = this;
			if (uiElement is IElementWindowTitleProvider titleProvider) {
				this.Text = titleProvider.Title;
			}
			this.ResumeLayout(false);
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			_eh.Child = null;
			base.Dispose(disposing);
		}
	}
}
