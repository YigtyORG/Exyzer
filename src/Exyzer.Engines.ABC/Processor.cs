/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
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
		private readonly RuntimeEngine     _owner;
		private readonly List<IDevice>     _devices;
		private          MainMemoryDevice? _rom;
		private          MainMemoryDevice? _rwm;
		private          int               _ip;

		/// <inheritdoc/>
		public int RegisterCount => 1;

		internal Processor(RuntimeEngine owner)
		{
			_owner   = owner;
			_devices = new();
			_ip      = 0;
		}

		/// <inheritdoc/>
		public void RunNext()
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc/>
		public void RunStart()
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc/>
		public bool TryGetFormattedRegisterValue(int index, ValueFormat format, [NotNullWhen(true)] out string? value)
		{
			if (index == 0) {
				if (format == ValueFormat.DisplayName) {
					value = "IP";
				} else {
					value = ValueFormatter.ToFormattedString(_ip, format);
				}
				return true;
			} else {
				value = null;
				return false;
			}
		}

		/// <inheritdoc/>
		public bool TrySetFormattedRegisterValue(int index, ValueFormat format, string? value)
		{
			throw new NotImplementedException();
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
