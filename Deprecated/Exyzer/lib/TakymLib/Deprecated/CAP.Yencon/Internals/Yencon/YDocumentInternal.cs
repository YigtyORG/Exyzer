/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using CAP.Yencon;

namespace CAP.Internals.Yencon
{
	internal sealed class YDocumentInternal : YDocument
	{
		private          YSectionInternal?    _section;
		public  override IReadOnlyList<YNode> Children => _section?.Children ?? Array.Empty<YNode>();

		public override void Load(YenconReader reader)
		{
			throw new System.NotImplementedException();
		}

		public override void Save(YenconWriter writer)
		{
			throw new System.NotImplementedException();
		}

		protected override TNode? CreateNodeCore<TNode>(string name) where TNode: class
		{
			return _section?.CreateNode<TNode>(name);
		}

		protected override YNode? GetNodeCore(string name)
		{
			return _section?.GetNode(name);
		}

		protected override bool RemoveNodeCore(YNode node)
		{
			return _section?.RemoveNode(node) ?? false;
		}

		protected override bool ContainsNameCore(string name)
		{
			return _section?.ContainsName(name) ?? false;
		}
	}
}
