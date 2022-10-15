/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ywando.Tests
{
	[TestClass()]
	public class VersionInfoTests
	{
		[TestMethod()]
		public void VersionInfoTest1()
		{
			string[] authors = new VersionInfo(new AssemblyMock("A, B, C, 1 2 3,, , xyz , 000-000 ")).Authors;
			Assert.AreEqual(6, authors.Length);
			Assert.AreEqual("A",       authors[0]);
			Assert.AreEqual("B",       authors[1]);
			Assert.AreEqual("C",       authors[2]);
			Assert.AreEqual("1 2 3",   authors[3]);
			Assert.AreEqual("xyz",     authors[4]);
			Assert.AreEqual("000-000", authors[5]);

			authors = new VersionInfo(new AssemblyMock("aaa \"bbb, ccc\" ddd")).Authors;
			Assert.AreEqual(2, authors.Length);
			Assert.AreEqual("aaa \"bbb", authors[0]);
			Assert.AreEqual("ccc\" ddd", authors[1]);

			authors = new VersionInfo(new AssemblyMock("author-name")).Authors;
			Assert.AreEqual(1, authors.Length);
			Assert.AreEqual("author-name", authors[0]);

			authors = new VersionInfo(new AssemblyMock(string.Empty)).Authors;
			Assert.AreEqual(0, authors.Length);
		}

		[TestMethod()]
		public void VersionInfoTest2()
		{
			var vinfo = new VersionInfo(new YwandoMetadata() { Name = "111 222 333" });
			Assert.AreEqual("111 222 333", vinfo.Name);
			Assert.AreEqual("111 222 333", vinfo.DisplayName);
			Assert.IsTrue(vinfo.Name == vinfo.DisplayName);
			Assert.AreEqual(vinfo.Configuration, "Release");
		}

		[TestMethod()]
		public void GetCaptionTest()
		{
			VersionInfo vi;

			vi = new VersionInfoMock(ConfigurationNames.Debug);
			Assert.AreEqual("TestApp - DEBUG [v1.2.3.4, cn:hoge]", vi.GetCaption());

			vi = new VersionInfoMock(ConfigurationNames.Release);
			Assert.AreEqual("TestApp [v1.2.3.4, cn:hoge]", vi.GetCaption());

			vi = new VersionInfoMock("Custom");
			Assert.AreEqual("TestApp [v1.2.3.4, cn:hoge, bc:Custom]", vi.GetCaption());

			vi = NullVersionInfoMock.Instance;
			Assert.AreEqual(" [v?.?.?.?, cn:Unknown]", vi.GetCaption());
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
					this.AttributeType        = typeof(AssemblyCompanyAttribute);
					this.Constructor          = this.AttributeType.GetConstructor(new[] { typeof(string) })!;
					this.ConstructorArguments = new[] { new CustomAttributeTypedArgument(authors) };
				}
			}
		}

		private sealed class VersionInfoMock : VersionInfo
		{
			public override string?  DisplayName   => "TestApp";
			public override Version? Version       => new(1, 2, 3, 4);
			public override string?  CodeName      => "hoge";
			public override string?  Configuration { get; }

			internal VersionInfoMock(string config)
			{
				this.Configuration = config;
			}
		}

		private sealed class NullVersionInfoMock : VersionInfo
		{
			internal static readonly NullVersionInfoMock Instance = new();

			private NullVersionInfoMock() { }
		}
	}
}
