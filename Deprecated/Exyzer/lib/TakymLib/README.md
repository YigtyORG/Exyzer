# TakymLib
Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
Copyright (C) 2020-2022 Takym.

![Takym](LOGO.png)

[![Version](https://img.shields.io/badge/version-none-inactive)](https://github.com/YigtyORG/TakymLib/releases)
[![License](https://img.shields.io/github/license/YigtyORG/TakymLib)](LICENSE.md)
[![BuildTest](https://github.com/YigtyORG/TakymLib/workflows/BuildTest/badge.svg)](https://github.com/YigtyORG/TakymLib/actions/workflows/BuildTest.yml)
[![Benchmark](https://github.com/YigtyORG/TakymLib/workflows/Benchmark/badge.svg)](https://github.com/YigtyORG/TakymLib/actions/workflows/Benchmark.yml)
[![CodeQL](https://github.com/YigtyORG/TakymLib/workflows/CodeQL/badge.svg)](https://github.com/YigtyORG/TakymLib/actions/workflows/CodeQL.yml)
[![NuGet](https://github.com/YigtyORG/TakymLib/workflows/NuGet/badge.svg)](https://github.com/YigtyORG/TakymLib/actions/workflows/NuGet.yml)

[![GitHub Watchers](https://img.shields.io/github/watchers/YigtyORG/TakymLib?style=social)](https://github.com/YigtyORG/TakymLib/watchers)
[![GitHub Stars](https://img.shields.io/github/stars/YigtyORG/TakymLib?style=social)](https://github.com/YigtyORG/TakymLib/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/YigtyORG/TakymLib?style=social)](https://github.com/YigtyORG/TakymLib/network/members)

[日本語](#ja)
[English](#en)



# <a id="en" href="#en">English</a>

## Summary
The common libraries used by Yigty.ORG.
Compatible with **.NET 5.0**.
* **[TakymLib](./src/TakymLib)** - the common library.
* **[TakymLib.Aspect](./src/TakymLib.Aspect)** - the common aspect-oriented programming library.
* **[TakymLib.CommandLine](./src/TakymLib.CommandLine)** - the common command-line arguments parser library.
* **[TakymLib.Extensibility](./src/TakymLib.Extensibility)** - the common extensibility library.
* **[TakymLib.Logging](./src/TakymLib.Logging)** - the common logging library.
* **[TakymLib.Security](./src/TakymLib.Security)** - the common security library.
* **[TakymLib.Text](./src/TakymLib.Text)** - the common text library.
* **[TakymLib.Threading.Distributed](./src/TakymLib.Threading.Distributed)** - the common distributed computing library.
* **[TakymLib.Threading.Tasks](./src/TakymLib.Threading.Tasks)** - the common asynchronous programming library.
* **[TakymLib.UI](./src/TakymLib.UI)** - the common user interface library. (Windows only)
* **[TakymLib.ConsoleApp](./src/TakymLib.ConsoleApp)** - the test console application for validation. The compatibility of this is not preserved.
* **[TakymApp](./src/TakymApp)** - the desktop application of integrated tools (intols). (Windows only)

## Recommended Environment
* Operating Systems
	* **Windows 10 20H2** or later
	* **Ubuntu 20.04** or later (not tested yet)
* Runtime: **.NET 5.0** or later
* Language: **C# 9.0** or later

## Get Started
(draft...)

## History
Any version does not release yet. Please wait patiently.

| # |Version |Code Name |Date      |Changes           |Release Notes|
|--:|:------:|:---------|:--------:|:-----------------|:------------|
|  0|v0.0.0.0|intols00a0|0000/00/00|The first release.|             |

Please see [here](./CHANGELOG.md) for more information.

## Repositories
- [GitHub](https://github.com/YigtyORG/TakymLib) (Main)
- [GitHub](https://github.com/Takym/TakymLib) (Fork)
- [GitLab](https://gitlab.com/Takym/TakymLib) (Mirror)
- [Gitee](https://gitee.com/Takym/TakymLib) (Mirror)
- [Bitbucket](https://bitbucket.org/Takym/takymlib) (Mirror)

## How to Contribute
Please see [here](./CONTRIBUTING.md).

## Acknowledgments
* The list of third party libraries is [here](./docs/third-party.md) (Japanese).
* Special thanks to all [core collaborators and contributors](./CONTRIBUTORS.md) for this project.

## Terms of Use
This libraries are distributed under the [MIT License](LICENSE.md).


----------------------------------------------------------------


# <a id="ja" href="#ja">日本語</a>

## 概要
Yigty.ORG で利用される共通ライブラリ群です。
**.NET 5.0** に対応しています。
* **[TakymLib](./src/TakymLib)** - 共通ライブラリです。
* **[TakymLib.Aspect](./src/TakymLib.Aspect)** - 共通アスペクト指向(分断指向)プログラミングライブラリです。
* **[TakymLib.CommandLine](./src/TakymLib.CommandLine)** - 共通コマンド行引数解析ライブラリです。
* **[TakymLib.Extensibility](./src/TakymLib.Extensibility)** - 共通拡張性ライブラリです。
* **[TakymLib.Logging](./src/TakymLib.Logging)** - 共通ログ出力ライブラリです。
* **[TakymLib.Security](./src/TakymLib.Security)** - 共通安全保障ライブラリです。
* **[TakymLib.Text](./src/TakymLib.Text)** - 共通文字列ライブラリです。
* **[TakymLib.Threading.Distributed](./src/TakymLib.Threading.Distributed)** - 共通分散コンピューティング(分散処理)ライブラリです。
* **[TakymLib.Threading.Tasks](./src/TakymLib.Threading.Tasks)** - 共通非同期プログラミング(非同期処理)ライブラリです。
* **[TakymLib.UI](./src/TakymLib.UI)** - 共通画面(UI)ライブラリです。（Windows 専用）
* **[TakymLib.ConsoleApp](./src/TakymLib.ConsoleApp)** - 検証用の実験的コンソールアプリケーションです。互換性は保たれません。
* **[TakymApp](./src/TakymApp)** - 統合便利道具（Intols）デスクトップアプリケーションです。（Windows 専用）

## 推奨環境
* OS
	* **Windows 10 20H2**以降
	* **Ubuntu 20.04**以降(まだ動作確認していません)
* ランタイム：**.NET 5.0**以降
* 言語：**C# 9.0**以降

## 使い方
(書きかけ...)

## 更新履歴
まだリリースされていません。気長にお待ちください。

| # |バージョン|開発コード名|更新日    |更新内容            |リリースノート|
|--:|:--------:|:-----------|:--------:|:-------------------|:-------------|
|  0|v0.0.0.0  |intols00a0  |0000/00/00|最初のリリースです。|              |

詳細は[こちら](./CHANGELOG.md)をご覧ください。

## 保管庫
- [GitHub](https://github.com/YigtyORG/TakymLib) (メイン)
- [GitHub](https://github.com/Takym/TakymLib) (フォーク)
- [GitLab](https://gitlab.com/Takym/TakymLib) (ミラー)
- [Gitee](https://gitee.com/Takym/TakymLib) (ミラー)
- [Bitbucket](https://bitbucket.org/Takym/takymlib) (ミラー)

## 貢献方法
[こちら](./CONTRIBUTING.md)をご覧ください。

## 謝辞
* 利用している外部ライブラリの一覧は[こちら](./docs/third-party.md)にあります。
* この場を借りてお礼を申し上げます。全ての[協力者さんと貢献者さん](./CONTRIBUTORS.md)に感謝致します。

## 利用規約
このライブラリ群は[MITライセンス](LICENSE.md)に基づいて配布されています。
