/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using CAP.Yencon;

namespace CAP.Internals.Yencon
{
	internal sealed class YSectionInternal : YSection
	{
		private readonly List<YNode>          _children;
		public  override IReadOnlyList<YNode> Children { get; }

		public YSectionInternal(YNode parent, string name) : base(parent, name)
		{
			_children = new List<YNode>();
			this.Children = _children.AsReadOnly();
		}

		protected override TNode? CreateNodeCore<TNode>(string name) where TNode: class
		{
			YNode? result = null;
			var    t      = typeof(TNode);
			if (t == typeof(YEmpty) || t == typeof(YEmptyInternal)) {
				_children.Add(result = new YEmptyInternal(this, name));
			} else if (t == typeof(YComment) || t == typeof(YCommentInternal)) {
				_children.Add(result = new YCommentInternal(this));
			} else if (t == typeof(YSection) || t == typeof(YSectionInternal)) {
				_children.Add(result = new YSectionInternal(this, name));
			} else if (t == typeof(YArray) || t == typeof(YArrayInternal)) {
				_children.Add(result = new YArrayInternal(this, name));
			} else if (t == typeof(YString) || t == typeof(YStringInternal)) {
				_children.Add(result = new YStringInternal(this, name));
			} else if (t == typeof(YNumber) || t == typeof(YNumberInternal)) {
				_children.Add(result = new YNumberInternal(this, name));
			} else if (t == typeof(YBoolean) || t == typeof(YBooleanInternal)) {
				_children.Add(result = new YBooleanInternal(this, name));
			} else if (t == typeof(YLink) || t == typeof(YLinkInternal)) {
				_children.Add(result = new YLinkInternal(this, name));
			}
			return result as TNode;
		}

		protected override YNode? GetNodeCore(string name)
		{
			for (int i = 0; i < _children.Count; ++i) {
				if (_children[i].Name == name) {
					return _children[i];
				}
			}
			return null;
		}

		protected override bool RemoveNodeCore(YNode node)
		{
			return _children.Remove(node);
		}

		protected override bool ContainsNameCore(string name)
		{
			for (int i = 0; i < _children.Count; ++i) {
				if (_children[i].Name == name) {
					return true;
				}
			}
			return false;
		}
	}
}
