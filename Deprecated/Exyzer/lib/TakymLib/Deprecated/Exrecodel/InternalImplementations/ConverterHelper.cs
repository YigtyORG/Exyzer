/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using Exrecodel.Properties;

namespace Exrecodel.InternalImplementations
{
	internal static class ConverterHelper
	{
		internal static void AppendStartMetadata(this StringBuilder sb)
		{
			sb.Append("<article class=\"metadata\" id=\"metadata\"><details>");
			sb.Append($"<summary><h2>{HtmlTexts.Metadata}</h2></summary>");
		}

		internal static void AppendStartMetadataTable(this StringBuilder sb)
		{
			sb.Append("<table class=\"metadata-table\"><tbody>");
		}

		internal static void AppendMetadataTableRow(this StringBuilder sb, string keyName, string keyValue)
		{
			sb.Append("<tr><th>");
			sb.Append(keyName);
			sb.Append("</th><td>");
			sb.Append(keyValue);
			sb.Append("</td></tr>");
		}

		internal static void AppendEndMetadataTable(this StringBuilder sb)
		{
			sb.Append("</tbody></table>");
		}

		internal static void AppendStartContactList(this StringBuilder sb)
		{
			sb.Append("<div class=\"contacts\" id=\"contacts\">");
			sb.Append($"<h3>{HtmlTexts.ContactList}</h3>");
		}

		internal static void AppendStartContactInfo(this StringBuilder sb, XrcdlContactInformation info)
		{
			string? caption = HtmlTexts.ResourceManager.GetString("ContactInfo_" + info.GetContactType(), info.Metadata.Language);
			sb.Append($"<address class=\"{info.GetContactType()}\">");
			sb.Append($"<h4>{caption}</h4>");
		}

		internal static void AppendEndContactInfo(this StringBuilder sb)
		{
			sb.Append("</address>");
		}

		internal static void AppendEndContactList(this StringBuilder sb)
		{
			sb.Append("</div>");
		}

		internal static void AppendEndMetadata(this StringBuilder sb)
		{
			sb.Append("</details></article>");
		}

		internal static string? Localize(this XrcdlDocumentType type)
		{
			return HtmlTexts.ResourceManager.GetString("Metadata_Type_" + type.ToString());
		}

		internal static string? Localize(this DateTime dt)
		{
			return dt.ToString(HtmlTexts.Metadata_DateTimeFormat);
		}
	}
}
