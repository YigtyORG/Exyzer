/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Threading;

namespace TakymLibTests.TakymLib.Threading
{
	[TestClass()]
	public class SimpleLockerTests
	{
		#region Test #1

		[TestMethod()]
		public void Test1_DoYield()
		{
			Test1(true);
		}

		[TestMethod()]
		public void Test1_DoNotYield()
		{
			Test1(false);
		}

		[TestMethod()]
		public void AsyncTest1_DoYield()
		{
			AsyncTest1(true);
		}

		[TestMethod()]
		public void AsyncTest1_DoNotYield()
		{
			AsyncTest1(false);
		}

		private static void Test1(bool doYield)
		{
			var  locker = new SimpleLocker(doYield);
			bool taken  = false;

			Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
			locker.EnterLock(ref taken);
			Assert.AreEqual(SimpleLocker.State.Locked, locker.GetState());
			Assert.IsTrue(taken);
			Assert.IsTrue(locker.LeaveLock());
			Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
			Assert.IsTrue(locker.LeaveLock());
		}

		private static void AsyncTest1(bool doYield)
		{
			AsyncTest1Core(doYield).ConfigureAwait(false).GetAwaiter().GetResult();

			static async Task AsyncTest1Core(bool doYield)
			{
				var locker = new SimpleLocker(doYield);

				Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
				await locker.EnterLockAsync();
				Assert.AreEqual(SimpleLocker.State.Locked, locker.GetState());
				Assert.IsTrue(await locker.LeaveLockAsync());
				Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
				Assert.IsTrue(await locker.LeaveLockAsync());
			}
		}

		#endregion

		#region Test #2

		[TestMethod()]
		public void Test2_DoYield()
		{
			Test2(true);
		}

		[TestMethod()]
		public void Test2_DoNotYield()
		{
			Test2(false);
		}

		[TestMethod()]
		public void AsyncTest2_DoYield()
		{
			AsyncTest2(true);
		}

		[TestMethod()]
		public void AsyncTest2_DoNotYield()
		{
			AsyncTest2(false);
		}

		private static void Test2(bool doYield)
		{
			var  locker = new SimpleLocker(doYield);
			bool taken  = false;
			int  value  = 0;

			Test2Core().ConfigureAwait(false).GetAwaiter().GetResult();

			Assert.AreEqual(32, value);

			Task Test2Core()
			{
				var tasks = new Task[32];
				for (int i = 0; i < tasks.Length; ++i) {
					tasks[i] = Task.Run(Run);
				}
				return Task.WhenAll(tasks);
			}

			void Run()
			{
				try {
					locker.EnterLock(ref taken);
					++value;
				} finally {
					if (taken) {
						locker.LeaveLock();
					}
				}
			}
		}

		private static void AsyncTest2(bool doYield)
		{
			var locker = new SimpleLocker(doYield);
			int value  = 0;

			AsyncTest2Core().ConfigureAwait(false).GetAwaiter().GetResult();

			Assert.AreEqual(32, value);

			Task AsyncTest2Core()
			{
				var tasks = new Task[32];
				for (int i = 0; i < tasks.Length; ++i) {
					tasks[i] = Task.Run(RunAsync);
				}
				return Task.WhenAll(tasks);
			}

			async Task RunAsync()
			{
				try {
					await locker.EnterLockAsync();
					++value;
				} finally {
					await locker.LeaveLockAsync();
				}
			}
		}

		#endregion
	}
}
