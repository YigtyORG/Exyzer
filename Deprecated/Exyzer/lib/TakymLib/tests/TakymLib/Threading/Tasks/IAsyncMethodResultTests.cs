/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Threading.Tasks;

namespace TakymLibTests.TakymLib.Threading.Tasks
{
	[TestClass()]
	public class IAsyncMethodResultTests
	{
		[TestMethod()]
		public void VoidTest()
		{
			VoidTestCore().ConfigureAwait(false).GetAwaiter().GetResult();

			static async Task VoidTestCore()
			{
				await Run();
			}

			static async IAsyncMethodResult Run()
			{
				await Task.CompletedTask;
			}
		}

		[TestMethod()]
		public void IntTest()
		{
			IntTestCore().ConfigureAwait(false).GetAwaiter().GetResult();

			static async Task IntTestCore()
			{
				Assert.AreEqual(0, await Run(0));
				Assert.AreEqual(1, await Run(1));
				Assert.AreEqual(2, await Run(2));
				Assert.AreEqual(3, await Run(3));
			}

			static async IAsyncMethodResult<int> Run(int n)
			{
				await Task.CompletedTask;
				return n;
			}
		}

		[TestMethod()]
		public void StringTest()
		{
			StringTestCore().ConfigureAwait(false).GetAwaiter().GetResult();

			static async Task StringTestCore()
			{
				Assert.AreEqual("hello", await Run("hello"));
				Assert.AreEqual("world", await Run("world"));
				Assert.AreEqual("abcde", await Run("abcde"));
				Assert.AreEqual("12345", await Run("12345"));
			}

			static async IAsyncMethodResult<string> Run(string s)
			{
				await Task.CompletedTask;
				return s;
			}
		}
	}
}
