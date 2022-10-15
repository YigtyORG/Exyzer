/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書の種類を表します。
	/// </summary>
	public enum XrcdlDocumentType
	{
		/// <summary>
		///  未知の種類です。
		/// </summary>
		Unknown,

		/// <summary>
		///  文書が論文である事を示します。
		/// </summary>
		Thesis,

		/// <summary>
		///  文書が仕様書である事を示します。
		/// </summary>
		Specification,

		/// <summary>
		///  文書が許諾書である事を示します。
		/// </summary>
		License,

		/// <summary>
		///  文書が個人情報保護方針である事を示します。
		/// </summary>
		PrivacyPolicy,

		/// <summary>
		///  文書が論理体系である事を示します。
		///  <see cref="Exrecodel.XrcdlDocumentType.Thesis"/>の別名です。
		/// </summary>
		Theory = Thesis,

		/// <summary>
		///  文書が仕様書である事を示します。
		///  <see cref="Exrecodel.XrcdlDocumentType.Specification"/>の別名です。
		/// </summary>
		Spec = Specification,

		/// <summary>
		///  文書が同意書である事を示します。
		///  <see cref="Exrecodel.XrcdlDocumentType.License"/>の別名です。
		/// </summary>
		Agreement = License,

		/// <summary>
		///  文書が個人情報保護方針である事を示します。
		///  <see cref="Exrecodel.XrcdlDocumentType.PrivacyPolicy"/>の別名です。
		/// </summary>
		Privacy = PrivacyPolicy
	}
}
