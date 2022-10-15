/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using CAP.Yencon.Extensions;
using TakymLib;

namespace CAP.Yencon
{
	/// <summary>
	///  ヱンコン環境設定ファイルを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YDocument : YSection
	{
		/// <summary>
		///  型'<see cref="CAP.Yencon.YDocument"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected YDocument() : base(null, string.Empty) { }

		/// <summary>
		///  指定したリーダーからヱンコン情報を読み取ります。
		/// </summary>
		/// <param name="reader">読み込み元のリーダーです。</param>
		public abstract void Load(YenconReader reader);

		/// <summary>
		///  指定したライターへヱンコン情報を書き込みます。
		/// </summary>
		/// <param name="writer">書き込み先のライターです。</param>
		public abstract void Save(YenconWriter writer);

		/// <summary>
		///  指定されたリンク文字列からノードを取得します。
		/// </summary>
		/// <param name="link">リンク文字列です。</param>
		/// <returns>
		///  指定されたリンク文字列からノードを取得できた場合はそのノードを表すオブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public YNode? GetNodeByLink(string link)
		{
			link.EnsureNotNull(nameof(link));
			string[] names   = link.Split('.');
			YSection section = this;
			for (int i = 0; i < names.Length - 1; ++i) {
				var section2 = section.GetSection(names[i]);
				if (section2 is null) {
					return null;
				} else {
					section = section2;
				}
			}
			return null;
		}
	}
}
