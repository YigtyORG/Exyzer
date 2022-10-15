/****
 * CAP - "Configuration and Property"
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using TakymLib;

namespace CAP
{
	/// <summary>
	///  型'<see cref="CAP.Yencon.YenconReader"/>'と型'<see cref="CAP.Yencon.YenconWriter"/>'の基底クラスです。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class CapStream : DisposableBase
	{
		/// <summary>
		///  型'<see cref="CAP.CapStream"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected CapStream() { }
	}
}
