/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace Exyzer.Tests
{
	internal sealed class MemoryDeviceMock : MemoryDevice
	{
		private readonly byte[] _data;

		public override Guid       Guid => Guid.Empty;
		public override Span<byte> Data => _data.AsSpan();
		public override bool       CanRead  { get; }
		public override bool       CanWrite { get; }

		public MemoryDeviceMock(bool canRead, bool canWrite, params byte[]? data)
		{
			_data         = data ?? Array.Empty<byte>();
			this.CanRead  = canRead;
			this.CanWrite = canWrite;
		}
	}
}
