# TakymLib コーディング規則
Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
Copyright (C) 2020-2022 Takym.

[こちら](https://github.com/YigtyORG/TakymLib/blob/master/.editorconfig)も参照してください。

<a id="class"></a>
## Classes/Structs/Interfaces/Delegates/Enums (クラス/構造体/インターフェース/デリゲート/列挙体)
* SomeClass.cs
```csharp
namespace SomeNamespace
{
	public class SomeClass
	{
		// 1. Private Fields (内部変数)

		// 2. Properties (プロパティ)

		// 3. Events (イベント)

		// 4. Constructors (コンストラクタ)

		// 5. Destructors (デストラクタ)

		// 6. Methods/Functions (関数)

		// 7. Operators/Casts (演算子/変換演算子)

		// 8. Internal classes (内部クラス)
	}
}
```
* SomeStruct.cs
```csharp
namespace SomeNamespace
{
	public struct SomeStruct
	{
		// 1. Private Fields (内部変数)

		// 2. Properties (プロパティ)

		// 3. Events (イベント)

		// 4. Constructors (コンストラクタ)

		// 5. Destructors (デストラクタ)

		// 6. Methods/Functions (関数)

		// 7. Operators/Casts (演算子/変換演算子)

		// 8. Internal classes (内部クラス)
	}
}
```
* ISomeInterface.cs
```csharp
namespace SomeNamespace
{
	public interface ISomeInterface
	{
		public string Name     { get; set; }
		public void   Do       ();
		public string GetString();
		public void   SetString(string   s);
		public object SomeFunc1(DateTime dt,   object  o);
		public object SomeFunc2(Guid     guid, Version ver);
	}
}
```
* SomeDelegate.cs
```csharp
namespace SomeNamespace
{
	public delegate void Action(object arg);
}
```
* SomeEnum.cs
```csharp
namespace SomeNamespace
{
	public enum SomeEnum
	{
		Invalid,
		Red,
		Green,
		Blue
	}

	[Flags()]
	public enum SomeFlagsEnum
	{
		None,
		CanRead,
		CanWrite,
		CanSeek,
		CanFlush,
		FullControl = CanRead | CanWrite | CanSeek | CanFlush
	}
}
```
* SomeEnum.cs (with values)
```csharp
namespace SomeNamespace
{
	public enum SomeEnum
	{
		Invalid = 0,
		Red     = 1,
		Green   = 2,
		Blue    = 3
	}

	[Flags()]
	public enum SomeFlagsEnum
	{
		None        = 0b0000,
		CanRead     = 0b0001,
		CanWrite    = 0b0010,
		CanSeek     = 0b0100,
		CanFlush    = 0b1000,
		FullControl = CanRead | CanWrite | CanSeek | CanFlush
	}
}
```

<a id="field"></a>
## Private Fields (内部変数)
```csharp
namespace SomeNamespace
{
	public class SomeClass
	{
		private  static readonly string         _global_name;
		internal static          string         _text_data;
		private         readonly string         _name;
		internal        readonly string         _shared_name;
		private         readonly AppDomain      _app_domain;
		private                  StringBuilder? _sb;

		// ...
	}
}
```

<a id="property"></a>
## Properties (プロパティ)
```csharp
namespace SomeNamespace
{
	public class SomeClass
	{
		// ...

		public string    Name      { get; }
		public string?   Text      { get; set; }
		public AppDomain AppDomain { get; }

		// ...
	}
}
```

<a id="event"></a>
## Events (イベント)
```csharp
namespace SomeNamespace
{
	public class SomeClass
	{
		// ...

		public event EventHandler Load;
		public event EventHandler Save;

		// ...
	}
}
```

<a id="field-property-event"></a>
## Private Fields, Properties, and Events (内部変数、プロパティ、及びイベント)
```csharp
namespace SomeNamespace
{
	public class SomeClass
	{
		private   static readonly string         _global_name;
		internal  static          string         _text_data;
		private          readonly string         _name;
		internal         readonly string         _shared_name;
		private          readonly AppDomain      _app_domain;
		private                   StringBuilder? _sb;
		public                    string         Name      => _name;
		public                    string?        Text      { get => this.TextCore; set => this.TextCore = value; }
		protected                 string?        TextCore  { get; set; }
		public                    AppDomain      AppDomain => _app_domain;
		public    static event    EventHandler   Initialize;
		public           event    EventHandler   Load;
		public           event    EventHandler   Save;

		// ...
	}
}
```

<a id="ctor"></a>
## Constructors and Destructors (コンストラクタとデストラクタ)
* SomeClass.cs
```csharp
namespace SomeNamespace
{
	public class SomeClass
	{
		// ...

		static SomeClass()
		{
			// ...
		}

		// ...

		public SomeClass() : this(null, null, null) { }

		public SomeClass(object? arg1, object? arg2, object? arg3)
		{
			this.Item1 = arg1;
			this.Item2 = arg2;
			this.Item3 = arg3;

			// Or

			_item1 = arg1;
			_item2 = arg2;
			_item3 = arg3;
		}

		// ...

		~SomeClass()
		{
			// ...
		}

		// ...
	}
}
```

<a id="method"></a>
## Methods/Functions and Operators/Casts (関数と演算子/変換演算子)
* SomeClass.cs
```csharp
namespace SomeNamespace
{
	public class SomeClass : SomeBaseClass
	{
		// ...

		public string GetString()
		{
			// ...
		}

		protected override void OnAct(EventArgs e)
		{
			base.OnAct(e);
			// ...
		}

		public async ValueTask SomeAsyncMethod()
		{
			// ...
		}

		public override async Task<object> GetObjectAsync()
		{
			return await base.GetObjectAsync();
		}

		private static string GetText(int? mode)
		{
			if (mode.HasValue) {
				return mode.Value switch {
					0 => "Hello, World!!",
					1 => "goodbye",
					_ => string.Empty
				}
			} else {
				return string.Empty;
			}

			// Or

			if (mode is null) {
				return string.Empty;
			}
			switch (mode) {
			case 0:  return "Hello, World!!";
			case 1:  return "goodbye";
			default: return string.Empty;
			}
		}

		private static IEnumerable<string> Filter(IEnumerable<string?> texts)
		{
			foreach (var item in texts) {
				if (!string.IsNullOrEmpty(item)) {
					yield return item;
				}
			}

			// Or

			using var enumerator = texts.GetEnumerator();
			while (enumerator.MoveNext()) {
				var item = enumerator.Current;
				if (!string.IsNullOrEmpty(item)) {
					yield return item;
				}
			}
		}

		private static void DoWork(params object[] values)
		{
			for (int i = 0; i < values.Length; ++i) {
				var val = values[i];
				// ...
			}
		}

		private static void DoWork(ILisy<object> values)
		{
			int count = values.Count;
			for (int i = 0; i < count; ++i) {
				var val = values[i];
				// ...
			}
		}

		private static string LoadFile(string filename)
		{
			using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var sr = new StreamReader(fs, Encoding.UTF8, true)) {
				return sr.ReadToEnd();
			}
		}

		private static ValueTask SaveFileAsync(string filename, IAsyncEnumerable<string> data)
		{
			var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			await using (fs.ConfigureAwait(false)) {
				var sw = new StreamWriter(fs, Encoding.UTF8);
				await using (sw.ConfigureAwait(false)) {
					await foreach (var line in data) {
						await sw.WriteLineAsync(line);
					}
				}
			}
		}

		public static SomeClass operator +(SomeClass left, SomeClass right)
		{
			// ...
		}

		public static implicit operator SomeClass(SomeOtherClass obj)
		{
			// ...
		}

		public static explicit operator SomeOtherClass(SomeClass obj)
		{
			// ...
		}

		// ...
	}
}
```

<a id="entrypoint"></a>
## Entry points (開始関数)
* Program.cs
```csharp
using System;

namespace SomeNamespace
{
	internal static class Program
	{
		// ...

		[STAThread()]
		private static int Main(string[] args)
		{
			try {
				// ...
				return 0;
			} catch (Exception e) {
				// ...
				return e.HResult;
			}
		}

		// ...
	}
}
```

<a id="generic"></a>
## Generics (ジェネリック/総称型)
```csharp

public class SomeClassA<T> { /* ... */ }

public class SomeClassB<T, U>
	where T: U
	where U: new()
{
	/* ... */
}

public interface ISomeInterface<in TIn, out TOut1, out TOut2>
{
	/* ... */
}

public delegate TResult SomeDelegate<in TArg1, in TArg2, out TResult>(TArg1 arg1, TArg2 arg2);

public T Get<T>() where T: IClonable
{
	/* ... */
}

public abstract TResult SomeMethod<TArg, TResult>(TArg arg)
	where TResult: TArg, class, new()
	where TArg   : IComparable, IFormattable;

```
