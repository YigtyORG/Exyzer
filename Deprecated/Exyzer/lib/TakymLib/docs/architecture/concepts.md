# TakymLib 基本概念
Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
Copyright (C) 2020-2022 Takym.

<a id="notes"></a>
## 注意事項
* このページでは特に明記されていなければ、コード例の先頭には `using TakymLib;` が記述されているものとします。

<a id="argument-helper"></a>
## ArgumentHelper について
<xref:TakymLib.ArgumentHelper> は引数の検証に便利な拡張関数を提供します。
例えば、渡された引数が `null` ではない事を検証するには以下の様に記述します。
```csharp
public void SomeFunc(object arg)
{
	arg.EnsureNotNull();
	...
}
```
`null` が渡された場合は <xref:System.ArgumentNullException> を発生させます。
TakymLib では殆どの引数の検証に <xref:TakymLib.ArgumentHelper> を利用しています。
<xref:System.Runtime.CompilerServices.CallerArgumentExpressionAttribute> が完全に実装されたら引数名も省略できる様にする予定です。

<a id="disposable-base"></a>
## 破棄可能なクラスの雛形
抽象クラス「<xref:TakymLib.DisposableBase>」を継承すれば <xref:System.IDisposable> と <xref:System.IAsyncDisposable> の実装を簡略化する事ができます。
```csharp
public class SomeClass : DisposableBase
{
	public void SomeFunc()
	{
		this.EnsureNotDisposed(); // 破棄されていないか確認します。
	}

	protected override void Dispose(bool disposing)
	{
		if (this.IsDisposed) {
			return;
		}
		if (disposing) {
			// TODO: ここでマネージドリソースを破棄します。
		}
		// TODO: ここでアンマネージリソースを破棄します。
		base.Dispose(disposing);
	}

	protected override async ValueTask DisposeAsyncCore()
	{
		// TODO: ここでマネージドリソースを非同期的に破棄します。
		await base.DisposeAsyncCore();
	}
}
```
`SomeFunc` の先頭に記述されている `this.EnsureNotDisposed();` はオブジェクトが破棄されている場合に <xref:System.ObjectDisposedException> を発生させます。
`SomeClass` は以下の様にして外部から利用します。
```csharp
using (var obj = new SomeClass()) {
	obj.SomeFunc();
}
```
TakymLib では、殆どの破棄可能なオブジェクトは <xref:TakymLib.DisposableBase> を継承しています。

<a id="version-info"></a>
## アセンブリのバージョン情報
TakymLib にはアセンブリの詳細なバージョン情報を取得する為のクラスが用意されています。
以下の様にして実行中のアセンブリのバージョン情報をコンソール画面に出力します。
```csharp
VersionInfo.Current.Print();
```
また、読み込まれている全てのアセンブリのバージョン情報を表示する事もできます。
```csharp
VersionInfo.PrintAllAssemblies();
```
その他のプロパティは <xref:TakymLib.VersionInfo> を参照してください。
今後は互換性の検証を行える様にする予定です。
