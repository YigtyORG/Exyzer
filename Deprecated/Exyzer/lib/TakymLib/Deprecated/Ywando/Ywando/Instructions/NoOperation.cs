/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;

namespace Ywando.Instructions
{
	/// <summary>
	///  何もしない命令を表します。
	/// </summary>
	[Serializable()]
	public sealed class NoOperation : YwandoInstruction, ISerializable
	{
		/// <summary>
		///  既定のインスタンスを取得します。
		/// </summary>
		public static readonly NoOperation Instance = new();

		private NoOperation() { }

		/// <inheritdoc/>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(ObjectReference));
		}

		private sealed class ObjectReference : IObjectReference
		{
			public object GetRealObject(StreamingContext context)
			{
				return Instance;
			}
		}
	}
}
