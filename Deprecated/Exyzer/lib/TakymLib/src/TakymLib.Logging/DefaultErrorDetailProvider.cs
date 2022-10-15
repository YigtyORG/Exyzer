/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.Json;
using System.Xml;
using TakymLib.IO;
using TakymLib.Text;

namespace TakymLib.Logging
{
	/// <summary>
	///  既定の例外の追加情報を提供します。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  情報は英語で出力されます。
	/// </remarks>
	public sealed class DefaultErrorDetailProvider : IMoreExceptionsProvider
	{
		/// <summary>
		///  型'<see cref="TakymLib.Logging.DefaultErrorDetailProvider"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DefaultErrorDetailProvider() { }

		/// <inheritdoc/>
		public string GetLocalizedDetail(Exception exception)
		{
			var sb = BuildString.Begin();
			sb.AppendLine("Detailed information (English):");
			switch (exception) {
			case ApplicationException:
				sb.AppendLine(" - This is an application exception.");
				break;
			case JsonException je:
				sb.AppendLine($" - The target path is: \"{je.Path}\"");
				sb.AppendLine($" - The line number is: {je.LineNumber}");
				sb.AppendLine($" - The byte position in line: {je.BytePositionInLine}");
				break;
			case NotImplementedException:
				sb.AppendLine(" - The process was not implemented.");
				break;
			case SystemException se:
				sb.AppendLine(" - This is a system exception.");
				switch (se) {
				case ArgumentException ae:
					sb.AppendLine($" - The parameter name is: \"{ae.ParamName}\".");
					switch (ae) {
					case ArgumentOutOfRangeException aoore:
						sb.AppendLine($" - The actual value is: \"{aoore.ActualValue}\".");
						break;
					case CultureNotFoundException cnfe:
						sb.AppendLine($" - The invalid culture name is: \"{cnfe.InvalidCultureName}\".");
						sb.AppendLine($" - The invalid culture identifier is: {cnfe.InvalidCultureId}.");
						break;
					}
					break;
				case ExternalException xe:
					sb.AppendLine($" - The error code is: 0x{xe.ErrorCode:X08} ({xe.ErrorCode}).");
					switch (xe) {
					case Win32Exception w32e:
						sb.AppendLine($" - The native error code is: \"{w32e.NativeErrorCode}\".");
						break;
					}
					break;
				case InvalidOperationException ioe:
					switch (ioe) {
					case ObjectDisposedException ode:
						sb.AppendLine($" - The object name: {ode.ObjectName}");
						break;
					}
					break;
				case IOException ioe:
					switch (ioe) {
					case FileLoadException fle:
						sb.AppendLine($" - Could not load the file \"{fle.FileName}\".");
						sb.AppendLine($" - The fusion log is: \"{fle.FusionLog}\".");
						break;
					case FileNotFoundException fnfe:
						sb.AppendLine($" - The file \"{fnfe.FileName}\" does not exist.");
						sb.AppendLine($" - The fusion log is: \"{fnfe.FusionLog}\".");
						break;
					case InvalidPathFormatException ipfe:
						sb.AppendLine($" - The invalid path is: \"{ipfe.InvalidPath}\".");
						break;
					}
					break;
				case OperationCanceledException oce:
					sb.AppendLine($" - Was cancellation requested? {oce.CancellationToken.IsCancellationRequested}");
					sb.AppendLine($" - Can be canceled? {oce.CancellationToken.CanBeCanceled}");
					sb.AppendLine($" - Is the wait handle closed? {oce.CancellationToken.WaitHandle?.SafeWaitHandle?.IsClosed}");
					sb.AppendLine($" - Is the wait handle invalid? {oce.CancellationToken.WaitHandle?.SafeWaitHandle?.IsInvalid}");
					break;
				case ReflectionTypeLoadException rtle:
					var types = rtle.Types;
					if (types is not null) {
						for (int i = 0; i < types.Length; ++i) {
							sb.AppendLine($" - The type[{i}]: {types[i]?.AssemblyQualifiedName}");
						}
					}
					break;
				case TypeLoadException tle:
					sb.AppendLine($" - The type name: {tle.TypeName}");
					break;
				case SecurityException see:
					sb.AppendLine(" - There is a problem about security.");
					sb.AppendLine($" - The permission type         : {see.PermissionType?.AssemblyQualifiedName}");
					sb.AppendLine($" - The permission state        : {see.PermissionState}");
					sb.AppendLine($" - The failed assembly info    : {see.FailedAssemblyInfo?.FullName}");
					sb.AppendLine($" - The refused set             : {see.RefusedSet}");
					sb.AppendLine($" - The granted set             : {see.GrantedSet}");
					sb.AppendLine($" - The demanded                : {see.Demanded}");
					sb.AppendLine($" - The deny        set instance: {see.DenySetInstance}");
					sb.AppendLine($" - The permit only set instance: {see.PermitOnlySetInstance}");
					break;
				case XmlException xe:
					sb.AppendLine($" - The target (source URI) is: \"{xe.SourceUri}\".");
					sb.AppendLine($" - The line number   is: {xe.LineNumber}.");
					sb.AppendLine($" - The line position is: {xe.LinePosition}.");
					break;
				}
				break;
			default:
				sb.AppendLine(" - No more information.");
				break;
			}
			return BuildString.End(sb);
		}

		/// <inheritdoc/>
		public IEnumerable<Exception> GetMoreExceptions(Exception exception)
		{
			switch (exception) {
			case AggregateException ae:
				return ae.InnerExceptions;
			case ReflectionTypeLoadException rtle:
				return rtle.LoaderExceptions.Where(x => x is not null)!;
			default:
				return Enumerable.Empty<Exception>();
			}
		}
	}
}
