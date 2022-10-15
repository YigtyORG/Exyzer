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
using System.Diagnostics.CodeAnalysis;
using CAP.Properties;
using TakymLib;

namespace CAP.Yencon
{
	/// <summary>
	///  複数のインデックス番号付きノードを子に持つノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YArray : YNode, IReadOnlyList<YNode>
	{
		/// <summary>
		///  上書きされた場合、読み取り専用の子ノードのリストを取得します。
		/// </summary>
		public abstract IReadOnlyList<YNode> Children { get; }

		/// <summary>
		///  この配列の要素数を取得します。
		/// </summary>
		public int Count => this.Children.Count;

		/// <summary>
		///  指定されたインデックス番号のノードを取得します。
		/// </summary>
		/// <param name="index">取得するノードのインデックス番号です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.NullReferenceException"/>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"/>
		public YNode this[int index] => this.GetNode(index);

		/// <summary>
		///  型'<see cref="CAP.Yencon.YArray"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しい配列の親セクションまたは親配列です。
		/// </param>
		/// <param name="name">新しい配列の名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>
		protected YArray(YNode parent, string name) : base(parent, name) { }

		/// <summary>
		///  新しいノードを作成し追加します。
		/// </summary>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <returns>
		///  新しいノードを表すオブジェクトです。
		///  不明なノードの種類が指定された場合は<see langword="null"/>を返します。
		/// </returns>
		public TNode? CreateNode<TNode>() where TNode: YNode
		{
			return this.CreateNodeCore<TNode>();
		}

		/// <summary>
		///  新しい空値を作成し追加します。
		/// </summary>
		/// <returns>
		///  新しい空値を表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YEmpty? CreateEmpty()
		{
			return this.CreateNodeCore<YEmpty>();
		}

		/// <summary>
		///  新しいコメントを作成し追加します。
		/// </summary>
		/// <returns>
		///  新しいコメントを表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YComment? CreateComment()
		{
			return this.CreateNodeCore<YComment>();
		}

		/// <summary>
		///  新しいセクションを作成し追加します。
		/// </summary>
		/// <returns>
		///  新しいセクションを表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YSection? CreateSection()
		{
			return this.CreateNodeCore<YSection>();
		}

		/// <summary>
		///  新しい配列を作成し追加します。
		/// </summary>
		/// <returns>
		///  新しい配列を表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YArray? CreateArray()
		{
			return this.CreateNodeCore<YArray>();
		}

		/// <summary>
		///  新しい文字列値を作成し追加します。
		/// </summary>
		/// <returns>
		///  新しい文字列値を表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YString? CreateString()
		{
			return this.CreateNodeCore<YString>();
		}

		/// <summary>
		///  新しい数値を作成し追加します。
		/// </summary>
		/// <returns>
		///  新しい数値を表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YNumber? CreateNumber()
		{
			return this.CreateNodeCore<YNumber>();
		}

		/// <summary>
		///  新しい論理値を作成し追加します。
		/// </summary>
		/// <returns>
		///  新しい論理値を表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YBoolean? CreateBoolean()
		{
			return this.CreateNodeCore<YBoolean>();
		}

		/// <summary>
		///  新しいリンク文字列を作成し追加します。
		/// </summary>
		/// <returns>
		///  新しいリンク文字列を表すオブジェクトです。
		///  サポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		public YLink? CreateLink()
		{
			return this.CreateNodeCore<YLink>();
		}

		/// <summary>
		///  指定されたインデックス番号のノードを取得します。
		/// </summary>
		/// <param name="index">取得するノードのインデックス番号です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.NullReferenceException"/>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"/>
		public YNode GetNode(int index)
		{
			if (index < 0 || this.Count <= index) {
				throw new ArgumentOutOfRangeException(
					nameof(index),
					index,
					string.Format(Resources.YArray_GetNode_ArgumentOutOfRangeException, index, this.Count, nameof(index))
				);
			}
			var result = this.Children[index];
			if (result is null) {
				throw new NullReferenceException(Resources.YArray_GetNode_NullReferenceException);
			} else {
				return result;
			}
		}

		/// <summary>
		///  指定されたインデックス番号のノードを取得します。
		/// </summary>
		/// <param name="index">取得するノードのインデックス番号です。</param>
		/// <returns>取得したノードを表すオブジェクトです。</returns>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		/// <exception cref="System.NullReferenceException"/>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException"/>
		/// <exception cref="System.InvalidCastException"/>
		public TNode? GetNode<TNode>(int index) where TNode : YNode
		{
			return ((TNode)(this.GetNode(index)));
		}

		/// <summary>
		///  指定されたインデックス番号のノードを取得します。
		/// </summary>
		/// <param name="index">取得するノードのインデックス番号です。</param>
		/// <param name="result">取得したノードを表すオブジェクトです。</param>
		/// <returns>取得に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TryGetNode(int index, [NotNullWhen(true)] out YNode? result)
		{
			if (index < 0 || this.Count <= index) {
				result = null;
				return false;
			}
			result = this.Children[index];
			return result is not null;
		}

		/// <summary>
		///  指定されたインデックス番号のノードを取得します。
		/// </summary>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <param name="index">取得するノードのインデックス番号です。</param>
		/// <param name="result">取得したノードを表すオブジェクトです。</param>
		/// <returns>取得に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TryGetNode<TNode>(int index, [NotNullWhen(true)] out TNode? result) where TNode: YNode
		{
			if (this.TryGetNode(index, out var result2)) {
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
			return this.RemoveNodeCore(node);
		}

		/// <summary>
		///  指定されたインデックス番号のノードを削除します。
		/// </summary>
		/// <param name="index">削除するノードのインデックス番号です。</param>
		/// <returns>正常に削除された場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool RemoveNode(int index)
		{
			if (index < 0 || this.Count <= index) {
				return false;
			}
			var node = this.Children[index];
			if (node is not null) {
				return this.RemoveNodeCore(node);
			} else {
				return false;
			}
		}

		/// <summary>
		///  この配列の反復処理を行う列挙子を取得します。
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

		/// <summary>
		///  上書きされた場合、新しいノードを作成し追加します。
		/// </summary>
		/// <typeparam name="TNode">ノードの種類です。</typeparam>
		/// <returns>
		///  新しいノードを表すオブジェクトです。
		///  不明なノードの種類が指定された場合は<see langword="null"/>を返します。
		/// </returns>
		protected abstract TNode? CreateNodeCore<TNode>() where TNode : YNode;

		/// <summary>
		///  上書きされた場合、指定されたノードを削除します。
		/// </summary>
		/// <param name="node">削除するノードです。</param>
		/// <returns>正常に削除された場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		protected abstract bool RemoveNodeCore(YNode node);
	}
}
