/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TakymLib.UI.Internals
{
	internal partial class FormSystemSettings : Form
	{
		private readonly IConfiguration _config;
		private readonly ILogger        _logger;

		internal FormSystemSettings(IServiceProvider provider)
		{
			_config = provider.GetRequiredService<IConfiguration>();
			_logger = provider.GetRequiredService<ILogger<FormSystemSettings>>();

			this.InitializeComponent();

			_logger.LogInformation("The system settings window is initialized.");
		}

		private void FormSystemSettings_Load(object sender, EventArgs e)
		{
			_logger.LogInformation(nameof(FormSystemSettings_Load));
			this.Reload();
		}

		private void btnReload_Click(object sender, EventArgs e)
		{
			_logger.LogInformation(nameof(btnReload_Click));
			this.Reload();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			_logger.LogInformation(nameof(btnReset_Click));
			SystemSettings
				.BeginSave()
				.ClearDisallowExtensions()
				.ClearShowSplash()
				.EndSave();
			this.Reload();
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			_logger.LogInformation(nameof(btnApply_Click));
			this.Apply();
		}

		private void btnAccept_Click(object sender, EventArgs e)
		{
			_logger.LogInformation(nameof(btnAccept_Click));
			this.Apply();
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			_logger.LogInformation(nameof(btnCancel_Click));
			this.Close();
		}

		private void Reload()
		{
			cboxDisallowExtensions.Checked = _config.DisallowExtensions();
			cboxShowSplash        .Checked = _config.ShowSplash();
		}

		private void Apply()
		{
			SystemSettings
				.BeginSave()
				.SetDisallowExtensions(cboxDisallowExtensions.Checked)
				.SetShowSplash        (cboxShowSplash        .Checked)
				.EndSave();
		}
	}
}
