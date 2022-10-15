/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Ywando.Instructions;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>のコードブロックを表します。
	/// </summary>
	[DataContract(IsReference = true)]
	public class YwandoCodeBlock : YwandoInstruction, IList<YwandoInstruction?>
	{
		private List<YwandoInstruction?> _insts;

		/// <summary>
		///  現在のコードブロック内の指定された位置の命令を取得または設定します。
		/// </summary>
		/// <param name="index">取得または設定する命令の位置です。</param>
		/// <returns><see cref="Ywando.YwandoInstruction"/>オブジェクトです。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		[IgnoreDataMember()]
		public YwandoInstruction? this[int index]
		{
			get
			{
				lock (_insts) {
					return _insts[index];
				}
			}
			set
			{
				lock (_insts) {
					_insts[index] = value;
				}
			}
		}

		/// <summary>
		///  現在のコードブロックに格納されている命令の配列を取得または設定します。
		/// </summary>
		/// <remarks>
		///  このプロパティは直列化時に利用されます。
		///  取得時または設定時に配列のコピーを行います。
		/// </remarks>
		[DataMember(IsRequired = true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public YwandoInstruction?[] Instructions
		{
			get
			{
				lock (_insts) {
					return _insts.ToArray();
				}
			}
			set
			{
				lock (_insts) {
					_insts.Clear();
					_insts.AddRange(value);
				}
			}
		}

		/// <summary>
		///  現在のコードブロックに格納されている命令の個数を取得します。
		/// </summary>
		[IgnoreDataMember()]
		public int Count
		{
			get
			{
				lock (_insts) {
					return _insts.Count;
				}
			}
		}

		bool ICollection<YwandoInstruction?>.IsReadOnly => false;

		/// <summary>
		///  型'<see cref="Ywando.YwandoCodeBlock"/>'の新しいインスタンスを生成します。
		/// </summary>
		public YwandoCodeBlock()
		{
			_insts = new();
		}

		/// <summary>
		///  現在のコードブロックの末尾に指定された命令を追加します。
		/// </summary>
		/// <param name="instruction">追加する命令です。</param>
		public void Add(YwandoInstruction? instruction)
		{
			lock (_insts) {
				_insts.Add(instruction);
			}
		}

		/// <summary>
		///  現在のコードブロックの末尾に指定された複数の命令を追加します。
		/// </summary>
		/// <param name="instructions">追加する複数の命令です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void AddRange(params YwandoInstruction?[] instructions)
		{
			instructions.EnsureNotNull(nameof(instructions));
			lock (_insts) {
				_insts.AddRange(instructions);
			}
		}

		/// <summary>
		///  現在のコードブロックの末尾に指定された複数の命令を追加します。
		/// </summary>
		/// <param name="instructions">追加する複数の命令です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void AddRange(IEnumerable<YwandoInstruction?> instructions)
		{
			instructions.EnsureNotNull(nameof(instructions));
			lock (_insts) {
				_insts.AddRange(instructions);
			}
		}

		/// <summary>
		///  現在のコードブロックの指定された位置に指定された命令を追加します。
		/// </summary>
		/// <param name="index">追加する命令の位置です。</param>
		/// <param name="instruction">追加する命令です。</param>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public void Insert(int index, YwandoInstruction? instruction)
		{
			lock (_insts) {
				_insts.Insert(index, instruction);
			}
		}

		/// <summary>
		///  現在のコードブロックの指定された位置に指定された複数の命令を追加します。
		/// </summary>
		/// <param name="index">追加する命令の位置です。</param>
		/// <param name="instructions">追加する複数の命令です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public void InsertRange(int index, params YwandoInstruction?[] instructions)
		{
			instructions.EnsureNotNull(nameof(instructions));
			lock (_insts) {
				_insts.InsertRange(index, instructions);
			}
		}

		/// <summary>
		///  現在のコードブロックの指定された位置に指定された複数の命令を追加します。
		/// </summary>
		/// <param name="index">追加する命令の位置です。</param>
		/// <param name="instructions">追加する複数の命令です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public void InsertRange(int index, IEnumerable<YwandoInstruction?> instructions)
		{
			instructions.EnsureNotNull(nameof(instructions));
			lock (_insts) {
				_insts.InsertRange(index, instructions);
			}
		}

		/// <summary>
		///  現在のコードブロックから指定された命令を削除します。
		///  同じ参照を持つ命令オブジェクトが複数存在する場合、最初の一つが削除されます。
		/// </summary>
		/// <param name="instruction">削除する命令です。</param>
		/// <returns>
		///  <paramref name="instruction"/>が存在し正常に削除された場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public bool Remove(YwandoInstruction? instruction)
		{
			lock (_insts) {
				return _insts.Remove(instruction);
			}
		}

		/// <summary>
		///  現在のコードブロックから指定された位置にある命令を削除します。
		/// </summary>
		/// <param name="index">削除する命令の位置です。</param>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public void RemoveAt(int index)
		{
			lock (_insts) {
				_insts.RemoveAt(index);
			}
		}

		// TODO: 文書コメント
		// TODO: Find系関数を実装

		public void RemoveRange(int index, int count)
		{
			lock (_insts) {
				_insts.RemoveRange(index, count);
			}
		}

		public void RemoveAll(Predicate<YwandoInstruction?> predicate)
		{
			predicate.EnsureNotNull(nameof(predicate));
			lock (_insts) {
				_insts.RemoveAll(predicate);
			}
		}

		public void RemoveNulls()
		{
			lock (_insts) {
				_insts.RemoveAll(x => x is null);
			}
		}

		public void Clear()
		{
			lock (_insts) {
				_insts.Clear();
			}
		}

		public void CopyTo(YwandoInstruction?[] array, int arrayIndex)
		{
			array.EnsureNotNull(nameof(array));
			lock (_insts) {
				_insts.CopyTo(array, arrayIndex);
			}
		}

		public int IndexOf(YwandoInstruction? instruction)
		{
			lock (_insts) {
				return _insts.IndexOf(instruction);
			}
		}

		public int IndexOf(YwandoInstruction? instruction, int index)
		{
			lock (_insts) {
				return _insts.IndexOf(instruction, index);
			}
		}

		public int IndexOf(YwandoInstruction? instruction, int index, int count)
		{
			lock (_insts) {
				return _insts.IndexOf(instruction, index, count);
			}
		}

		public int LastIndexOf(YwandoInstruction? instruction)
		{
			lock (_insts) {
				return _insts.LastIndexOf(instruction);
			}
		}

		public int LastIndexOf(YwandoInstruction? instruction, int index)
		{
			lock (_insts) {
				return _insts.LastIndexOf(instruction, index);
			}
		}

		public int LastIndexOf(YwandoInstruction? instruction, int index, int count)
		{
			lock (_insts) {
				return _insts.LastIndexOf(instruction, index, count);
			}
		}

		public bool Contains(YwandoInstruction? instruction)
		{
			lock (_insts) {
				return _insts.Contains(instruction);
			}
		}

		public List<YwandoInstruction?>.Enumerator GetEnumerator()
		{
			lock (_insts) {
				return _insts.GetEnumerator();
			}
		}

		IEnumerator<YwandoInstruction?> IEnumerable<YwandoInstruction?>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		///  現在のコードブロックに格納されている命令を順番通りに実行する為のオブジェクトを作成します。
		/// </summary>
		/// <returns><see cref="Ywando.InstructionExecutor"/>オブジェクトです。</returns>
		public override InstructionExecutor GetExecutor()
		{
			return new YwandoCodeBlockInstructionExecutor(this.Instructions);
		}

		private sealed class YwandoCodeBlockInstructionExecutor : InstructionExecutor
		{
			private readonly InstructionExecutor[] _executors;

			internal YwandoCodeBlockInstructionExecutor(YwandoInstruction?[] insts)
			{
				_executors = new InstructionExecutor[insts.Length];
				for (int i = 0; i < _executors.Length; ++i) {
					_executors[i] = insts[i]?.GetExecutor() ?? NoOperation.Instance.GetExecutor();
				}
			}

			protected override void InvokeCore(ExecutionContext context)
			{
				var executors = _executors;
				using (var scope = context.CreateScope()) {
					for (int i = 0; i < executors.Length; ++i) {
						executors[i].Invoke(scope);
					}
				}
			}

			protected override async ValueTask InvokeAsyncCore(ExecutionContext context, CancellationToken cancellationToken = default)
			{
				var executors = _executors;
				var scope     = context.CreateScope();
				await using (scope.ConfigureAwait(false)) {
					for (int i = 0; i < executors.Length; ++i) {
						await executors[i].InvokeAsync(context, cancellationToken).ConfigureAwait(false);
					}
				}
			}

			protected override void Dispose(bool disposing)
			{
				if (this.IsDisposed) {
					return;
				}
				if (disposing) {
					var executors = _executors;
					for (int i = 0; i < executors.Length; ++i) {
						executors[i].Dispose();
					}
				}
				base.Dispose(disposing);
			}

			protected override async ValueTask DisposeAsyncCore()
			{
				if (this.IsDisposed) {
					return;
				}
				var executors = _executors;
				for (int i = 0; i < executors.Length; ++i) {
					await executors[i].ConfigureAwait(false).DisposeAsync();
				}
				await base.DisposeAsyncCore();
			}
		}
	}
}
