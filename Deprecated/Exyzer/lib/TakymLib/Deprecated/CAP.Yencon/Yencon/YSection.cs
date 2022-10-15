/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CAP.Properties;
using TakymLib;

namespace CAP.Yencon
{
	/// <summary>
	///  複数の名前付きノードを子に持つノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YSection : YNode, IReadOnlyCollection<YNode>, IReadOnlyDictionary<string, YNode>
	{
		private          ulong               _version;
		private readonly IEnumerable<string> _keys;

		IEnumerable<string> IReadOnlyDictionary<string, YNode>.Keys   => _keys;
		IEnumerable<YNode>  IReadOnlyDictionary<string, YNode>.Values => this.Children;

		/// <summary>
		///  現在の子ノードの名前を取得します。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		public string[] ChildNames => _keys.ToArray();

		/// <summary>
		///  上書きされた場合、読み取り専用の子ノードのリストを取得します。
		/// </summary>
		public abstract IReadOnlyList<YNode> Children { get; }

		/// <summary>
		///  このセクションの要素数を取得します。
		/// </summary>
		public int Count => this.Children.Count;

		/// <summary>
		///  指定された名前のノードを取得します。
		/// </summary>
		/// <param name="name">取得するノードの名前です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.NullReferenceException"/>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"/>
		public YNode this[string name] => this.GetNode(name);

		/// <summary>
		///  型'<see cref="CAP.Yencon.YSection"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しいセクションの親セクションまたは親配列です。
		///  このセクションが根セクションの場合は<see langword="null"/>になります。
		/// </param>
		/// <param name="name">新しいセクションの名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		protected YSection(YNode? parent, string name) : base(parent, name)
		{
			_version = 0;
			_keys    = new KeyNameEnumerable(this);
		}

		/// <summary>
		///  新しいノードを作成し追加します。
		/// </summary>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <param name="name">新しいノードの名前です。</param>
		/// <returns>
		///  新しいノードを表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  または不明なノードの種類が指定された場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public TNode? CreateNode<TNode>(string name) where TNode: YNode
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<TNode>(name);
		}

