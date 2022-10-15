/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.CommandLine;
using TakymLibTests.Properties;

namespace TakymLibTests.TakymLib.CommandLine
{
	[TestClass()]
	public class ManualBuilderTests
	{
		[TestMethod()]
		public void ParseTest()
		{
			var conv = CLConvFactory.Create();
			conv.AddType<SwitchA>();
			conv.AddType<SwitchB>();
			conv.AddType<SwitchC>();
			var help = new ManualBuilder(conv);
			help.Build();
			Assert.AreEqual(
				Resources.CL_ManualBuilder_Result.Replace("\r\n", "\n").Replace("\n\r", "\n").Replace('\r', '\n'),
				help.ToString()                  .Replace("\r\n", "\n").Replace("\n\r", "\n").Replace('\r', '\n')
			);
		}

		[TestMethod()]
		public void ParseTestIV()
		{
			DependsOn.Language("iv");
			this.ParseTest();
		}

		[TestMethod()]
		public void ParseTestJA()
		{
			DependsOn.Language("ja");
			this.ParseTest();
		}

		[TestMethod()]
		public void ParseTestEN()
		{
			DependsOn.Language("en");
			this.ParseTest();
		}

		[Switch("A")]
		public class SwitchA
		{
			[Option("option-a", "a")]
			public object? OptionA { get; set; }

			[Option("option-b", "b")]
			public object? OptionB { get; set; }

			[Option("option-c", "c")]
			public object? OptionC { get; set; }

			[Option("long-option")]
			public object? LongOption { get; set; }
		}

		[Switch("B")]
		public class SwitchB : SwitchA, IHelpProvider
		{
			public void WriteHelp(StringBuilder sb, string? optionName = null)
			{
				switch (optionName) {
				case null:
					sb.Append("test switch");
					break;
				case "option-a":
					sb.Append("test option A");
					break;
				case "option-b":
					sb.Append("test option B");
					break;
				case "option-c":
					sb.Append("test option C");
					break;
				case "long-option":
					sb.Append("test long option");
					break;
				}
			}
		}

		[Switch("C")]
		public class SwitchC : SwitchA { }
	}
}
