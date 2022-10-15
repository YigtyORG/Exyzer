/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.Serialization;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>のコードファイルを表します。
	/// </summary>
	[DataContract(Namespace = nameof(Ywando) + ".xsd", IsReference = true)]
	public class YwandoCodeFile : YwandoCodeBlock
	{
		/// <summary>
		///  メタ情報を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = false)]
		public YwandoMetadata? Metadata { get; set; }
	}
}
