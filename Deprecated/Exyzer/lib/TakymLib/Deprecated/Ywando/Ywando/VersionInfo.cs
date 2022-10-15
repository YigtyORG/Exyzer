/****
 * Ywando
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Reflection;

namespace Ywando
{
	/// <summary>
	///  バージョン情報を表します。
	/// </summary>
	public class VersionInfo
	{
		/// <summary>
		///  アセンブリの名前を取得します。
		/// </summary>
		public virtual string? Name { get; }

		/// <summary>
		///  アセンブリの表示名を取得します。
		/// </summary>
		public virtual string? DisplayName { get; }

		/// <summary>
		///  アセンブリの説明を取得します。
		/// </summary>
		public virtual string? Description { get; }

		/// <summary>
		///  アセンブリの作成者を取得します。
		/// </summary>
		public virtual string[] Authors { get; }

		/// <summary>
		///  アセンブリの著作権表記を取得します。
		/// </summary>
		public virtual string? Copyright { get; }

		/// <summary>
		///  アセンブリのバージョン情報を取得します。
		/// </summary>
		public virtual Version? Version { get; }

		/// <summary>
		///  アセンブリの開発コード名を取得します。
		/// </summary>
		public virtual string? CodeName { get; }

		/// <summary>
		///  アセンブリのビルド構成を取得します。
		/// </summary>
		public virtual string? Configuration { get; }

		/// <summary>
		///  型'<see cref="Ywando.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="asm">バージョン情報の読み取り元のアセンブリです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public VersionInfo(Assembly asm)
		{
			asm.EnsureNotNull(nameof(asm));
			var asmname        = asm.GetName();
			this.Name          = asmname.Name;
			this.DisplayName   = asm.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
			this.Description   = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
			this.Copyright     = asm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
			this.Version       = asmname.Version;
			this.CodeName      = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
			this.Configuration = asm.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;

			if (asm.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company is not null and string authors) {
				this.Authors = authors.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			} else {
				this.Authors = Array.Empty<string>();
			}
		}

		/// <summary>
		///  型'<see cref="Ywando.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="metadata">バージョン情報の読み取り元のメタ情報です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public VersionInfo(YwandoMetadata metadata)
		{
			metadata.EnsureNotNull(nameof(metadata));
			this.Name          = metadata.Name;
			this.DisplayName   = metadata.Name;
			this.Authors       = metadata.Authors ?? Array.Empty<string>();
			this.Description   = metadata.Description;
			this.Copyright     = metadata.Copyright;
			this.Version       = metadata.Version;
			this.CodeName      = metadata.CodeName;
			this.Configuration = ConfigurationNames.Release;
		}

		/// <summary>
		///  型'<see cref="Ywando.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected VersionInfo()
		{
			this.Authors = Array.Empty<string>();
		}

		/// <summary>
		///  アセンブリの題名を取得します。
		/// </summary>
		/// <returns>題名を表す文字列です。</returns>
		public string GetCaption()
		{
			// v:  Version
			// cn: Code Name
			// bc: Build Configuration

			string v = $"v{this.Version?.ToString(4) ?? "?.?.?.?"}, cn:{this.CodeName ?? "Unknown"}";
			switch (this.Configuration) {
			case ConfigurationNames.Debug:
				return $"{this.DisplayName} - DEBUG [{v}]";
			case ConfigurationNames.Release:
			case null:
				return $"{this.DisplayName} [{v}]";
			default:
				return $"{this.DisplayName} [{v}, bc:{this.Configuration}]";
			}
		}
	}
}
