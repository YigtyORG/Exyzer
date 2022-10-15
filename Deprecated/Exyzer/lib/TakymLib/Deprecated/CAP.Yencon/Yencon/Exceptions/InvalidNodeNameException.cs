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
	///  無効なノード名を検出した時に発生します。
	/// </summary>
	[Serializable()]
	public class InvalidNodeNameException : YenconException
	{
		private readonly string? _actual_name;

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="actualName">無効なノード名です。</param>
		public InvalidNodeNameException(string actualName)
			: base(string.Format(Resources.InvalidNodeNameException, actualName))
		{
			_actual_name = actualName;
		}

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="actualName">無効なノード名です。</param>
		/// <param name="innerException">内部例外です。</param>
		public InvalidNodeNameException(string actualName, Exception? innerException)
			: base(string.Format(Resources.InvalidNodeNameException, actualName), innerException)
		{
			_actual_name = actualName;
		}

		/// <summary>
		///  型'<see cref="CAP.Yencon.Exceptions.InvalidNodeNameException"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected InvalidNodeNameException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_actual_name = info.GetString($"{nameof(InvalidNodeNameException)}.{nameof(_actual_name)}");
		}

		/// <summary>
		///  現在の例外を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue($"{nameof(InvalidNodeNameException)}.{nameof(_actual_name)}", _actual_name);
		}
	}
}
