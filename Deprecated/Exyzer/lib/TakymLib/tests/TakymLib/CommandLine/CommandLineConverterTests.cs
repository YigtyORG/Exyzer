/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.CommandLine;

namespace TakymLibTests.TakymLib.CommandLine
{
	[TestClass()]
	public class CommandLineConverterTests
	{
		[TestMethod()]
		public void ParseTest()
		{
			var conv = Create();
			Assert.IsTrue(conv.AddType<FileList>());
			conv.PreParse += this.Conv_PreParse;
			conv.Parse();
			var filelist = conv.Get<FileList>();
			Assert.IsNotNull(filelist);
			Assert.AreEqual("123456", filelist?.Inputs[0]);
		}

		private static CommandLineConverter Create()
		{
			return CLConvFactory.Create("command", "/files", "--input", "abc.xyz", "/files", "123456");
		}

		private void Conv_PreParse(object? sender, PreParseEventArgs e)
		{
			Assert.AreEqual("command", e.SubCommand);
		}

		[Switch("files")]
		public class FileList
		{
			[Option("input", "")]
			public string[] Inputs { get; set; }

			public FileList()
			{
				this.Inputs = Array.Empty<string>();
			}
		}
	}
}
