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
	public class CommandLineParserTests
	{
		private int _state;

		[TestInitialize()]
		[TestCleanup()]
		public void ResetState()
		{
			_state = -1;
		}

		[TestMethod()]
		public void ParseTest()
		{
			var conv = Create();
			conv.PreParse  += this.conv_PreParse;
			conv.ParseNext += this.conv_ParseNext;
			conv.Parse();
		}

		private static CommandLineConverter Create()
		{
			// CommandLineParser は抽象型なので、
			// CommandLineConverter を利用して実験する。
			return CLConvFactory.Create(
				"command", "a", "b", "c",
					"-o",
					"--option",
				"/S",
				"/switch", "val0", "val1",
					"-x", "val2", "val3",
					"-y",
					"--z",
				"/", "a",
					"-", "abc",
				"/123",
					"!/123", "!!!", "#--aa"
			);
		}

		private void conv_PreParse(object? sender, PreParseEventArgs e)
		{
			if (_state != -1) {
				throw new InvalidOperationException();
			}
			Assert.AreEqual("command", e.SubCommand);
			++_state;
		}

		private void conv_ParseNext(object? sender, ParseEventArgs e)
		{
			switch (_state) {
			case 0:
				Compare(e, string.Empty, string.Empty, "a", "b", "c");
				break;
			case 1:
				Compare(e, string.Empty, "o");
				break;
			case 2:
				Compare(e, string.Empty, "-option");
				break;
			case 3:
				Compare(e, "S", string.Empty);
				break;
			case 4:
				Compare(e, "switch", string.Empty, "val0", "val1");
				break;
			case 5:
				Compare(e, "switch", "x", "val2", "val3");
				break;
			case 6:
				Compare(e, "switch", "y");
				break;
			case 7:
				Compare(e, "switch", "-z");
				break;
			case 8:
				Compare(e, string.Empty, string.Empty, "a");
				break;
			case 9:
				Compare(e, string.Empty, string.Empty, "abc");
				break;
			case 10:
				Compare(e, "123", string.Empty, "/123", "!!");
				break;
			default:
				Assert.Fail("the state value: " + _state);
				break;
			}
			++_state;
		}

		private static void Compare(ParseEventArgs e, string s, string o, params string[] vs)
		{
			Assert.AreEqual(s, e.SwitchName);
			Assert.AreEqual(o, e.OptionName);
			Assert.AreEqual(vs.Length, e.Values.Length);
			for (int i = 0; i < vs.Length; ++i) {
				Assert.AreEqual(vs[i], e.Values[i]);
			}
		}
	}
}
