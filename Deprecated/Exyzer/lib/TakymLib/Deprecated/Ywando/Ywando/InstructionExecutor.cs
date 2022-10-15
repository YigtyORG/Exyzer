/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading;
using System.Threading.Tasks;

namespace Ywando
{
	/// <summary>
	///  <see cref="Ywando.YwandoInstruction"/>の実行を行います。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class InstructionExecutor : DisposableBase
	{
		/// <summary>
		///  命令を実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public void Invoke(ExecutionContext context)
		{
			this.EnsureNotDisposed();
			context.EnsureNotNull(nameof(context));
			this.InvokeCore(context);
		}

		/// <summary>
		///  命令を非同期的に実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <param name="cancellationToken">処理の取り消し通知を行うトークンを指定します。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public ValueTask InvokeAsync(ExecutionContext context, CancellationToken cancellationToken = default)
		{
			this.EnsureNotDisposed();
			context.EnsureNotNull(nameof(context));
			if (cancellationToken.IsCancellationRequested) {
				return ValueTask.FromCanceled(cancellationToken);
			}
			return this.InvokeAsyncCore(context, cancellationToken);
		}

		/// <summary>
		///  上書きされた場合は、命令を実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		protected abstract void InvokeCore(ExecutionContext context);

		/// <summary>
		///  上書きされた場合は、命令を非同期的に実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <param name="cancellationToken">処理の取り消し通知を行うトークンを指定します。</param>
		/// <returns>この処理の非同期操作です。</returns>
		protected virtual ValueTask InvokeAsyncCore(ExecutionContext context, CancellationToken cancellationToken = default)
		{
			this.InvokeCore(context);
			return default;
		}
	}
}
