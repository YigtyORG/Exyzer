/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exyzer.Engines
{
	/// <summary>
	///  <see cref="Exyzer.IRuntimeEngine"/>を生成します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class RuntimeEngineFactory
	{
		/// <summary>
		///  実行環境「<c>XYZ00000</c>」を生成します。
		/// </summary>
		/// <returns>新しい<see cref="Exyzer.IRuntimeEngine"/>オブジェクトを返します。</returns>
		public static IRuntimeEngine CreateXYZ00000()
		{
			return new ABC.RuntimeEngine();
		}
	}
}
