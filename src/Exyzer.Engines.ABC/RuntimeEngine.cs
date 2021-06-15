/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace Exyzer.Engines.ABC
{
	/// <summary>
	///  <see cref="Exyzer"/>の最新の実行環境を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class RuntimeEngine : IRuntimeEngine, IRuntimeEngineInfo
	{
		private readonly Processor _processor;

		/// <inheritdoc/>
		public IRuntimeEngineInfo Information => this;

		/// <inheritdoc/>
		public IProcessor MainProcessor => _processor;

		/// <inheritdoc/>
		public string Name => "XYZ00000";

		/// <inheritdoc/>
		public uint Version => 0xABC_00000;

		/// <inheritdoc/>
		public bool IsLatest => true;

		/// <summary>
		///  型'<see cref="Exyzer.Engines.ABC.RuntimeEngine"/>'の新しいインスタンスを生成します。
		/// </summary>
		public RuntimeEngine()
		{
			_processor = new Processor(this);
		}

		/// <inheritdoc/>
		public void Connect(IDevice device)
		{
			if (device is null) {
				throw new ArgumentNullException(nameof(device));
			}
			_processor.AddDevice(device);
		}

		/// <inheritdoc/>
		public bool Disconnect(IDevice? device)
		{
			if (device is null) {
				return false;
			}
			return _processor.RemoveDevice(device);
		}

		/// <inheritdoc/>
		public IEnumerable<IDevice> EnumerateConnectedDevices()
		{
			return _processor.GetDevices();
		}
	}
}
