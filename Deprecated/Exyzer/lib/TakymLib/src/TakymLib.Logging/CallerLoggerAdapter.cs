/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace TakymLib.Logging
{
	/// <summary>
	///  <see cref="Microsoft.Extensions.Logging.ILogger"/>を<see cref="TakymLib.Logging.ICallerLogger"/>として扱います。
	/// </summary>
	public class CallerLoggerAdapter : ICallerLogger
	{
		private readonly ILogger? _logger;

		/// <summary>
		///  型'<see cref="TakymLib.Logging.CallerLoggerAdapter"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="logger">ログの出力先を指定します。</param>
		public CallerLoggerAdapter(ILogger? logger)
		{
			_logger = logger;
		}

		/// <inheritdoc/>
		public void Begin(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1
		)
		{
			this.Begin($"The method began: {memberName} at \"{filePath}\"({lineNumber})", memberName, filePath, lineNumber);
		}

		/// <inheritdoc/>
		public void Begin(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		)
		{
			this.Begin<object?>(message, memberName, filePath, lineNumber, messageExpression);
		}

		/// <inheritdoc/>
		public virtual void Begin<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		)
		{
			this.LogCore(message, nameof(this.Begin), memberName, filePath, lineNumber, messageExpression);
		}

		/// <inheritdoc/>
		public void End(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1
		)
		{
			this.End($"The method ended: {memberName} at \"{filePath}\"({lineNumber})", memberName, filePath, lineNumber);
		}

		/// <inheritdoc/>
		public void End(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		)
		{
			this.End<object?>(message, memberName, filePath, lineNumber, messageExpression);
		}

		/// <inheritdoc/>
		public virtual void End<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		)
		{
			this.LogCore(message, nameof(this.End), memberName, filePath, lineNumber, messageExpression);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogCore<T>(T? message, string type, string memberName, string filePath, int lineNumber, string messageExpression)
		{
			_logger?.LogTrace(
				"{message}; {{ type={type}, memberName={memberName}, filePath={filePath}, lineNumber={lineNumber}, messageExpression={messageExpression} }}",
				message,
				type,
				memberName,
				filePath,
				lineNumber,
				messageExpression
			);
		}
	}
}
