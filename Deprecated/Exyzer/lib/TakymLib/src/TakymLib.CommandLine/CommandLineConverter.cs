/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using TakymLib.IO;

namespace TakymLib.CommandLine
{
	/// <summary>
	///  コマンド行引数を解析し、クラスまたは構造体へ変換します。
	/// </summary>
	public class CommandLineConverter : CommandLineParser
	{
		private readonly Dictionary<string, TypeEntry> _types;
		private readonly Dictionary<Type,   object>    _insts;
		private          IArgumentConverter?           _custom_converter;

		/// <summary>
		///  変換処理を非同期的に実行するかどうかを表す論理値を取得または設定します。
		/// </summary>
		/// <value>
		///  非同期的に実行する場合は<see langword="true"/>、同期的に実行する場合は<see langword="false"/>を設定します。
		///  既定値は<see langword="false"/>です。
		/// </value>
		public bool DoConvertAsync { get; set; }

		/// <summary>
		///  イベントハンドラを非同期的に実行するかどうかを表す論理値を取得または設定します。
		/// </summary>
		/// <value>
		///  非同期的に実行する場合は<see langword="true"/>、同期的に実行する場合は<see langword="false"/>を設定します。
		///  既定値は<see langword="false"/>です。
		/// </value>
		public bool RunEventsAsync { get; set; }

		/// <summary>
		///  解析前に呼び出されます。
		/// </summary>
		public event EventHandler<PreParseEventArgs>? PreParse;

		/// <summary>
		///  解析時に呼び出されます。
		/// </summary>
		public event EventHandler<ParseEventArgs>? ParseNext;

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineConverter"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="args">コマンド行引数を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public CommandLineConverter(string[] args) : base(args)
		{
			_types = new();
			_insts = new();
		}

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineConverter"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="text">コマンド行を表す文字列を指定します。</param>
		/// <param name="lexer">
		///  コマンド行を引数へ分解する字句解析機能を持つオブジェクトを指定します。
		///  この引数は省略できます。
		/// </param>
		/// <exception cref="System.ArgumentNullException"/>
		protected CommandLineConverter(string text, CommandLineLexer? lexer = null)
			: base(lexer, text)
		{
			_types = new();
			_insts = new();
		}

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineConverter"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="openReader">コマンド行を格納した<see cref="System.IO.TextReader"/>を生成する関数を指定します。</param>
		/// <param name="lexer">
		///  コマンド行を引数へ分解する字句解析機能を持つオブジェクトを指定します。
		///  この引数は省略できます。
		/// </param>
		/// <exception cref="System.ArgumentNullException"/>
		protected CommandLineConverter(Func<TextReader> openReader, CommandLineLexer? lexer = null)
			: base(lexer, openReader)
		{
			_types = new();
			_insts = new();
		}

		/// <summary>
		///  変換後の型を追加します。
		/// </summary>
		/// <typeparam name="T">追加する型の種類です。</typeparam>
		/// <returns>正しく追加された場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.NotSupportedException"/>
		/// <exception cref="System.Reflection.AmbiguousMatchException"/>
		/// <exception cref="System.TypeLoadException"/>
		/// <exception cref="System.AggregateException"/>
		public bool AddType<T>()
		{
			return this.AddType(typeof(T));
		}

		/// <summary>
		///  変換後の型を追加します。
		/// </summary>
		/// <param name="t">追加する型の種類です。</param>
		/// <returns>正しく追加された場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.NotSupportedException"/>
		/// <exception cref="System.Reflection.AmbiguousMatchException"/>
		/// <exception cref="System.TypeLoadException"/>
		/// <exception cref="System.AggregateException"/>
		public bool AddType(Type t)
		{
			t.EnsureNotNull();
			var s = t.GetCustomAttribute<SwitchAttribute>();
			if (s is null || s.Name is null || _types.ContainsKey(s.Name)) {
				return false;
			} else {
				var entry = new TypeEntry(this, t);
				_types.Add(s.Name, entry);
				if (entry._inst is not null) {
					_insts.Add(t, entry._inst);
				}
				return true;
			}
		}

		/// <summary>
		///  コマンド行引数のオプションの変換を行うオブジェクトを設定します。
		/// </summary>
		/// <param name="converter"><see cref="TakymLib.CommandLine.IArgumentConverter"/>オブジェクトです。</param>
		public void SetCustomConverter(IArgumentConverter? converter)
		{
			_custom_converter = converter;
		}

