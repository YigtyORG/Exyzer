/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exyzer.Apps.Web.Server.Pages
{
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[IgnoreAntiforgeryToken()]
	public class ErrorModel : PageModel
	{
		public string? RequestId     { get; set; }
		public bool    ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

		public void OnGet()
		{
			this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
		}
	}
}
