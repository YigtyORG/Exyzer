/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace TakymLib.Extensibility
{
	/// <summary>
	///  アセンブリに対し既定の<see cref="TakymLib.Extensibility.FeatureModule"/>を指定します。
	///  このクラスは継承できません。
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class FeatureAddonAttribute : Attribute
	{
		/// <summary>
		///  <see cref="TakymLib.Extensibility.FeatureModule"/>の型情報を取得します。
		/// </summary>
		public Type ModuleType { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Extensibility.FeatureAddonAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="moduleType">
		///  <see cref="TakymLib.Extensibility.FeatureModule"/>の型情報を指定します。
		///  <see cref="TakymLib.Extensibility.FeatureModule"/>ではない型を指定した場合、
		///  <see cref="TakymLib.Extensibility.DefaultFeatureModule"/>が使用されます。
		/// </param>
		public FeatureAddonAttribute(Type moduleType)
		{
			moduleType.EnsureNotNull();
			this.ModuleType = moduleType;
		}
	}
}