		/// <summary>
		///  指定された型のコマンド行引数の情報を保持するオブジェクトを取得します。
		/// </summary>
		/// <typeparam name="T">取得するオブジェクトの型です。</typeparam>
		/// <returns>有効なインスタンスまたは<see langword="null"/>を返します。</returns>
		public T? Get<T>()
		{
			if (this.Get(typeof(T)) is T inst) {
				return inst;
			} else {
				return default;
			}
		}

		/// <summary>
		///  指定された型のコマンド行引数の情報を保持するオブジェクトを取得します。
		/// </summary>
		/// <param name="t">取得するオブジェクトの型です。</param>
		/// <returns>有効なインスタンスまたは<see langword="null"/>を返します。</returns>
		public object? Get(Type t)
		{
			t.EnsureNotNull();
			if (_insts.ContainsKey(t)) {
				return _insts[t];
			} else {
				return null;
			}
		}

		/// <summary>
		///  コマンド行引数により生成された全てのオブジェクトを取得します。
		/// </summary>
		/// <returns>オブジェクトを格納している列挙体を返します。</returns>
		public IEnumerable<object> GetAll()
		{
			return _insts.Values;
		}

		/// <summary>
		///  コマンド行引数により生成された全てのオブジェクトを配列として取得します。
		/// </summary>
		/// <returns>オブジェクトを格納している配列のコピーを返します。</returns>
		public object[] GetAllAsArray()
		{
			return _insts.Values.ToArray();
		}

		/// <summary>
		///  解析前に呼び出されます。
		/// </summary>
		/// <param name="subCommand">子コマンドです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		protected sealed override async ValueTask OnPreParse(string? subCommand)
		{
			if (this.RunEventsAsync) {
				await Task.Run(() => this.OnPreParse(new PreParseEventArgs(subCommand)));
			} else {
				this.OnPreParse(new PreParseEventArgs(subCommand));
			}
		}

		/// <summary>
		///  <see cref="TakymLib.CommandLine.CommandLineConverter.PreParse"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		protected virtual void OnPreParse(PreParseEventArgs e)
		{
			this.PreParse?.Invoke(this, e);
		}

		/// <summary>
		///  解析時に呼び出されます。
		/// </summary>
		/// <param name="switchName">スイッチ名です。</param>
		/// <param name="optionName">オプション名です。</param>
		/// <param name="values">文字列配列です。</param>
		/// <returns>この処理の非同期操作です。</returns>
		protected sealed override async ValueTask OnParse(string? switchName, string? optionName, string[] values)
		{
			switchName ??= string.Empty;
			optionName ??= string.Empty;
			if (_types.ContainsKey(switchName)) {
				if (this.DoConvertAsync) {
					await Task.Run(() => _types[switchName].SetValue(optionName, values));
				} else {
					_types[switchName].SetValue(optionName, values);
				}
			}
			if (this.RunEventsAsync) {
				await Task.Run(() => this.OnParseNext(new ParseEventArgs(switchName, optionName, values)));
			} else {
				this.OnParseNext(new ParseEventArgs(switchName, optionName, values));
			}
		}

		/// <summary>
		///  <see cref="TakymLib.CommandLine.CommandLineConverter.ParseNext"/>イベントを発生させます。
		/// </summary>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		protected virtual void OnParseNext(ParseEventArgs e)
		{
			this.ParseNext?.Invoke(this, e);
		}

		private sealed class TypeEntry
		{
			private  static readonly Uri                                  _default_uri = new("http://localhost");
			private  static readonly Version                              _default_ver = new(0, 0, 0, 0);
			private         readonly CommandLineConverter                 _owner;
			internal        readonly Type                                 _type;
			internal        readonly object?                              _inst;
			private         readonly Dictionary<string, Action<string[]>> _opts;
			private                  bool                                 _init;

			internal TypeEntry(CommandLineConverter owner, Type t)
			{
				try {
					_owner = owner;
					_type  = t;
					_inst  = Activator.CreateInstance(t);
					_opts  = new Dictionary<string, Action<string[]>>();
					_init  = true;
				} catch (Exception e) {
					throw new AggregateException(e);
				}
			}

