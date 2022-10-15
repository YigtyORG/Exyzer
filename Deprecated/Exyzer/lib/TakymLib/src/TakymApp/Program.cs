/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using TakymLib.Logging;
using TakymLib.UI.App.Properties;
using Application = System.Windows.Forms.Application;
using MessageBox  = System.Windows.Forms.MessageBox;

namespace TakymLib.UI.App
{
	internal static class Program
	{
		private static AppHost? _ah;

		[STAThread()]
		private static int Main(string[] args)
		{
			try {
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
				Application.ThreadException                += Application_ThreadException;
				return Run(args);
			} catch (Exception e) {
				HandleException(e);
				return e.HResult;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int Run(string[] args)
		{
			_ah = new AppHost(args);
			return _ah.Run();
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception exception) {
				HandleException(exception);
			} else {
				var tmp = new Exception((sender, e, e.ExceptionObject).ToString());
				tmp.Data.Add("Sender", sender);
				tmp.Data.Add("EvArgs", e);
				HandleException(tmp);
			}
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleException(e.Exception);
		}

		private static void HandleException(Exception exception)
		{
			Debug.Fail(exception.Message, exception.ToString());
#if true
			var    path = ErrorReportBuilder.PrintAndLog(exception);
			string id   = string.Empty;
			MessageBox.Show(
				string.Format(
					Resources.HandleException_Message,
					path, exception.Message
				),
				VersionInfo.Current.GetCaption(),
				MessageBoxButtons.OK,
				MessageBoxIcon.Error
			);
#else
			MessageBox.Show(exception.Message, VersionInfo.Current.GetCaption(), MessageBoxButtons.OK, MessageBoxIcon.Error);

			var    now = DateTime.Now;
			string dir = Path.Combine(AppContext.BaseDirectory, "Logs", now.ToString("yyyy-MM"));

			if (!Directory.Exists(dir)) {
				Directory.CreateDirectory(dir);
			}

			string path = Path.Combine(AppContext.BaseDirectory, "error_report.md");
			int    pid  = Environment.ProcessId;
			string dt   = now.ToString("yyyy/MM/dd HH:mm:ss.fffffff");
			string id   = now.ToString("yyyyMMddHHmmssfffffff") + "__" + pid;

			using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None))
			using (var sw = new StreamWriter(fs, Encoding.UTF8)) {
				sw.WriteLine();
				sw.WriteLine("<a id=\"{0}\"></a>", id);
				sw.WriteLine("# **[Date/Time: {0}, PID: {1}](#{2})**", dt, pid, id);
				sw.WriteLine("```log");
				sw.WriteLine(exception);
				sw.WriteLine("```");
				sw.WriteLine("## Data");
				sw.WriteLine("|Key|Value|");
				sw.WriteLine("|:--|:--|");
				foreach (DictionaryEntry item in exception.Data) {
					sw.WriteLine(
						"|{0}|{1}|",
						item.Key.ToString()?.Replace("|", "\\|"),
						item.Value.ToString()?.Replace("|", "\\|")
					);
				}
				sw.WriteLine("----------------");
				sw.WriteLine();
			}
#endif
			_ah?.LogException(exception, "The error occurred and saved logs into: \"{LogFile}\"; ID=\"{ID}\"", path, id);
		}

#if DEBUG
		private static class DebugEnvironment
		{
			[STAThread()]
			private static int Main(string[] args)
			{
				return Run(args);
			}
		}
#endif
	}
}
