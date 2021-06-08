/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Linq;

namespace Exyzer
{
	/// <summary>
	///  型'<see cref="Exyzer.IRuntimeEngine"/>'の機能を拡張します。
	/// </summary>
	public static class RuntimeEngineExtensions
	{
		/// <summary>
		///  接続された全ての外部装置を取得します。
		/// </summary>
		/// <returns><see cref="Exyzer.IDevice"/>の配列を返します。</returns>
		public static IDevice[] GetConnectedDevices(this IRuntimeEngine runtimeEngine)
		{
			if (runtimeEngine is null) {
				throw new ArgumentNullException(nameof(runtimeEngine));
			}
			return runtimeEngine.EnumerateConnectedDevices().ToArray();
		}
	}
}
