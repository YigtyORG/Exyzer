/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Security;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.IO;
using TakymLib.Logging;

namespace TakymLibTests.TakymLib.Logging
{
	[TestClass()]
	public class ErrorReportBuilderTests
	{
		[TestMethod()]
		public void CreateTest()
		{
			Assert.IsTrue(ErrorReportBuilder.Create(new Exception()) is ErrorReportBuilder);
		}

		[TestMethod()]
		public void SaveTest()
		{
			var dir = PathStringPool.Get("Temp/Logs");
			Directory.CreateDirectory(dir);
			SaveER(dir, new Exception());
			SaveER(dir, new NullReferenceException());
			SaveER(dir, new DivideByZeroException());
			SaveER(dir, new InvalidOperationException());
			SaveER(dir, new InvalidDataException());
			SaveER(dir, new InvalidPathFormatException("xyz"));
			SaveER(dir, new AccessViolationException());
			SaveER(dir, new StackOverflowException());
			SaveER(dir, new ObjectDisposedException("a"));
			SaveER(dir, new ArgumentNullException("b"));
			SaveER(dir, new OperationCanceledException());
			SaveER(dir, new AggregateException(new Exception(), new Exception(), new Exception("c", new Exception("d", new Exception()))));
		}

		[TestMethod()]
		public void SaveTest2()
		{
			var dir = PathStringPool.Get("Temp/Logs");
			Directory.CreateDirectory(dir);
			using var cts  = new CancellationTokenSource();
			using var cts2 = new CancellationTokenSource();
			cts2.Cancel();
			var ex = new AggregateException(
				new Exception(),
				new NullReferenceException(),
				new DivideByZeroException(),
				new InvalidOperationException(),
				new InvalidDataException(),
				new InvalidPathFormatException("xyz"),
				new AccessViolationException(),
				new StackOverflowException(),
				new ObjectDisposedException("a"),
				new ArgumentNullException("b"),
				new OperationCanceledException(),
				new AggregateException(
					new Exception(),
					new Exception(),
					new Exception("c", new Exception("d", new Exception("e",
						new OperationCanceledException(cts.Token)
					))),
					new SecurityException(),
					new SecurityException(
						"message",
						this.GetType(),
						"state"
					),
					new SecurityException(
						"message",
						new Exception()
					)
				),
				new OperationCanceledException(cts2.Token),
				new OperationCanceledException(CancellationToken.None)
			);
			ErrorReportBuilder.Create(ex).Save(dir, "FULL");
		}

		private static void SaveER(PathString dir, Exception ex)
		{
			ErrorReportBuilder.Create(ex).Save(dir, null);
		}
	}
}
