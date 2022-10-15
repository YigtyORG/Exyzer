/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  引数に関する例外の発生を行います。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class ArgumentHelper
	{
		/// <summary>
		///  引数から渡されたオブジェクトが<see langword="null"/>である場合に例外を発生させます。
		/// </summary>
		/// <param name="obj">検証するオブジェクトです。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureNotNull([NotNull()] this object? obj,
			[CallerArgumentExpression("obj")] string? argName = null)
		{
			if (obj is null) {
				throw new ArgumentNullException(argName);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが<see langword="null"/>である場合に例外を発生させます。
		/// </summary>
		/// <typeparam name="T">検証するオブジェクトの種類です。</typeparam>
		/// <param name="obj">検証するオブジェクトです。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureNotNull<T>([NotNull()] this T? obj,
			[CallerArgumentExpression("obj")] string? argName = null)
		{
			if (obj is null) {
				throw new ArgumentNullException(argName);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが指定された範囲(閉区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値も範囲に含まれます。</param>
		/// <param name="max">最大値です。この値も範囲に含まれます。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureWithinClosedRange(this IComparable? actual, object? min, object? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is not null && (actual.CompareTo(min) < 0 || actual.CompareTo(max) > 0)) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinClosedRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが指定された範囲(閉区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <typeparam name="T">検証するオブジェクトの種類です。</typeparam>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値も範囲に含まれます。</param>
		/// <param name="max">最大値です。この値も範囲に含まれます。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureWithinClosedRange<T>(this IComparable<T>? actual, T? min, T? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is not null && (actual.CompareTo(min!) < 0 || actual.CompareTo(max!) > 0)) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinClosedRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが指定された範囲(開区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値は範囲に含まれません。</param>
		/// <param name="max">最大値です。この値は範囲に含まれません。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureWithinOpenRange(this IComparable? actual, object? min, object? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is not null && (actual.CompareTo(min) <= 0 || actual.CompareTo(max) >= 0)) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinOpenRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが指定された範囲(開区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <typeparam name="T">検証するオブジェクトの種類です。</typeparam>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値は範囲に含まれません。</param>
		/// <param name="max">最大値です。この値は範囲に含まれません。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureWithinOpenRange<T>(this IComparable<T>? actual, T? min, T? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is not null && (actual.CompareTo(min!) <= 0 || actual.CompareTo(max!) >= 0)) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinOpenRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが<see langword="null"/>である場合または指定された範囲(閉区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値も範囲に含まれます。</param>
		/// <param name="max">最大値です。この値も範囲に含まれます。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureNotNullWithinClosedRange([NotNull()] this IComparable? actual, object? min, object? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is null) {
				throw new ArgumentNullException(argName);
			}
			if (actual.CompareTo(min) < 0 || actual.CompareTo(max) > 0) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinClosedRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが<see langword="null"/>である場合または指定された範囲(閉区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <typeparam name="T">検証するオブジェクトの種類です。</typeparam>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値も範囲に含まれます。</param>
		/// <param name="max">最大値です。この値も範囲に含まれます。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureNotNullWithinClosedRange<T>([NotNull()] this IComparable<T>? actual, T? min, T? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is null) {
				throw new ArgumentNullException(argName);
			}
			if (actual.CompareTo(min!) < 0 || actual.CompareTo(max!) > 0) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinClosedRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが<see langword="null"/>である場合または指定された範囲(開区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値は範囲に含まれません。</param>
		/// <param name="max">最大値です。この値は範囲に含まれません。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureNotNullWithinOpenRange([NotNull()] this IComparable? actual, object? min, object? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is null) {
				throw new ArgumentNullException(argName);
			}
			if (actual.CompareTo(min) <= 0 || actual.CompareTo(max) >= 0) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinClosedRange, min, max)
				);
			}
		}

		/// <summary>
		///  引数から渡されたオブジェクトが<see langword="null"/>である場合または指定された範囲(開区間)を超える場合に例外を発生させます。
		/// </summary>
		/// <typeparam name="T">検証するオブジェクトの種類です。</typeparam>
		/// <param name="actual">検証するオブジェクトです。</param>
		/// <param name="min">最小値です。この値は範囲に含まれません。</param>
		/// <param name="max">最大値です。この値は範囲に含まれません。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ArgumentOutOfRangeException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		public static void EnsureNotNullWithinOpenRange<T>([NotNull()] this IComparable<T>? actual, T? min, T? max,
			[CallerArgumentExpression("actual")] string? argName = null)
		{
			if (actual is null) {
				throw new ArgumentNullException(argName);
			}
			if (actual.CompareTo(min!) <= 0 || actual.CompareTo(max!) >= 0) {
				throw new ArgumentOutOfRangeException(
					argName,
					actual,
					string.Format(Resources.ErrorHelper_EnsureWithinClosedRange, min, max)
				);
			}
		}

		/// <summary>
		///  <see cref="TakymLib.ArgumentHelper.EnsureNotNull(object?, string?)"/>を呼び出します。
		/// </summary>
		/// <remarks>
		///  デバッグログまたはスタックトレースへログ出力を行う場合に利用します。
		/// </remarks>
		/// <param name="obj">検証するオブジェクトです。</param>
		/// <param name="argName">検証するオブジェクトの引数名です。</param>
		/// <exception cref="System.ArgumentNullException" />
		public static void ThrowIfNull([NotNull()] this object? obj,
			[CallerArgumentExpression("obj")] string? argName = null)
		{
			obj.LogThrowIfNull(argName);
			obj.EnsureNotNull(argName);
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		private static void LogThrowIfNull(this object? obj, string? argName)
		{
			Debug.WriteLineIf(obj is     null, $"{argName} == null");
			Debug.Assert     (obj is not null, $"{argName} is null.");
		}
	}
}
