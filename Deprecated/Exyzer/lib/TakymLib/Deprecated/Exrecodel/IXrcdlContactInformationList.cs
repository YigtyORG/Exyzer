/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using Exrecodel.ContactInfo;

namespace Exrecodel
{
	/// <summary>
	///  連絡先情報を格納したリストを提供します。
	/// </summary>
	public interface IXrcdlContactInformationList : IXrcdlElement, IReadOnlyList<XrcdlContactInformation>, IAsyncEnumerable<XrcdlContactInformation>
	{
		/// <summary>
		///  新しい住所情報を作成して追加します。
		/// </summary>
		/// <param name="address">住所を表す文字列です。</param>
		/// <param name="postcode">郵便番号です。既定値は<see langword="null"/>です。</param>
		/// <returns>新しく生成された住所情報を操作するオブジェクトです。</returns>
		public XrcdlAddressInfo AppendAddressInfo(string address, long? postcode = null);

		/// <summary>
		///  新しい電子メールアドレス情報を作成して追加します。
		/// </summary>
		/// <param name="email">電子メールアドレスを表す文字列です。</param>
		/// <param name="subject">既定の件名を設定します。省略可能です。</param>
		/// <param name="body">既定の本文を設定します。省略可能です。</param>
		/// <returns>新しく生成された電子メールアドレス情報を操作するオブジェクトです。</returns>
		public XrcdlEmailInfo AppendEmailInfo(string email, string? subject = null, string? body = null);

		/// <summary>
		///  新しい通話用の固定電話または携帯電話の電話番号情報を作成して追加します。
		/// </summary>
		/// <param name="telNumber">電話番号を表す文字列です。</param>
		/// <returns>新しく生成された電話番号情報を操作するオブジェクトです。</returns>
		public XrcdlPhoneNumberInfo AppendTelInfo(string telNumber);

		/// <summary>
		///  新しい模写電送または写真電送用の電話番号情報を作成して追加します。
		/// </summary>
		/// <param name="faxNumber">電話番号を表す文字列です。</param>
		/// <returns>新しく生成された電話番号情報を操作するオブジェクトです。</returns>
		public XrcdlPhoneNumberInfo AppendFaxInfo(string faxNumber);

		/// <summary>
		///  新しい短文の文字列メッセージの送受信用の電話番号情報を作成して追加します。
		/// </summary>
		/// <param name="smsNumber">電話番号を表す文字列です。</param>
		/// <returns>新しく生成された電話番号情報を操作するオブジェクトです。</returns>
		public XrcdlPhoneNumberInfo AppendSmsInfo(string smsNumber);

		/// <summary>
		///  新しい社会ネットワークアカウント情報を作成して追加します。
		/// </summary>
		/// <param name="accountName">アカウント名です。</param>
		/// <param name="uriPrefix">SNSサービスへのURLの接頭辞です。</param>
		/// <param name="serviceName">SNSサービスの名前です。省略可能です。</param>
		/// <returns>新しく生成された社会ネットワークアカウント情報を操作するオブジェクトです。</returns>
		public XrcdlSocialAccountInfo AppendSocialAccountInfo(string accountName, Uri uriPrefix, string? serviceName = null);

		/// <summary>
		///  新しいURI形式のリンク情報を作成して追加します。
		/// </summary>
		/// <param name="link">URI形式のリンクです。</param>
		/// <param name="title">リンク先のオブジェクトの題名または概要です。省略可能です。</param>
		/// <returns>新しく生成されたURI形式のリンク情報を操作するオブジェクトです。</returns>
		public XrcdlLinkInfo AppendLinkInfo(Uri link, string? title = null);

		/// <summary>
		///  指定された連絡先情報を削除します。
		/// </summary>
		/// <param name="contactInfo">削除する連絡先情報です。</param>
		/// <returns>削除に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool Remove(XrcdlContactInformation contactInfo);

		/// <summary>
		///  指定された連絡先情報が格納されているかどうか判定します。
		/// </summary>
		/// <param name="contactInfo">判定する連絡先情報です。</param>
		/// <returns>格納されている場合は<see langword="true"/>、格納されていない場合は<see langword="false"/>を返します。</returns>
		public bool Contains(XrcdlContactInformation contactInfo);

		/// <summary>
		///  指定された連絡先情報の格納位置を判定します。
		/// </summary>
		/// <param name="contactInfo">判定する連絡先情報です。</param>
		/// <returns>格納されている場合は正のインデックス番号、それ以外の場合は<c>-1</c>を返します。</returns>
		public int IndexOf(XrcdlContactInformation contactInfo);
	}
}