			internal void SetValue(string name, string[] args)
			{
				if (_init) {
					this.InitProperties();
					this.InitFields();
					_init = false;
				}
				if (_opts.ContainsKey(name)) {
					lock (this) {
						_opts[name](args);
					}
				}
			}

			private void InitProperties()
			{
				var props = _type.GetProperties();
				for (int i = 0; i < props.Length; ++i) {
					var o = props[i].GetCustomAttribute<OptionAttribute>();
					if (o is not null) {
						var prop   = props[i];
						var action = new Action<string[]>(args =>
							prop.SetValue(_inst, this.ConvertToObject(args, prop.PropertyType))
						);
						_opts.Add("-" + o.LongName, action);
						if (o.ShortName is not null) {
							_opts.Add(o.ShortName, action);
						}
					}
				}
			}

			private void InitFields()
			{
				var fields = _type.GetFields();
				for (int i = 0; i < fields.Length; ++i) {
					var o = fields[i].GetCustomAttribute<OptionAttribute>();
					if (o is not null) {
						var field  = fields[i];
						var action = new Action<string[]>(args =>
							field.SetValue(_inst, this.ConvertToObject(args, field.FieldType))
						);
						_opts.Add("-" + o.LongName, action);
						if (o.ShortName is not null) {
							_opts.Add(o.ShortName, action);
						}
					}
				}
			}

