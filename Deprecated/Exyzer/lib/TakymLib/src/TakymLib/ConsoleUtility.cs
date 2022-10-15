/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Security;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  標準入出力ストリームをより便利に扱う為の機能を提供します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class ConsoleUtility
	{
		/// <summary>
		///  利用者がキーを押すまでプログラムの処理を中断します。
		/// </summary>
		public static void Pause()
		{
			Console.Write(Resources.ConsoleUtil_Pause);
			Console.Write(' ');
			Console.ReadKey(true);
			Console.WriteLine();
		}

		/// <summary>
		///  利用者に「はい」または「いいえ」の二択で答える事のできる質問をします。
		/// </summary>
		/// <param name="question">質問文です。疑問符は自動的に付加されません。</param>
		/// <returns>
		///  利用者が<c>Y</c>または<c>y</c>を入力した場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>です。
		/// </returns>
		public static bool ReadYesNo(string question)
		{
			Console.Write($"{question} [Y/n] ");
			var cki = Console.ReadKey();
			Console.WriteLine();
			return cki.KeyChar == 'Y' || cki.KeyChar == 'y';
		}

		/// <summary>
		///  パスワードを入力します。
		///  画面上にパスワードを表す文字列は表示されません。
		/// </summary>
		/// <param name="secretChar">パスワードの代わりに表示する文字です。</param>
		/// <returns>セキュリティで保護された文字列です。</returns>
		public static SecureString ReadPassword(char secretChar = '*')
		{
			var ss = new SecureString();
			Console.Write(Resources.ConsoleUtil_ReadPassword);
			Console.Write(' ');
			while (true) {
				var cki = Console.ReadKey(true);
				switch (cki.Key) {
				case ConsoleKey.Enter:
					Console.WriteLine();
					ss.MakeReadOnly();
					return ss;
				case ConsoleKey.Backspace:
					if (ss.Length != 0) {
						ss.RemoveAt(ss.Length - 1);
						--Console.CursorLeft;
						Console.Write(' ');
						--Console.CursorLeft;
					}
					break;
				default:
					if (cki.KeyChar != '\0') {
						ss.AppendChar(cki.KeyChar);
						Console.Write(secretChar);
					}
					break;
				}
			}
		}

		/// <summary>
		///  区切り線を出力します。
		/// </summary>
		/// <param name="splitter">区切り線に利用する文字です。</param>
		public static void WriteHorizontalRule(char splitter = '=')
		{
			Console.WriteLine();
			Console.WriteLine(new string(splitter, 16));
			Console.WriteLine();
		}

		/// <summary>
		///  題名、バージョン情報、及び著作者情報をコンソール画面に出力します。
		/// </summary>
		/// <param name="version">バージョン情報を表すオブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public static void Print(this VersionInfo version)
		{
			version.EnsureNotNull();
			Console.WriteLine(version.GetCaption());
			Console.WriteLine(version.Copyright);
			Console.WriteLine();
		}
	}
}
