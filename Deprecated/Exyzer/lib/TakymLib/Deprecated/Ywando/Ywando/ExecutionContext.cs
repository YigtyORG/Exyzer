/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Ywando
{
	/// <summary>
	///  実行文脈情報を保持します。
	/// </summary>
	public class ExecutionContext : DisposableBase
	{
		/// <summary>
		///  親の実行文脈情報を取得します。
		/// </summary>
		public ExecutionContext? Parent { get; }

		/// <summary>
		///  この実行文脈が根かどうか判定します。
		/// </summary>
		/// <returns>
		///  根である場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public bool IsRoot => this.Parent is null;

		/// <summary>
		///  型'<see cref="Ywando.ExecutionContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ExecutionContext() { }

		/// <summary>
		///  型'<see cref="Ywando.ExecutionContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">親の実行文脈情報を指定します。</param>
		public ExecutionContext(ExecutionContext? parent)
		{
			this.Parent = parent;
		}

		/// <summary>
		///  現在の実行文脈情報を基に新しい実行文脈情報を作成します。
		/// </summary>
		/// <returns>現在の実行文脈情報の子となる<see cref="Ywando.ExecutionContext"/>オブジェクトを返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public ExecutionContext CreateScope()
		{
			this.EnsureNotDisposed();
			return new(this);
		}
	}
}
