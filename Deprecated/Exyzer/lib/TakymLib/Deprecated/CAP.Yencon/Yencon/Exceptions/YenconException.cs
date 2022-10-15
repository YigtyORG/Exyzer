/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using CAP.Properties;

namespace CAP.Yencon.Exceptions
{
	/// <summary>
	///  ヱンコンに関する例外を表します。
	/// </summary>
	[Serializable()]
	public class YenconException : Exception
	{
		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.YenconException"/>'の新しいインスタンスを生成します。
		/// </summary>
		public YenconException()
			: base(Resources.YenconException) { }

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.YenconException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="message">例外メッセージです。</param>
		public YenconException(string? message)
			: base(message) { }

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.YenconException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="innerException">内部例外です。</param>
		public YenconException(Exception? innerException)
			: base(Resources.YenconException, innerException) { }

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.YenconException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="message">例外メッセージです。</param>
		/// <param name="innerException">内部例外です。</param>
		public YenconException(string? message, Exception? innerException)
			: base(message, innerException) { }

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.YenconException"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected YenconException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		/// <summary>
		///  現在の例外を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
