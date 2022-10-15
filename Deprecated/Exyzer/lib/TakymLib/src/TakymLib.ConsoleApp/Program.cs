/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TakymLib.Text;

namespace TakymLib.ConsoleApp
{
	internal static class Program
	{
		private static async ValueTask Run()
		{
			//DownloadLatestDefinitionAndConvert();
			using (var hc = new HttpClient()) {
				await DownloadEAWDefinitionAndConvert( "4.1.0", hc);
				await DownloadEAWDefinitionAndConvert( "5.0.0", hc);
				await DownloadEAWDefinitionAndConvert( "5.1.0", hc);
				await DownloadEAWDefinitionAndConvert( "5.2.0", hc);
				await DownloadEAWDefinitionAndConvert( "6.0.0", hc);
				await DownloadEAWDefinitionAndConvert( "6.1.0", hc);
				await DownloadEAWDefinitionAndConvert( "6.2.0", hc);
				await DownloadEAWDefinitionAndConvert( "6.3.0", hc);
				await DownloadEAWDefinitionAndConvert( "7.0.0", hc);
				await DownloadEAWDefinitionAndConvert( "8.0.0", hc);
				await DownloadEAWDefinitionAndConvert( "9.0.0", hc);
				await DownloadEAWDefinitionAndConvert("10.0.0", hc);
				await DownloadEAWDefinitionAndConvert("11.0.0", hc);
				await DownloadEAWDefinitionAndConvert("12.0.0", hc);
				await DownloadEAWDefinitionAndConvert("12.1.0", hc);
				await DownloadEAWDefinitionAndConvert("13.0.0", hc);
				await DownloadEAWDefinitionAndConvert("14.0.0", hc);
			}
		}

		private static async ValueTask DownloadEAWDefinitionAndConvert(string version, HttpClient hc)
		{
			var eaw = await EastAsianWidth.DownloadDefinitionFromAsync(
				new($"https://www.unicode.org/Public/{version}/ucd/EastAsianWidth.txt"),
				hc
			).ConfigureAwait(false);

			var ranges = eaw.Ranges;
			int count  = ranges.Count;
			var fs     = new FileStream(version + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
			await using (fs.ConfigureAwait(false)) {
				var sw = new StreamWriter(fs, Encoding.UTF8);
				await using (sw.ConfigureAwait(false)) {
					for (int i = 0; i < count; ++i) {
						var range = ranges[i];
						if (range.Type == EastAsianWidthType.Invalid) {
							continue;
						}
						if (range.Start > ushort.MaxValue || range.End > ushort.MaxValue) {
							continue;
						}
						if (range.Start == range.End) {
							await sw.WriteAsync(string.Format("\'\\u{0:X04}\'", range.Start));
						} else {
							await sw.WriteAsync(string.Format(">= \'\\u{0:X04}\' and <= \'\\u{1:X04}\'", range.Start, range.End));
						}
						await sw.WriteAsync(" => ");
						await sw.WriteAsync(nameof(EastAsianWidthType));
						await sw.WriteAsync('.');
						await sw.WriteAsync(range.Type.ToString());
						await sw.WriteLineAsync(",");
					}
				}
			}
			await Console.Out.WriteLineAsync(version);
		}

		private static void DownloadLatestDefinitionAndConvert()
		{
			var sb     = BuildString.Begin();
			var ranges = EastAsianWidth.DownloadLatestDefinition().Ranges;
			int count  = ranges.Count;
			for (int i = 0; i < count; ++i) {
				var range = ranges[i];
				if (range.Type == EastAsianWidthType.Invalid) {
					continue;
				}
				if (range.Start > ushort.MaxValue || range.End > ushort.MaxValue) {
					continue;
				}
				if (range.Start == range.End) {
					sb.AppendFormat("\'\\u{0:X04}\'", range.Start);
				} else {
					sb.AppendFormat(">= \'\\u{0:X04}\' and <= \'\\u{1:X04}\'", range.Start, range.End);
				}
				sb.Append(" => ");
				sb.Append(nameof(EastAsianWidthType));
				sb.Append('.');
				sb.Append(range.Type);
				sb.AppendLine(",");
			}
			Console.WriteLine(BuildString.End(sb));
		}

		[STAThread()]
		private static async Task<int> Main(string[] args)
		{
			try {
				Console.WriteLine("TakymLib Testing Utility");
				Console.WriteLine("========================");
				VersionInfo.Current.Print();
				await Run();
				return 0;
			} catch (Exception e) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine();
				Console.Error.WriteLine(e.ToString());
				Console.ResetColor();
				return e.HResult;
			}
		}
	}
}
