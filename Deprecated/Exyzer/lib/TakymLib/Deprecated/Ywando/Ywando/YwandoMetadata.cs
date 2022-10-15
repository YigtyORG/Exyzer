/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>のメタ情報を表します。
	/// </summary>
	[DataContract()]
	public class YwandoMetadata
	{
		/// <summary>
		///  <see cref="Ywando.YwandoCodeFile"/>の名前を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public string? Name { get; set; }

		/// <summary>
		///  <see cref="Ywando.YwandoCodeFile"/>の説明を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public string? Description { get; set; }

		/// <summary>
		///  <see cref="Ywando.YwandoCodeFile"/>の作成者を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public string[]? Authors { get; set; }

		/// <summary>
		///  <see cref="Ywando.YwandoCodeFile"/>の著作権表記を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public string? Copyright { get; set; }

		/// <summary>
		///  <see cref="Ywando.YwandoCodeFile"/>のバージョンを取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public Version? Version { get; set; }

		/// <summary>
		///  <see cref="Ywando.YwandoCodeFile"/>の開発コード名を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public string? CodeName { get; set; }
	}
}
