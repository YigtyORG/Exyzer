/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TakymLib.UI.Internals
{
	internal sealed partial class FormSplash : Form
	{
		private Point _old_point;

		internal FormSplash()
		{
			this.InitializeComponent();
		}

		private void FormSplash_Load(object sender, EventArgs e)
		{
			var v = VersionInfo.Current;
			this        .Text = v.GetCaption();
			lblCaption  .Text = v.GetCaption();
			lblCopyright.Text = v.Copyright;

			using (var font = lblCaption.Font) {
				lblCaption.Font = new Font(font.FontFamily, 14.0F);
			}
		}

		private void FormSplash_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button.HasFlag(MouseButtons.Left)) {
				_old_point = e.Location;
			}
		}

		private void FormSplash_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button.HasFlag(MouseButtons.Left)) {
				this.Left += e.Location.X - _old_point.X;
				this.Top  += e.Location.Y - _old_point.Y;
			}
		}
	}
}
