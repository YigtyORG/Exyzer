/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using TakymLib;

namespace Exyzer.Devices
{
	/// <summary>
	///  副記憶装置(倉庫)を表します。
	/// </summary>
	public class StorageDevice : MemoryDevice
	{
		private readonly byte[] _data;

		/// <inheritdoc/>
		public sealed override string Name { get; }

		/// <inheritdoc/>
		public sealed override Guid Guid { get; }

		/// <inheritdoc/>
		public override Span<byte> Data => _data;

		/// <inheritdoc/>
		public sealed override bool CanRead { get; }

		/// <inheritdoc/>
		public sealed override bool CanWrite { get; }

		/// <summary>
		///  型'<see cref="Exyzer.Devices.StorageDevice"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">副記憶装置の名前を指定します。</param>
		/// <param name="guid">副記憶装置の一意識別子を指定します。</param>
		/// <param name="data">副記憶装置の初期データを指定します。</param>
		/// <param name="canRead">読み取りが可能かどうかを示す論理値を指定します。</param>
		/// <param name="canWrite">書き込みが可能かどうかを示す論理値を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public StorageDevice(string name, Guid guid, byte[] data, bool canRead, bool canWrite)
		{
			name.EnsureNotNull(nameof(name));
			data.EnsureNotNull(nameof(data));

			_data         = data;
			this.Name     = name;
			this.Guid     = guid;
			this.CanRead  = canRead;
			this.CanWrite = canWrite;
		}
	}
}
