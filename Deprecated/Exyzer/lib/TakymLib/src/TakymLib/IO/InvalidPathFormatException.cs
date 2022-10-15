/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Runtime.Serialization;
using TakymLib.Properties;

namespace TakymLib.IO
{
	/// <summary>
	///  無効なパス文字列を検出した時に発生させます。
	/// </summary>
	[Serializable()]
	public class InvalidPathFormatException : IOException
	{
		private const string PREFIX = "InvalidPathFormat";

		/// <summary>
		///  この例外の原因となった無効なパス文字列を取得します。
		/// </summary>
		public string? InvalidPath { get; }

		/// <summary>
		///  型'<see cref="TakymLib.IO.InvalidPathFormatException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="path">この例外の原因となった無効なパス文字列です。</param>
		public InvalidPathFormatException(string? path)
			: base(string.Format(Resources.InvalidPathFormatException, path ?? "<UNKNOWN>"))
		{
			this.InvalidPath = path;
		}

		/// <summary>
		///  型'<see cref="TakymLib.IO.InvalidPathFormatException"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="path">この例外の原因となった無効なパス文字列です。</param>
		/// <param name="innerException">内部例外です。</param>
		public InvalidPathFormatException(string? path, Exception innerException)
			: base(string.Format(Resources.InvalidPathFormatException, path ?? "<UNKNOWN>"), innerException)
		{
			this.InvalidPath = path;
		}

		/// <summary>
		///  型'<see cref="TakymLib.IO.InvalidPathFormatException"/>'を逆直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		protected InvalidPathFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.InvalidPath = info.GetString($"{PREFIX}_{nameof(this.InvalidPath)}");
		}

		/// <summary>
		///  現在の例外を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトです。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報です。</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue($"{PREFIX}_{nameof(this.InvalidPath)}", this.InvalidPath);
		}
	}
}
