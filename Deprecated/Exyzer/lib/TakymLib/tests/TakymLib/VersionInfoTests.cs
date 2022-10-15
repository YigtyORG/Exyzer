/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

namespace TakymLibTests.TakymLib
{
	[TestClass()]
	public class VersionInfoTests
	{
		[TestMethod()]
		public void VersionInfoTest1()
		{
			string[] authors = new VersionInfo(new AssemblyMock("A, B, C, 1 2 3,, , xyz , 000-000 ")).GetAuthorArray();
			Assert.AreEqual(6, authors.Length);
			Assert.AreEqual("A",       authors[0]);
			Assert.AreEqual("B",       authors[1]);
			Assert.AreEqual("C",       authors[2]);
			Assert.AreEqual("1 2 3",   authors[3]);
			Assert.AreEqual("xyz",     authors[4]);
			Assert.AreEqual("000-000", authors[5]);

			authors = new VersionInfo(new AssemblyMock("aaa \"bbb, ccc\" ddd")).GetAuthorArray();
			Assert.AreEqual(2, authors.Length);
			Assert.AreEqual("aaa \"bbb", authors[0]);
			Assert.AreEqual("ccc\" ddd", authors[1]);

			authors = new VersionInfo(new AssemblyMock("author-name")).GetAuthorArray();
			Assert.AreEqual(1, authors.Length);
			Assert.AreEqual("author-name", authors[0]);

			authors = new VersionInfo(new AssemblyMock(string.Empty)).GetAuthorArray();
			Assert.AreEqual(0, authors.Length);
		}

		[TestMethod()]
		public void VersionInfoTest2()
		{
			var vinfo = new DerivedVersionInfoMock();
			Assert.AreEqual("111 222 333", vinfo.Name);
			Assert.AreEqual("111 222 333", vinfo.DisplayName);
			Assert.IsTrue(vinfo.Name == vinfo.DisplayName);
			Assert.AreEqual(vinfo.Configuration, "Release");
		}

		[TestMethod()]
		public void GetCaptionTest()
		{
			DependsOn.Language();

			DerivedVersionInfoMock dvi;

			dvi = new DerivedVersionInfoMock(ConfigurationNames.Debug);
			Assert.AreEqual($"TestApp - {dvi.GetDebugEditionName()} [v1.2.3.4, cn:hoge]", dvi.GetCaption());

			dvi = new DerivedVersionInfoMock(ConfigurationNames.Release);
			Assert.AreEqual("TestApp [v1.2.3.4, cn:hoge]", dvi.GetCaption());

			dvi = new DerivedVersionInfoMock("Custom");
			Assert.AreEqual("TestApp [v1.2.3.4, cn:hoge, bc:Custom]", dvi.GetCaption());

			var evi = EmptyVersionInfoMock.Instance;
			Assert.AreEqual(" [v?.?.?.?, cn:]", evi.GetCaption());

			var nvi = NullVersionInfoMock.Instance;
			Assert.AreEqual("VersionInfo_DisplayName [v?.?.?.?, cn:unknown]", nvi.GetCaption());
		}

		private sealed class AssemblyMock : Assembly
		{
			private readonly IList<CustomAttributeData>       _attrs1;
			private readonly object[]                         _attrs2;
			private readonly AssemblyName                     _asmname;
			public  override IEnumerable<CustomAttributeData> CustomAttributes => _attrs1;

			internal AssemblyMock(string authors)
			{
				_attrs1  = new[] { new CustomAttributeDataMock (authors) };
				_attrs2  = new[] { new AssemblyCompanyAttribute(authors) };
				_asmname = new();
			}

			public override IList<CustomAttributeData> GetCustomAttributesData()
			{
				return _attrs1;
			}

			public override object[] GetCustomAttributes(bool inherit)
			{
				return _attrs2;
			}

			public override object[] GetCustomAttributes(Type attributeType, bool inherit)
			{
				if (attributeType == typeof(AssemblyCompanyAttribute)) {
					return _attrs2;
				} else {
					return Array.Empty<Attribute>();
				}
			}

			public override AssemblyName GetName(bool copiedName)
			{
				return _asmname;
			}

			private sealed class CustomAttributeDataMock : CustomAttributeData
			{
				public override Type                                AttributeType        { get; }
				public override ConstructorInfo                     Constructor          { get; }
				public override IList<CustomAttributeTypedArgument> ConstructorArguments { get; }

				internal CustomAttributeDataMock(string authors)
				{
					var type = typeof(AssemblyCompanyAttribute);

					this.AttributeType        = type;
					this.Constructor          = type.GetConstructor(new[] { typeof(string) })!;
					this.ConstructorArguments = new[] { new CustomAttributeTypedArgument(authors) };
				}
			}
		}

		private sealed class DerivedVersionInfoMock : VersionInfo
		{
			internal DerivedVersionInfoMock() : base("111 222 333", "111 222 333", null, null, null, null)
			{
				this.Configuration = ConfigurationNames.Release;
			}

			internal DerivedVersionInfoMock(string config) : base(null, "TestApp", null, null, null, "hoge")
			{
				this.Version       = new(1, 2, 3, 4);
				this.Configuration = config;
				this.Edition       = this.CreateDebugEditionName(null);
			}

			internal new string GetDebugEditionName()
			{
				return base.GetDebugEditionName();
			}
		}

		private sealed class NullVersionInfoMock : VersionInfo
		{
			internal static readonly NullVersionInfoMock Instance = new();

			private NullVersionInfoMock() : base(null, null, null, null, null, null) { }
		}

		private sealed class EmptyVersionInfoMock : VersionInfo
		{
			internal static readonly EmptyVersionInfoMock Instance = new();

			private EmptyVersionInfoMock() : base(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }
		}
	}
}
