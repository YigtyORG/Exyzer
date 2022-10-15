/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>の命令を表します。
	/// </summary>
	[DataContract(IsReference = true)]
	public abstract class YwandoInstruction
	{
		/// <summary>
		///  この命令を実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void Invoke(ExecutionContext context)
		{
			context.EnsureNotNull(nameof(context));
			using (var executor = this.GetExecutor()) {
				executor.Invoke(context);
			}
		}

		/// <summary>
		///  この命令を非同期的に実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <param name="cancellationToken">処理の取り消し通知を行うトークンを指定します。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.OperationCanceledException"/>
		public async ValueTask InvokeAsync(ExecutionContext context, CancellationToken cancellationToken = default)
		{
			context.EnsureNotNull(nameof(context));
			cancellationToken.ThrowIfCancellationRequested();
			var executor = this.GetExecutor();
			await using (executor.ConfigureAwait(false)) {
				await executor.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <summary>
		///  この命令を実行する為のオブジェクトを作成します。
		/// </summary>
		/// <returns><see cref="Ywando.InstructionExecutor"/>オブジェクトです。</returns>
		public virtual InstructionExecutor GetExecutor()
		{
			return new DefaultInstructionExecutor(this);
		}

		/// <summary>
		///  上書きされた場合は、この命令を実行します。
		/// </summary>
		/// <remarks>
		///  <see cref="Ywando.YwandoInstruction.GetExecutor"/>を実装した場合は呼び出されません。
		/// </remarks>
		/// <param name="context">実行文脈情報です。</param>
		protected virtual void InvokeCore(ExecutionContext context)
		{
			// do nothing
		}

		/// <summary>
		///  上書きされた場合は、この命令を非同期的に実行します。
		/// </summary>
		/// <remarks>
		///  <see cref="Ywando.YwandoInstruction.GetExecutor"/>を実装した場合は呼び出されません。
		/// </remarks>
		/// <param name="context">実行文脈情報です。</param>
		/// <param name="cancellationToken">処理の取り消し通知を行うトークンを指定します。</param>
		/// <returns>この処理の非同期操作です。</returns>
		protected virtual ValueTask InvokeAsyncCore(ExecutionContext context, CancellationToken cancellationToken = default)
		{
			this.InvokeCore(context);
			return default;
		}

		private sealed class DefaultInstructionExecutor : InstructionExecutor
		{
			private readonly YwandoInstruction _owner;

			internal DefaultInstructionExecutor(YwandoInstruction owner)
			{
				_owner = owner;
			}

			protected override void InvokeCore(ExecutionContext context)
			{
				_owner.InvokeCore(context);
			}

			protected override ValueTask InvokeAsyncCore(ExecutionContext context, CancellationToken cancellationToken = default)
			{
				return _owner.InvokeAsyncCore(context, cancellationToken);
			}
		}
	}
}
