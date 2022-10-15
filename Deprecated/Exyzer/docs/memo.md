# Exyzer 開発メモ
Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
Copyright (C) 2020-2022 Takym.

## バージョンの記法

```
XYZaabbb
```

* aa  - メジャー番号
* bbb - マイナー番号

### 16進法表現
* 先頭の `XYZ` を `ABC` にした場合、最新である事を表す。
* 先頭の `XYZ` を `DEF` にした場合、古いバージョンを表す。

## プロジェクト・名前空間の構造
* `Exyzer` (名前空間)
	* `Exyzer.Core` (プロジェクト)
	* `Exyzer.Engines` (名前空間)
		* `Exyzer.Engines.All` (プロジェクト)
		* `Exyzer.Engines.ABC` (プロジェクト、名前空間)
		* `Exyzer.Engines.DEFs` (名前空間)
			* `Exyzer.Engines.DEFs.XYZ00000` (プロジェクト、名前空間)
	* `Exyzer.Apps.Shell` (プロジェクト、名前空間)
	* `Exyzer.Apps.Web` (プロジェクト、名前空間)
