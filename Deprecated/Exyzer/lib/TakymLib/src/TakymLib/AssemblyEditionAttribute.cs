using System;

namespace TakymLib
{
	/// <summary>
	///  アセンブリに改版情報を記入します。
	///  このクラスは継承できません。
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class AssemblyEditionAttribute : Attribute
	{
		/// <summary>
		///  改版名を取得します。
		/// </summary>
		public string? Edition { get; }

		/// <summary>
		///  型'<see cref="TakymLib.AssemblyEditionAttribute"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="edition">改版名です。</param>
		public AssemblyEditionAttribute(string? edition)
		{
			this.Edition = edition;
		}
	}
}
