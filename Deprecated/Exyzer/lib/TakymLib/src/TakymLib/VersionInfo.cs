/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Linq;
using System.Reflection;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  <see cref="System.Reflection.Assembly"/>のバージョン情報を提供します。
	/// </summary>
	public class VersionInfo
	{
		private const string UNKNOWN_NAME     = "Unknown";
		private const string UNKNOWN_CODENAME = "unknown";

		private static readonly char[] _separator = new[] { ',', ';', '\u3001', '\uFF0C', '\uFF1B', '\uFF64' };

		/// <summary>
		///  このライブラリ(<see cref="TakymLib"/>)のバージョン情報を取得します。
		/// </summary>
		public static VersionInfo Library { get; } = new(typeof(VersionInfo).Assembly);

		/// <summary>
		///  現在のプロセスの既定のアセンブリのバージョン情報を取得します。
		/// </summary>
		public static VersionInfo Current { get; } = new(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());

		/// <summary>
		///  現在の<see cref="System.AppDomain"/>に読み込まれている全てのアセンブリのバージョン情報をコンソール画面に出力します。
		/// </summary>
		public static void PrintAllAssemblies()
		{
			var asms = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < asms.Length; ++i) {
				new VersionInfo(asms[i]).Print();
			}
		}

		/// <summary>
		///  コンストラクタに渡されたアセンブリ情報を取得します。
		/// </summary>
		public Assembly? Assembly { get; protected init; }

		/// <summary>
		///  アセンブリの内部名を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  アセンブリの表示名を取得します。
		/// </summary>
		public string DisplayName { get; }

		/// <summary>
		///  アセンブリの作成者情報を取得します。
		/// </summary>
		public string Authors { get; }

		/// <summary>
		///  アセンブリの著作者情報を取得します。
		/// </summary>
		public string Copyright { get; }

		/// <summary>
		///  アセンブリの説明を取得します。
		/// </summary>
		public string Description { get; }

		/// <summary>
		///  アセンブリのバージョン情報を取得します。
		/// </summary>
		public Version? Version { get; protected init; }

		/// <summary>
		///  アセンブリの開発コード名を取得します。
		/// </summary>
		public string CodeName { get; }

		/// <summary>
		///  アセンブリのビルド構成を取得します。
		/// </summary>
		/// <remarks>
		///  一般的なビルド構成は<see cref="TakymLib.VersionInfo.Configuration"/>を参照してください。
		/// </remarks>
		public string? Configuration { get; protected init; }

		/// <summary>
		///  アセンブリの改版名を取得します。
		/// </summary>
		public string? Edition { get; protected init; }

		/// <summary>
		///  型'<see cref="TakymLib.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <remarks>
		///  現在実行中のアセンブリのバージョン情報を取得します。
		/// </remarks>
		public VersionInfo() : this(Assembly.GetCallingAssembly()) { }

		/// <summary>
		///  型'<see cref="TakymLib.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="asm">バージョン情報の取得元のアセンブリです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public VersionInfo(Assembly asm)
		{
			asm.EnsureNotNull();
			var asmname        = asm.GetName();
			this.Assembly      = asm;
			this.Name          = asmname.Name ?? UNKNOWN_NAME;
			this.DisplayName   = asm.GetCustomAttribute<AssemblyProductAttribute>    ()?.Product     ?? Resources.VersionInfo_DisplayName;
			this.Authors       = asm.GetCustomAttribute<AssemblyCompanyAttribute>    ()?.Company     ?? Resources.VersionInfo_Authors;
			this.Copyright     = asm.GetCustomAttribute<AssemblyCopyrightAttribute>  ()?.Copyright   ?? Resources.VersionInfo_Copyright;
			this.Description   = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? Resources.VersionInfo_Description;
			this.Version       = asmname.Version;
			this.CodeName      = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? UNKNOWN_CODENAME;
			this.Configuration = asm.GetCustomAttribute<AssemblyConfigurationAttribute>       ()?.Configuration;
			this.Edition       = this.CreateDebugEditionName(asm.GetCustomAttribute<AssemblyEditionAttribute>()?.Edition);
		}

		/// <summary>
		///  型'<see cref="TakymLib.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">アセンブリの内部名を指定します。</param>
		/// <param name="displayName">アセンブリの表示名を指定します。</param>
		/// <param name="authors">アセンブリの作成者情報を指定します。</param>
		/// <param name="copyright">アセンブリの著作権情報を指定します。</param>
		/// <param name="description">アセンブリの説明を指定します。</param>
		/// <param name="codeName">アセンブリの開発コード名を指定します。</param>
		protected VersionInfo(string? name, string? displayName, string? authors, string? copyright, string? description, string? codeName)
		{
			this.Name        = name        ?? UNKNOWN_NAME;
			this.DisplayName = displayName ?? Resources.VersionInfo_DisplayName;
			this.Authors     = authors     ?? Resources.VersionInfo_Authors;
			this.Copyright   = copyright   ?? Resources.VersionInfo_Copyright;
			this.Description = description ?? Resources.VersionInfo_Description;
			this.CodeName    = codeName    ?? UNKNOWN_CODENAME;
		}

		/// <summary>
		///  指定されたバージョン情報が現在のバージョン情報に対し互換性を持つかどうか判定します。
		/// </summary>
		/// <param name="other">比較対象のバージョン情報です。</param>
		/// <returns>互換性を持つ場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public bool HasCompatibleWith(VersionInfo other)
		{
			return this.HasForwardCompatibleWith(other) && this.HasBackwardCompatibleWith(other);
		}

		/// <summary>
		///  指定されたバージョン情報が現在のバージョン情報に対し前方互換性を持つかどうか判定します。
		/// </summary>
		/// <param name="other">比較対象のバージョン情報です。</param>
		/// <returns>前方互換性を持つ場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public virtual bool HasForwardCompatibleWith(VersionInfo other)
		{
			other.EnsureNotNull();
			var thisName  = this.Assembly?.GetName();
			var otherName = this.Assembly?.GetName();
			return  thisName?.Name        == otherName?.Name
				&&  thisName?.CultureName == otherName?.CultureName
				&& (thisName?.GetPublicKey()?.SequenceEqual(otherName?.GetPublicKey() ?? Array.Empty<byte>()) ?? false)
				&&  this     .Version     <= other     .Version;
		}

		/// <summary>
		///  指定されたバージョン情報が現在のバージョン情報に対し後方互換性を持つかどうか判定します。
		/// </summary>
		/// <param name="other">比較対象のバージョン情報です。</param>
		/// <returns>後方互換性を持つ場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public virtual bool HasBackwardCompatibleWith(VersionInfo other)
		{
			other.EnsureNotNull();
			var thisName  = this.Assembly?.GetName();
			var otherName = this.Assembly?.GetName();
			return  thisName?.Name        == otherName?.Name
				&&  thisName?.CultureName == otherName?.CultureName
				&& (thisName?.GetPublicKey()?.SequenceEqual(otherName?.GetPublicKey() ?? Array.Empty<byte>()) ?? false)
				&&  this     .Version     >= other     .Version;
		}

		/// <summary>
		///  アセンブリのバージョン情報を含む題名を取得します。
		/// </summary>
		/// <returns>題名を表す文字列です。</returns>
		public string GetCaption()
		{
			if (string.IsNullOrEmpty(this.Edition)) {
				return $"{this.DisplayName} [{this.GetFullVersionString()}]";
			} else {
				return $"{this.DisplayName} - {this.Edition} [{this.GetFullVersionString()}]";
			}
		}

		/// <summary>
		///  バージョン情報の長い形式を取得します。
		/// </summary>
		/// <returns>バージョン情報を表す文字列です。</returns>
		public string GetFullVersionString()
		{
			string  v  = this.GetVersionString();
			string  cn = this.CodeName;
			string? bc = this.Configuration;
			if (string.IsNullOrEmpty(bc) || bc == ConfigurationNames.Debug || bc == ConfigurationNames.Release) {
				return $"v{v}, cn:{cn}";
			} else {
				return $"v{v}, cn:{cn}, bc:{bc}";
			}
		}

		/// <summary>
		///  バージョン情報の文字列形式を取得します。
		/// </summary>
		/// <returns>バージョン情報を表す文字列です。</returns>
		public string GetVersionString()
		{
			return this.Version?.ToString(4) ?? "?.?.?.?";
		}

		/// <summary>
		///  アセンブリの作成者の一覧を含む配列を取得します。
		/// </summary>
		/// <remarks>
		///  配列は呼び出し毎に作成されます。
		/// </remarks>
		/// <returns>作成者の一覧を含む文字列配列です。</returns>
		public string[] GetAuthorArray()
		{
			return this.Authors.Split(_separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}

		/// <summary>
		///  デバッグ版の改版名を取得します。
		/// </summary>
		/// <returns>デバッグ版の改版名を表す文字列です。</returns>
		protected virtual string GetDebugEditionName()
		{
			return Resources.VersionInfo_Edition_Debug;
		}

		/// <summary>
		///  デバッグ版の改版名を作成します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.VersionInfo.Configuration"/>が<see cref="TakymLib.ConfigurationNames.Debug"/>である時にのみ作成されます。
		/// </remarks>
		/// <param name="edition">元の改版名を指定します。</param>
		/// <returns>新たに作成されたデバッグ版の改版名を表す文字列です。</returns>
		protected string? CreateDebugEditionName(string? edition)
		{
			if (this.Configuration == ConfigurationNames.Debug) {
				string den = this.GetDebugEditionName();
				if (string.IsNullOrEmpty(edition)) {
					return den;
				} else {
					return "(" + den + ")";
				}
			}
			return edition;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.GetFullVersionString();
		}
	}
}
