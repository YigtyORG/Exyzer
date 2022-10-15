/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ywando.Tests
{
	[TestClass()]
	public class DisposableBaseTests
	{
		[TestMethod()]
		public void DisposeTest()
		{
			DisposableBaseMock disposable;

			using (disposable = new DisposableBaseMock()) {
				Assert.IsFalse(disposable.IsDisposing);
				Assert.IsFalse(disposable.IsDisposed);
				disposable.DoSomething();
				Assert.IsFalse(disposable.IsDisposing);
				Assert.IsFalse(disposable.IsDisposed);
			}

			Assert.IsFalse(disposable.IsDisposing);
			Assert.IsTrue(disposable.IsDisposed);

			Assert.ThrowsException<ObjectDisposedException>(disposable.DoSomething);

			disposable.Dispose();
		}

		[TestMethod()]
		public void DisposeAsyncTest()
		{
			DisposeAsyncTestCore().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static async Task DisposeAsyncTestCore()
		{
			var disposable = new DisposableBaseMock();

			await using (disposable.ConfigureAwait(false)) {
				Assert.IsFalse(disposable.IsDisposing);
				Assert.IsFalse(disposable.IsDisposed);
				await disposable.DoSomethingAsync();
				Assert.IsFalse(disposable.IsDisposing);
				Assert.IsFalse(disposable.IsDisposed);
			}

			Assert.IsFalse(disposable.IsDisposing);
			Assert.IsTrue(disposable.IsDisposed);

			await Assert.ThrowsExceptionAsync<ObjectDisposedException>(async () => await disposable.DoSomethingAsync());

			await disposable.DisposeAsync();
		}

		private sealed class DisposableBaseMock : DisposableBase
		{
			internal void DoSomething()
			{
				this.EnsureNotDisposed();
			}

			internal ValueTask DoSomethingAsync()
			{
				this.EnsureNotDisposed();
				return default;
			}

			protected override void Dispose(bool disposing)
			{
				if (this.IsDisposed) {
					return;
				}
				Assert.IsTrue(this.IsDisposing);
				base.Dispose(disposing);
			}

			protected override async ValueTask DisposeAsyncCore()
			{
				if (this.IsDisposed) {
					return;
				}
				Assert.IsTrue(this.IsDisposing);
				await base.DisposeAsyncCore();
			}
		}
	}
}
