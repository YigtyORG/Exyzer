/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace Exyzer.Devices
{
	/// <summary>
	///  主記憶装置を表します。
	/// </summary>
	public class MainMemoryDevice : MemoryDevice
	{
		private readonly byte[] _data;

		/// <inheritdoc/>
		public override string Name { get; }

		/// <inheritdoc/>
		public override Guid Guid { get; }

		/// <summary>
		///  現在の主記憶装置の容量を取得します。
		/// </summary>
		public int Size => this.Data.Length;

		/// <inheritdoc/>
		public override Span<byte> Data => _data;

		/// <inheritdoc/>
		public override bool CanRead { get; }

		/// <inheritdoc/>
		public override bool CanWrite { get; }

		private MainMemoryDevice(string name, int size, bool canWrite)
		{
			_data = size == 0 ? Array.Empty<byte>() : new byte[size];

			this.Name     = name;
			this.Guid     = Guid.NewGuid();
			this.CanRead  = true;
			this.CanWrite = canWrite;
		}

		/// <summary>
		///  型'<see cref="Exyzer.Devices.MainMemoryDevice"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected MainMemoryDevice()
		{
			_data = Array.Empty<byte>();

			this.Name = string.Empty;
		}

		/// <summary>
		///  4キロバイトの容量を持つ読み取り専用のメモリを作成します。
		/// </summary>
		/// <returns><see cref="Exyzer.Devices.MainMemoryDevice"/>オブジェクトを返します。</returns>
		public static MainMemoryDevice CreateRom4K()
		{
			return new("ROM_04KB", 0x1000, false);
		}

		/// <summary>
		///  8キロバイトの容量を持つ読み取り専用のメモリを作成します。
		/// </summary>
		/// <returns><see cref="Exyzer.Devices.MainMemoryDevice"/>オブジェクトを返します。</returns>
		public static MainMemoryDevice CreateRom8K()
		{
			return new("ROM_08KB", 0x2000, false);
		}

		/// <summary>
		///  64キロバイトの容量を持つ読み書き可能なメモリを作成します。
		/// </summary>
		/// <returns><see cref="Exyzer.Devices.MainMemoryDevice"/>オブジェクトを返します。</returns>
		public static MainMemoryDevice CreateRwm64K()
		{
			return new("RWM_64KB", 0x00001_0000, true);
		}

		/// <summary>
		///  1メガバイトの容量を持つ読み書き可能なメモリを作成します。
		/// </summary>
		/// <returns><see cref="Exyzer.Devices.MainMemoryDevice"/>オブジェクトを返します。</returns>
		public static MainMemoryDevice CreateRwm1M()
		{
			return new("RWM_01MB", 0x0010_0000, true);
		}

		/// <summary>
		///  2メガバイトの容量を持つ読み書き可能なメモリを作成します。
		/// </summary>
		/// <returns><see cref="Exyzer.Devices.MainMemoryDevice"/>オブジェクトを返します。</returns>
		public static MainMemoryDevice CreateRwm2M()
		{
			return new("RWM_02MB", 0x0020_0000, true);
		}

		/// <summary>
		///  1ギガバイトの容量を持つ読み書き可能なメモリを作成します。
		/// </summary>
		/// <returns><see cref="Exyzer.Devices.MainMemoryDevice"/>オブジェクトを返します。</returns>
		public static MainMemoryDevice CreateRwm1G()
		{
			return new("RWM_01GB", 0x4000_0000, true);
		}
	}
}
