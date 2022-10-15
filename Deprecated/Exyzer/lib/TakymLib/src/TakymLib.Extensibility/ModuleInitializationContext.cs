/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace TakymLib.Extensibility
{
	/// <summary>
	///  拡張機能初期化時に提供される文脈情報を表します。
	/// </summary>
	public class ModuleInitializationContext
	{
		private readonly List<Exception> _errors_on_load;

		/// <summary>
		///  読み込み時に発生した例外を取得します。
		/// </summary>
		public IReadOnlyList<Exception> ErrorsOnLoad { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Extensibility.ModuleInitializationContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ModuleInitializationContext()
		{
			_errors_on_load   = new();
			this.ErrorsOnLoad = _errors_on_load.AsReadOnly();
		}

		internal void AddErrorOnLoad(Exception e)
		{
			_errors_on_load.Add(e);
		}
	}
}
