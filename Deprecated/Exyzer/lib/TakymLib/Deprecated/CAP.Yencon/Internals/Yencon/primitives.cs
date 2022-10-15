/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using CAP.Yencon;

namespace CAP.Internals.Yencon
{
	internal sealed class YEmptyInternal : YEmpty
	{
		public YEmptyInternal(YNode parent, string name)
			: base(parent, name) { }
	}

	internal sealed class YCommentInternal : YComment
	{
		public override string? Message { get; set; }

		public YCommentInternal(YNode parent)
			: base(parent) { }
	}

	internal sealed class YStringInternal : YString
	{
		public override string? Value { get; set; }

		public YStringInternal(YNode parent, string name)
			: base(parent, name) { }
	}

	internal sealed class YNumberInternal : YNumber
	{
		internal long    _s64;
		internal ulong   _u64;
		internal double  _df;
		internal decimal _m;

		public override long ValueS64
		{
			get => _s64;
			set
			{
				_s64 = value;
				_u64 = unchecked((ulong)(value));
				_df  = value;
				_m   = value;
			}
		}

		public override ulong ValueU64
		{
			get => _u64;
			set
			{
				_s64 = unchecked((long)(value));
				_u64 = value;
				_df  = value;
				_m   = value;
			}
		}

		public override double ValueDF
		{
			get => _u64;
			set
			{
				_s64 = unchecked(( long)(value));
				_u64 = unchecked((ulong)(value));
				_df  = value;
				_m   = unchecked((decimal)(value));
			}
		}

		public override decimal ValueM
		{
			get => _u64;
			set
			{
				_s64 = unchecked(( long )(value));
				_u64 = unchecked((ulong )(value));
				_df  = unchecked((double)(value));
				_m   = value;
			}
		}

		public YNumberInternal(YNode parent, string name)
			: base(parent, name) { }
	}

	internal sealed class YBooleanInternal : YBoolean
	{
		public override bool Value { get; set; }

		public YBooleanInternal(YNode parent, string name)
			: base(parent, name) { }
	}

	internal sealed class YLinkInternal : YLink
	{
		public override string? Value { get; set; }

		public YLinkInternal(YNode parent, string name)
			: base(parent, name) { }
	}
}