		/// <summary>
		///  新しい空値を作成し追加します。
		/// </summary>
		/// <param name="name">新しい空値の名前です。</param>
		/// <returns>
		///  新しい空値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YEmpty? CreateEmpty(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YEmpty>(name);
		}

		/// <summary>
		///  新しいコメントを作成し追加します。
		/// </summary>
		/// <returns>
		///  新しいコメントを表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public YComment? CreateComment()
		{
			++_version;
			return this.CreateNodeCore<YComment>(string.Empty);
		}

		/// <summary>
		///  新しいセクションを作成し追加します。
		/// </summary>
		/// <param name="name">新しいセクションの名前です。</param>
		/// <returns>
		///  新しいセクションを表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YSection? CreateSection(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YSection>(name);
		}

		/// <summary>
		///  新しい配列を作成し追加します。
		/// </summary>
		/// <param name="name">新しい配列の名前です。</param>
		/// <returns>
		///  新しい配列を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YArray? CreateArray(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YArray>(name);
		}

		/// <summary>
		///  新しい文字列値を作成し追加します。
		/// </summary>
		/// <param name="name">新しい文字列値の名前です。</param>
		/// <returns>
		///  新しい文字列値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YString? CreateString(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YString>(name);
		}

		/// <summary>
		///  新しい数値を作成し追加します。
		/// </summary>
		/// <param name="name">新しい数値の名前です。</param>
		/// <returns>
		///  新しい数値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YNumber? CreateNumber(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YNumber>(name);
		}

		/// <summary>
		///  新しい論理値を作成し追加します。
		/// </summary>
		/// <param name="name">新しい論理値の名前です。</param>
		/// <returns>
		///  新しい論理値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YBoolean? CreateBoolean(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YBoolean>(name);
		}

		/// <summary>
		///  新しいリンク文字列を作成し追加します。
		/// </summary>
		/// <param name="name">新しいリンク文字列の名前です。</param>
		/// <returns>
		///  新しいリンク文字列を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		public YLink? CreateLink(string name)
		{
			name.EnsureNotNull(nameof(name));
			++_version;
			return this.CreateNodeCore<YLink>(name);
		}

		/// <summary>
		///  指定された名前のノードを取得します。
		/// </summary>
		/// <param name="name">取得するノードの名前です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.NullReferenceException"/>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"/>
		public YNode GetNode(string name)
		{
			name.EnsureNotNull(nameof(name));
			if (!this.ContainsNameCore(name)) {
				throw new KeyNotFoundException(string.Format(Resources.YSection_GetNode_KeyNotFoundException, name));
			}
			var result = this.GetNodeCore(name);
			if (result is null) {
				throw new NullReferenceException(Resources.YSection_GetNode_NullReferenceException);
			} else {
				return result;
			}
		}

		/// <summary>
		///  指定された名前のノードを取得します。
		/// </summary>
		/// <param name="name">取得するノードの名前です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.NullReferenceException"/>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"/>
		/// <exception cref="System.InvalidCastException"/>
		public TNode? GetNode<TNode>(string name) where TNode : YNode
		{
			return ((TNode)(this.GetNode(name)));
		}

		/// <summary>
		///  指定された名前のノードを取得します。
		/// </summary>
		/// <param name="name">取得するノードの名前です。</param>
		/// <param name="result">取得したノードを表すオブジェクトです。</param>
		/// <returns>取得に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TryGetNode(string name, [NotNullWhen(true)] out YNode? result)
		{
			result = null;
			if (name is null) {
				return false;
			}
			if (!this.ContainsNameCore(name)) {
				return false;
			}
			result = this.GetNodeCore(name);
			return result is not null;
		}

		/// <summary>
		///  指定された名前のノードを取得します。
		/// </summary>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <param name="name">取得するノードの名前です。</param>
		/// <param name="result">取得したノードを表すオブジェクトです。</param>
		/// <returns>取得に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TryGetNode<TNode>(string name, [NotNullWhen(true)] out TNode? result) where TNode: YNode
		{
			if (this.TryGetNode(name, out var result2)) {
				result = result2 as TNode;
				return result is not null;
			} else {
				result = null;
				return false;
			}
		}

		/// <summary>
		///  指定されたノードを削除します。
		/// </summary>
		/// <param name="node">削除するノードです。</param>
		/// <returns>正常に削除された場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public bool RemoveNode(YNode node)
		{
			node.EnsureNotNull(nameof(node));
			if (this.RemoveNodeCore(node)) {
				++_version;
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		///  指定された名前のノードを削除します。
		/// </summary>
		/// <param name="name">削除するノードの名前です。</param>
		/// <returns>正常に削除された場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public bool RemoveNode(string name)
		{
			name.EnsureNotNull(nameof(name));
			var node = this.GetNodeCore(name);
			if (node is not null) {
				if (this.RemoveNodeCore(node)) {
					++_version;
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}

		/// <summary>
		///  指定された名前のノードが存在するかどうか判定します。
		/// </summary>
		/// <param name="name">検索するノードの名前です。</param>
		/// <returns>存在する場合は<see langword="true"/>、存在しない場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public bool ContainsName(string name)
		{
			name.EnsureNotNull(nameof(name));
			return this.ContainsNameCore(name);
		}

		/// <summary>
		///  このセクションの反復処理を行う列挙子を取得します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IEnumerator{T}"/>オブジェクトです。</returns>
		public IEnumerator<YNode> GetEnumerator()
		{
			return this.Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator<KeyValuePair<string, YNode>> IEnumerable<KeyValuePair<string, YNode>>.GetEnumerator()
		{
			return new KeyValueEnumerator(this);
		}

		bool IReadOnlyDictionary<string, YNode>.TryGetValue(string key, out YNode value)
		{
			return this.TryGetNode(key, out value!);
		}

		bool IReadOnlyDictionary<string, YNode>.ContainsKey(string key)
		{
			return this.ContainsNameCore(key);
		}

		/// <summary>
		///  上書きされた場合、新しいノードを作成し追加します。
		/// </summary>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <param name="name">新しいノードの名前です。</param>
		/// <returns>
		///  新しいノードを表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  または不明なノードの種類が指定された場合は<see langword="null"/>を返します。
		/// </returns>
		protected abstract TNode? CreateNodeCore<TNode>(string name) where TNode : YNode;

		/// <summary>
		///  上書きされた場合、指定された名前のノードを取得します。
		/// </summary>
		/// <param name="name">取得するノードの名前です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		protected abstract YNode? GetNodeCore(string name);

		/// <summary>
		///  上書きされた場合、指定されたノードを削除します。
		/// </summary>
		/// <param name="node">削除するノードです。</param>
		/// <returns>正常に削除された場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		protected abstract bool RemoveNodeCore(YNode node);

		/// <summary>
		///  上書きされた場合、指定された名前のノードが存在するかどうか判定します。
		/// </summary>
		/// <param name="name">検索するノードの名前です。</param>
		/// <returns>存在する場合は<see langword="true"/>、存在しない場合は<see langword="false"/>を返します。</returns>
		protected abstract bool ContainsNameCore(string name);

		private sealed class KeyNameEnumerable : IEnumerable<string>
		{
			private readonly YSection _owner;

			internal KeyNameEnumerable(YSection owner)
			{
				_owner = owner;
			}

			public IEnumerator<string> GetEnumerator()
			{
				return new KeyNameEnumerator(_owner);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			private struct KeyNameEnumerator : IEnumerator<string>
			{
				private readonly YSection _owner;
				private          ulong    _version;
				private          int      _index;
				private          bool     _is_disposed;

				public string Current
				{
					get
					{
						this.EnsureStatus();
						return _owner.Children[_index].Name;
					}
				}

				object? IEnumerator.Current => this.Current;

				internal KeyNameEnumerator(YSection owner)
				{
					_owner       = owner;
					_version     = owner._version;
					_index       = -1;
					_is_disposed = false;
				}

				public bool MoveNext()
				{
					this.EnsureStatus();
					++_index;
					if (0 <= _index && _index < _owner.Count) {
						if (string.IsNullOrEmpty(this.Current)) {
							return this.MoveNext();
						} else {
							return true;
						}
					} else {
						return false;
					}
				}

				public void Reset()
				{
					_version     = _owner._version;
					_index       = -1;
					_is_disposed = false;
				}

				public void Dispose()
				{
					_is_disposed = true;
				}

				[DebuggerHidden()]
				[StackTraceHidden()]
				private void EnsureStatus()
				{
					if (_is_disposed) {
						throw new ObjectDisposedException(nameof(KeyNameEnumerator));
					}
					if (_version != _owner._version) {
						throw new InvalidOperationException(Resources.YSection_KeyNameEnumerator);
					}
				}
			}
		}

		private struct KeyValueEnumerator : IEnumerator<KeyValuePair<string, YNode>>
		{
			private readonly YSection            _owner;
			private readonly IEnumerator<string> _enumerator;
			private          bool                _is_disposed;

			public KeyValuePair<string, YNode> Current
			{
				get
				{
					if (_is_disposed) {
						throw new ObjectDisposedException(nameof(KeyValueEnumerator));
					}
					string name = _enumerator.Current;
					return new KeyValuePair<string, YNode>(name, _owner[name]);
				}
			}

			object? IEnumerator.Current => this.Current;

			internal KeyValueEnumerator(YSection owner)
			{
				_owner       = owner;
				_enumerator  = ((IReadOnlyDictionary<string, YNode>)(owner)).Keys.GetEnumerator();
				_is_disposed = false;
			}

			public bool MoveNext()
			{
				if (_is_disposed) {
					throw new ObjectDisposedException(nameof(KeyValueEnumerator));
				}
				return _enumerator.MoveNext();
			}

			public void Reset()
			{
				_enumerator.Reset();
				_is_disposed = false;
			}

			public void Dispose()
			{
				_enumerator.Dispose();
				_is_disposed = true;
			}
		}
	}
}

namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct, Inherited = false)]
	internal sealed class StackTraceHiddenAttribute : Attribute { }
}
