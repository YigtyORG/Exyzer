/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Exyzer.Devices;

namespace Exyzer.Engines.ABC
{
	/// <summary>
	///  <see cref="Exyzer"/>の最新の処理装置を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class Processor : IProcessor
	{
		private const    byte              NOP = 0x00;
		private readonly RuntimeEngine     _owner;
		private readonly List<IDevice>     _devices;
		private          MainMemoryDevice? _rom;
		private          MainMemoryDevice? _rwm;
		private          int               _ip;

		/// <inheritdoc/>
		public int RegisterCount { get; }

		internal Processor(RuntimeEngine owner)
		{
			_owner   = owner;
			_devices = new();
			_ip      = 0;
		}

		/// <inheritdoc/>
		public void RunNext()
		{
			this.RunNextCore();
		}

		/// <inheritdoc/>
		public void RunStart()
		{
			while (this.RunNextCore()) ;
		}

		private bool RunNextCore()
		{
			int ip    = _ip;
			int index = ip;
			if (this.TryGetSpan(ref index, false, out var mem) && index < mem.Length) {
				switch (mem[index]) {
				case NOP:
					break;
				}
				Interlocked.Exchange(ref _ip, ip >= 0 ? ip + 1 : ip - 1);
				return true;
			}
			return false;
		}

		private bool TryGetSpan(ref int index, bool willWrite, [MaybeNullWhen(false)][NotNullWhen(true)] out Span<byte> result)
		{
			if (index >= 0 && _rom is not null and var rom && !willWrite) {
				result = rom.Data;
				return true;
			}
			if (index < 0 && _rwm is not null and var rwm) {
				index = -index;
				result = rwm.Data;
				return true;
			}
			result = default;
			return false;
		}

		/// <inheritdoc/>
		public bool TryGetFormattedRegisterValue(int index, ValueFormat format, [MaybeNullWhen(false)][NotNullWhen(true)] out string? value)
		{
			switch (index) {
			case 0:
				if (format == ValueFormat.DisplayName) {
					value = "IP";
					return true;
				} else {
					return ValueFormatter.TryToString(_ip, format, out value);
				}
			}

			value = null;
			return false;
		}

		/// <inheritdoc/>
		public bool TrySetFormattedRegisterValue(int index, ValueFormat format, string? value)
		{
			switch (index) {
			case 0:
				if (ValueFormatter.TryToSInt32(value, format, out int ip)) {
					_ip = ip;
					return true;
				}
				break;
			}

			return false;
		}

		internal void AddDevice(IDevice device)
		{
			lock (_devices) {
				_devices.Add(device);
			}
			if (device is MainMemoryDevice mem) {
				if (!mem.CanWrite && _rom is null) {
					Interlocked.CompareExchange(ref _rom, mem, null);
				} else if (mem.CanWrite && _rwm is null) {
					Interlocked.CompareExchange(ref _rwm, mem, null);
				}
			}
		}

		internal bool RemoveDevice(IDevice device)
		{
			if (device == _rom || device == _rwm) {
				return false;
			}
			lock (_devices) {
				return _devices.Remove(device);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal IEnumerable<IDevice> GetDevices()
		{
			return _devices;
		}
	}
}
