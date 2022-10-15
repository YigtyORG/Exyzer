/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Distributed.Internals
{
	internal class ExecutionContextManagerInternal : ExecutionContextManager
	{
		private readonly ReaderWriterLockSlim              _rwlock;
		private readonly Dictionary<int, ExecutionContext> _dict;

		internal ExecutionContextManagerInternal()
		{
			_rwlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			_dict   = new Dictionary<int, ExecutionContext>();
		}

		protected sealed override ExecutionContext GetClientContextCore()
		{
			return this.GetExecutionContext(Thread.CurrentThread);
		}

		protected sealed override ExecutionContext GetServerContextCore(Thread thread)
		{
			return this.GetExecutionContext(thread);
		}

		private ExecutionContext GetExecutionContext(Thread thread)
		{
			try {
retry:
				if (_rwlock.TryEnterReadLock(Timeout.Infinite)) {
					int id = thread.ManagedThreadId;
					if (_dict.ContainsKey(id)) {
						var result = _dict[id];
						if (result.IsDisposing || result.IsDisposed) {
							result = new ExecutionContext();
							AddContext(id, result);
						}
						return result;
					} else {
						var result = new ExecutionContext();
						AddContext(id, result);
						return result;
					}
				} else {
					goto retry;
				}
			} finally {
				if (_rwlock.IsReadLockHeld) {
					_rwlock.ExitReadLock();
				}
			}
			void AddContext(int id, ExecutionContext context)
			{
				try {
retry:
					if (_rwlock.TryEnterWriteLock(Timeout.Infinite)) {
						_dict.Add(id, context);
					} else {
						goto retry;
					}
				} finally {
					if (_rwlock.IsWriteLockHeld) {
						_rwlock.ExitWriteLock();
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				try {
retry:
					if (_rwlock.TryEnterWriteLock(Timeout.Infinite)) {
						foreach (var item in _dict.Values) {
							item.Dispose();
						}
					} else {
						goto retry;
					}
				} finally {
					if (_rwlock.IsWriteLockHeld) {
						_rwlock.ExitWriteLock();
					}
				}
				_rwlock.Dispose();
			}
			_dict.Clear();
			base.Dispose(disposing);
		}

		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			try {
retry:
				if (_rwlock.TryEnterWriteLock(Timeout.Infinite)) {
					foreach (var item in _dict.Values) {
						await item.ConfigureAwait(true).DisposeAsync();
					}
				} else {
					goto retry;
				}
			} finally {
				if (_rwlock.IsWriteLockHeld) {
					_rwlock.ExitWriteLock();
				}
			}
			if (_rwlock is IAsyncDisposable asyncDisposable) {
				await asyncDisposable.ConfigureAwait(false).DisposeAsync();
			} else {
				_rwlock.Dispose();
			}
			await base.DisposeAsyncCore();
		}
	}
}
