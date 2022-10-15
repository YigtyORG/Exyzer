/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.ComponentModel;

namespace TakymLib.GUI
{
	/// <summary>
	///  他の部品を格納する事ができる部品を表します。
	/// </summary>
	public class ContainerComponent : Component
	{
		/// <summary>
		///  コンテナを取得します。
		/// </summary>
		public Container Container { get; }

		/// <summary>
		///  型'<see cref="TakymLib.GUI.ContainerComponent"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ContainerComponent()
		{
			this.Container = new();
		}

		/// <summary>
		///  型'<see cref="TakymLib.GUI.ContainerComponent"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="container">新しく生成する部品を格納するオブジェクトを指定します。</param>
		public ContainerComponent(IContainer container) : base(container)
		{
			this.Container = new();
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			this.Container.Dispose();
			base.Dispose(disposing);
		}
	}
}
