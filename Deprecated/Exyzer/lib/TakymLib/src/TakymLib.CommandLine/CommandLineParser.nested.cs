/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace TakymLib.CommandLine
{
	partial class CommandLineParser
	{
		private abstract class ParseResult
		{
			internal abstract string?      Command  { get; set; }
			internal abstract List<string> Values   { get; }
			internal abstract List<Option> Options  { get; }
			internal abstract List<Switch> Switches { get; }

			internal abstract void AddValue (string value);
			internal abstract void AddSwitch(string name);
			internal abstract void AddOption(string name);
			internal abstract void Combine  (ParseResult parseResult);
		}

		private sealed class EmptyParseResult : ParseResult
		{
			internal static readonly EmptyParseResult _inst = new();

			internal override string? Command
			{
				get => null;
				set { }
			}

			internal override List<string> Values   => EmptyListCache<string>.Get();
			internal override List<Option> Options  => EmptyListCache<Option>.Get();
			internal override List<Switch> Switches => EmptyListCache<Switch>.Get();

			private EmptyParseResult() { }

			internal override void AddOption(string name)  { }
			internal override void AddSwitch(string name)  { }
			internal override void AddValue (string value) { }
			internal override void Combine  (ParseResult parseResult) { }

			private static class EmptyListCache<T>
			{
				[ThreadStatic()]
				private static List<T>? _cache;

				internal static List<T> Get()
				{
					if (_cache is null || _cache.Count != 0) {
						_cache = new(0);
					}
					return _cache;
				}
			}
		}

		private sealed class ConcreteParseResult : ParseResult
		{
			private List<string> _current_vs;
			private List<Option> _current_os;

			internal override string?      Command  { get; set; }
			internal override List<string> Values   { get; }
			internal override List<Option> Options  { get; }
			internal override List<Switch> Switches { get; }

			internal ConcreteParseResult()
			{
				this.Values   = new List<string>();
				this.Options  = new List<Option>();
				this.Switches = new List<Switch>();
				_current_vs   = this.Values;
				_current_os   = this.Options;
			}

			internal override void AddValue(string value)
			{
				_current_vs.Add(value);
			}

			internal override void AddSwitch(string name)
			{
				var s = new Switch(name);
				_current_vs = s.Values;
				_current_os = s.Options;
				this.Switches.Add(s);
			}

			internal override void AddOption(string name)
			{
				var o = new Option(name);
				_current_vs = o.Values;
				_current_os.Add(o);
			}

			internal override void Combine(ParseResult parseResult)
			{
				this.Values  .AddRange(parseResult.Values);
				this.Options .AddRange(parseResult.Options);
				this.Switches.AddRange(parseResult.Switches);
			}
		}

		private sealed class Switch
		{
			internal string       Name    { get; }
			internal List<string> Values  { get; }
			internal List<Option> Options { get; }

			internal Switch(string name)
			{
				this.Name    = name;
				this.Values  = new List<string>();
				this.Options = new List<Option>();
			}
		}

		private sealed class Option
		{
			internal string       Name   { get; }
			internal List<string> Values { get; }

			internal Option(string name)
			{
				this.Name   = name;
				this.Values = new List<string>();
			}
		}

		private struct ArrayAsyncEnumerator : IAsyncEnumerable<string>, IAsyncEnumerator<string>
		{
			private readonly int               _mtid;
			private readonly string[]          _array;
			private          int               _index;
			private          CancellationToken _ct;

			public string Current => _array[_index];

			internal ArrayAsyncEnumerator(string[] array)
			{
				_mtid  = Environment.CurrentManagedThreadId;
				_array = array;
				_index = -1;
				_ct    = default;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				if (_ct.IsCancellationRequested) {
					return ValueTask.FromCanceled<bool>(_ct);
				}
				++_index;
				return ValueTask.FromResult(_index < _array.Length);
			}

			public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
			{
				if (Environment.CurrentManagedThreadId == _mtid) {
					if (_ct == default) {
						_ct = cancellationToken;
					} else {
						_ct = CancellationTokenSource.CreateLinkedTokenSource(_ct, cancellationToken).Token;
					}
					return this;
				} else {
					throw new InvalidOperationException();
				}
			}

			public ValueTask DisposeAsync()
			{
				return default;
			}
		}
	}
}
