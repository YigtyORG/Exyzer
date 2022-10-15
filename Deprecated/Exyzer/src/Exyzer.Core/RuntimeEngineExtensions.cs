/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Linq;
using TakymLib;

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
		/// <exception cref="System.ArgumentNullException"/>
		public static IDevice[] GetConnectedDevices(this IRuntimeEngine runtimeEngine)
		{
			runtimeEngine.EnsureNotNull(nameof(runtimeEngine));
			return runtimeEngine.EnumerateConnectedDevices().ToArray();
		}
	}
}
