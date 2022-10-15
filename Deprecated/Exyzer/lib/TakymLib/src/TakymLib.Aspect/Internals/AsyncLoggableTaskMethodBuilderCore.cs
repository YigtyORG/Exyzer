/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TakymLib.Aspect.Internals
{
	[StructLayout(LayoutKind.Auto)]
	internal struct AsyncLoggableTaskMethodBuilderCore
	{
		private string? _member_name;
		private string? _file_path;
		private int     _line_number;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Init()
		{
			_line_number = -2;
		}

		internal void Start()
		{
			if (LoggableTask.Logger is not null and var logger) {
				var frame = new StackFrame(2);
				var minfo = frame.GetMethod();
				_member_name = minfo?.Name;
				_file_path   = frame.GetFileName() ?? minfo?.DeclaringType?.AssemblyQualifiedName;
				_line_number = _file_path is null ? -1 : frame.GetFileLineNumber();

				logger.Begin(_member_name ?? string.Empty, _file_path ?? string.Empty, _line_number);
			}
		}

		internal void Stop()
		{
			LoggableTask.Logger?.End(_member_name ?? string.Empty, _file_path ?? string.Empty, _line_number);
		}
	}
}
