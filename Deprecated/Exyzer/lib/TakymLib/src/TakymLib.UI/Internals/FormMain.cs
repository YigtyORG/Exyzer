/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TakymLib.Extensibility;
using TakymLib.UI.Models;
using TakymLib.UI.Views;
using MICI = TakymLib.UI.Internals.ModuleInitializationContextInternal;

namespace TakymLib.UI.Internals
{
	internal sealed partial class FormMain : Form
	{
		private readonly AppHost             _app_host;
		private readonly IServiceProvider    _provider;
		private readonly ILogger             _logger;
		private readonly MICI?               _mici;
		private          FormSystemSettings? _fss;
		private          ElementWindow?      _pmgr;
		private          PluginManagerView?  _pmgr_view;
		private readonly PluginManagerModel  _pmgr_model;

		internal FormMain(AppHost appHost, IServiceProvider provider)
		{
			Debug.Assert(appHost  is not null);
			Debug.Assert(provider is not null);

			_app_host   = appHost;
			_provider   = provider;
			_logger     = provider.GetRequiredService<ILogger<FormMain>>();
			_mici       = provider.GetService<ModuleInitializationContext>() as MICI;
			_pmgr_model = new();

			this.InitializeComponent();

			_logger.LogInformation("The main window is initialized.");
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			var v = VersionInfo.Current;
			string  caption   = v.GetCaption();
			string? copyright = v.Copyright;

			_logger.LogDebug($"The {nameof(caption)}   is: \"{{{nameof(caption)}}}\".",   caption);
			_logger.LogDebug($"The {nameof(copyright)} is: \"{{{nameof(copyright)}}}\".", copyright);

			this.Text         = caption;
			lblCopyright.Text = copyright;
		}

		protected override void OnLoad(EventArgs e)
		{
			_logger.LogInformation("Loading...");

			base.OnLoad(e);

			_fss = new(_provider);
			btnSystemSettings.Text        = _fss.Text;
			btnSystemSettings.ToolTipText = _fss.Text;
			btnSystemSettings.Image       = _fss.Icon.ToBitmap();

			_pmgr = new(this.GetPluginManagerView());
			btnPluginManager.Text        = _pmgr.Text;
			btnPluginManager.ToolTipText = _pmgr.Text;
			btnPluginManager.Image       = _pmgr.Icon.ToBitmap();

			if (_mici is not null) {
				_pmgr_model.PluginTree = _mici.Plugins;
			}

			_logger.LogInformation("Loaded");
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_logger.LogInformation("Closing...");
			base.OnFormClosing(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			_logger.LogInformation("Closed");
			_app_host.Shutdown();
		}

		private void btnSystemSettings_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("Showing the system settings window...");
			this.ShowChildWindow(ref _fss, mwnd => new(mwnd._provider));
		}

		private void btnPluginManager_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("Showing the plugin manager window...");
			this.ShowChildWindow(ref _pmgr, mwnd => new(mwnd.GetPluginManagerView()));
		}

		private void ShowChildWindow<TForm>(ref TForm? form, Func<FormMain, TForm> formFactory) where TForm: Form
		{
			if (form is null || form.IsDisposed) {
				form = formFactory(this);
				form.MdiParent = this;
				form.Show();
			} else {
				if (form.MdiParent != this) {
					form.Visible   = false;
					form.MdiParent = this;
					form.Visible   = true;
				} else if (form.Visible) {
					if (form.WindowState == FormWindowState.Minimized) {
						form.WindowState = FormWindowState.Normal;
					}
					form.Activate();
				} else {
					form.MdiParent = this;
					form.Visible   = true;
				}
			}
		}

		private PluginManagerView GetPluginManagerView()
		{
			if (_pmgr_view is null) {
				_pmgr_view = new PluginManagerView(_pmgr_model);
			}
			return _pmgr_view;
		}
	}
}