			private object? ConvertToObject(string[] args, Type target)
			{
				if (target == typeof(string) || target == typeof(object)) {
					return args.Length > 0 ? args[0] : string.Empty;
				} else if (target == typeof(PathString)) {
					try {
						return args.Length > 0 ? PathStringPool.Get(args[0]) : PathStringPool.Get();
					} catch {
						return PathStringPool.Get();
					}
				} else if (target == typeof(Uri)) {
					try {
						return args.Length > 0 ? new Uri(args[0]) : _default_uri;
					} catch {
						return _default_uri;
					}
				} else if (target == typeof(char)) {
					return (args.Length > 0 && args[0].Length > 0) ? args[0][0] : '\0';
				} else if (target == typeof(bool)) {
					//return args.Length == 0 ? true : args[0].TryToBoolean(out bool result) ? result : false;
					return args.Length == 0 || (args[0].TryToBoolean(out bool result) && result);
				} else if (target == typeof(byte)) {
					return args.Length == 0 ? default : byte.TryParse(args[0], out byte result) ? result : default;
				} else if (target == typeof(sbyte)) {
					return args.Length == 0 ? default : sbyte.TryParse(args[0], out sbyte result) ? result : default;
				} else if (target == typeof(ushort)) {
					return args.Length == 0 ? default : ushort.TryParse(args[0], out ushort result) ? result : default;
				} else if (target == typeof(short)) {
					return args.Length == 0 ? default : short.TryParse(args[0], out short result) ? result : default;
				} else if (target == typeof(uint)) {
					return args.Length == 0 ? default : uint.TryParse(args[0], out uint result) ? result : default;
				} else if (target == typeof(int)) {
					return args.Length == 0 ? default : int.TryParse(args[0], out int result) ? result : default;
				} else if (target == typeof(ulong)) {
					return args.Length == 0 ? default : ulong.TryParse(args[0], out ulong result) ? result : default;
				} else if (target == typeof(long)) {
					return args.Length == 0 ? default : long.TryParse(args[0], out long result) ? result : default;
				} else if (target == typeof(float)) {
					return args.Length == 0 ? default : float.TryParse(args[0], out float result) ? result : default;
				} else if (target == typeof(double)) {
					return args.Length == 0 ? default : double.TryParse(args[0], out double result) ? result : default;
				} else if (target == typeof(decimal)) {
					return args.Length == 0 ? default : decimal.TryParse(args[0], out decimal result) ? result : default;
				} else if (target == typeof(DateTime)) {
					return args.Length == 0 ? default : DateTime.TryParse(args[0], out var result) ? result : default;
				} else if (target == typeof(Guid)) {
					return args.Length == 0 ? default : Guid.TryParse(args[0], out var result) ? result : default;
				} else if (target == typeof(Version)) {
					return args.Length == 0 ? _default_ver : Version.TryParse(args[0], out var result) ? result : _default_ver;
				} else if (target == typeof(IPAddress)) {
					return args.Length == 0 ? IPAddress.Loopback : IPAddress.TryParse(args[0], out var result) ? result : IPAddress.Loopback;
				} else if (target == typeof(string[])) {
					return args;
				} else if (target == typeof(object[])) {
					object[] result = new object[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = args[i];
					}
					return result;
				} else if (target == typeof(PathString[])) {
					var result = new PathString[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						try {
							result[i] = PathStringPool.Get(args[i]);
						} catch {
							result[i] = PathStringPool.Get();
						}
					}
					return result;
				} else if (target == typeof(Uri[])) {
					var result = new Uri[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						try {
							result[i] = new Uri(args[i]);
						} catch {
							result[i] = _default_uri;
						}
					}
					return result;
				} else if (target == typeof(char[])) {
					char[] result = new char[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = args[i].Length > 0 ? args[i][0] : '\0';
					}
					return result;
				} else if (target == typeof(bool[])) {
					bool[] result = new bool[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						//result[i] = args[0].TryToBoolean(out bool r) ? r : false;
						result[i] = args[0].TryToBoolean(out bool r) && r;
					}
					return result;
				} else if (target == typeof(byte[])) {
					byte[] result = new byte[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = byte.TryParse(args[i], out byte r) ? r : default;
					}
					return result;
				} else if (target == typeof(sbyte[])) {
					sbyte[] result = new sbyte[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = sbyte.TryParse(args[i], out sbyte r) ? r : default;
					}
					return result;
				} else if (target == typeof(ushort[])) {
					ushort[] result = new ushort[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = ushort.TryParse(args[i], out ushort r) ? r : default;
					}
					return result;
				} else if (target == typeof(short[])) {
					short[] result = new short[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = short.TryParse(args[i], out short r) ? r : default;
					}
					return result;
				} else if (target == typeof(uint[])) {
					uint[] result = new uint[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = uint.TryParse(args[i], out uint r) ? r : default;
					}
					return result;
				} else if (target == typeof(int[])) {
					int[] result = new int[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = int.TryParse(args[i], out int r) ? r : default;
					}
					return result;
				} else if (target == typeof(ulong[])) {
					ulong[] result = new ulong[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = ulong.TryParse(args[i], out ulong r) ? r : default;
					}
					return result;
				} else if (target == typeof(long[])) {
					long[] result = new long[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = long.TryParse(args[i], out long r) ? r : default;
					}
					return result;
				} else if (target == typeof(float[])) {
					float[] result = new float[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = float.TryParse(args[i], out float r) ? r : default;
					}
					return result;
				} else if (target == typeof(double[])) {
					double[] result = new double[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = double.TryParse(args[i], out double r) ? r : default;
					}
					return result;
				} else if (target == typeof(decimal[])) {
					decimal[] result = new decimal[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = decimal.TryParse(args[i], out decimal r) ? r : default;
					}
					return result;
				} else if (target == typeof(DateTime[])) {
					var result = new DateTime[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = DateTime.TryParse(args[i], out var r) ? r : default;
					}
					return result;
				} else if (target == typeof(Guid[])) {
					var result = new Guid[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = Guid.TryParse(args[i], out var r) ? r : default;
					}
					return result;
				} else if (target == typeof(Version[])) {
					var result = new Version[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = Version.TryParse(args[i], out var r) ? r : _default_ver;
					}
					return result;
				} else if (target == typeof(IPAddress[])) {
					var result = new IPAddress[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						result[i] = IPAddress.TryParse(args[i], out var r) ? r : IPAddress.Loopback;
					}
					return result;
				} else if (typeof(IArgumentConvertible).IsAssignableFrom(target)) {
					try {
						var obj = Activator.CreateInstance(target) as IArgumentConvertible;
						obj?.FromStringArray(args);
						return obj;
					} catch {
						return null;
					}
				} else if (target.IsArray && typeof(IArgumentConvertible).IsAssignableFrom(target.GetElementType())) {
					var result = new IArgumentConvertible?[args.Length];
					for (int i = 0; i < args.Length; ++i) {
						try {
							result[i] = Activator.CreateInstance(target) as IArgumentConvertible;
							result[i]?.FromStringArray(new[] { args[i] });
						} catch {
							result[i] = null;
						}
					}
					return result;
				} else {
					return _owner._custom_converter?.ConvertToObject(args, target);
				}
			}
		}
	}
}
