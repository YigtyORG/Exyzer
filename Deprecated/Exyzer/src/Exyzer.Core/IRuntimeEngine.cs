/****
 * Exyzer
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;

namespace Exyzer
{
	/// <summary>
	///  <see cref="Exyzer"/>の実行環境を提供します。
	/// </summary>
	public interface IRuntimeEngine
	{
		/// <summary>
		///  現在の実行環境に関する情報を取得します。
		/// </summary>
		public IRuntimeEngineInfo Information { get; }

		/// <summary>
		///  既定の処理装置を取得します。
		/// </summary>
		public IProcessor MainProcessor { get; }

		/// <summary>
		///  指定された外部装置に接続します。
		/// </summary>
		/// <param name="device">接続する外部装置です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void Connect(IDevice device);

		/// <summary>
		///  指定された外部装置を切断します。
		/// </summary>
		/// <param name="device">切断する外部装置です。</param>
		/// <returns>
		///  切断に成功した場合は<see langword="true"/>、
		///  それ以外の場合(<paramref name="device"/>が<see langword="null"/>または接続されていない場合)は<see langword="false"/>を返します。
		/// </returns>
		public bool Disconnect(IDevice? device);

		/// <summary>
		///  接続された全ての外部装置を列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IEnumerable{T}"/>オブジェクトを返します。</returns>
		public IEnumerable<IDevice> EnumerateConnectedDevices();
	}

	/// <summary>
	///  <see cref="Exyzer"/>の実行環境を提供します。
	/// </summary>
	/// <typeparam name="TProcessor">既定の処理装置の種類を指定します。</typeparam>
	public interface IRuntimeEngine<out TProcessor> : IRuntimeEngine
		where TProcessor: IProcessor
	{
		/// <summary>
		///  既定の処理装置を取得します。
		/// </summary>
		public new TProcessor MainProcessor { get; }

		IProcessor IRuntimeEngine.MainProcessor => this.MainProcessor;
	}
}
