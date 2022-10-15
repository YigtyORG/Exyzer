/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TakymLib.Properties;

namespace TakymLib.Text
{
	/// <summary>
	///  識別子として利用できる名前を表します。
	///  このクラスは読み取り専用構造体です。
	/// </summary>
	[Serializable()]
	public readonly struct NameString : ISerializable, IEquatable<NameString>, IComparable, IComparable<NameString>, IReadOnlyList<char>
	{
		private readonly byte[]? _buf;

		/// <summary>
		///  この名前文字列の文字列長を取得します。
		/// </summary>
		public int Count => _buf?.Length ?? 0;

		/// <summary>
		///  指定された位置にある文字を取得します。
		/// </summary>
		/// <param name="index">インデックス番号を指定します。</param>
		/// <returns>指定された位置にある文字を指定します。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public char this[int index] => DecodeCore((_buf ?? Array.Empty<byte>())[index]);

		/// <summary>
		///  指定された位置にある文字を取得します。
		/// </summary>
		/// <param name="index">インデックス番号を指定します。</param>
		/// <returns>指定された位置にある文字を指定します。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public char this[Index index] => DecodeCore((_buf ?? Array.Empty<byte>())[index]);

		/// <summary>
		///  指定された範囲の部分文字列を取得します。
		/// </summary>
		/// <param name="range">取得する部分文字列の範囲を指定します。</param>
		/// <returns>指定された範囲の部分文字列を指定します。</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.ArgumentOutOfRangeException"/>
		public NameString this[Range range] => new((_buf ?? Array.Empty<byte>())[range]);

		/// <summary>
		///  型'<see cref="TakymLib.Text.NameString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="s">保持する文字列を指定します。</param>
		/// <exception cref="System.ArgumentException"/>
		public NameString(string s)
		{
			s ??= string.Empty;
			_buf = new byte[s.Length];
			try {
				for (int i = 0; i < s.Length; ++i) {
					_buf[i] = Encode(s[i]);
				}
			} catch (ArgumentException e) {
				throw new ArgumentException(Resources.NameString_Constructor_ArgumentException, nameof(s), e);
			}
		}

		/// <summary>
		///  型'<see cref="TakymLib.Text.NameString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="chars">保持する文字列を指定します。</param>
		/// <exception cref="System.ArgumentException"/>
		public NameString(params char[] chars)
		{
			chars ??= Array.Empty<char>();
			_buf = new byte[chars.Length];
			try {
				for (int i = 0; i < chars.Length; ++i) {
					_buf[i] = Encode(chars[i]);
				}
			} catch (ArgumentException e) {
				throw new ArgumentException(Resources.NameString_Constructor_ArgumentException, nameof(chars), e);
			}
		}

		/// <summary>
		///  型'<see cref="TakymLib.Text.NameString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="chars">保持する文字列を指定します。</param>
		/// <exception cref="System.ArgumentException"/>
		public NameString(ReadOnlySpan<char> chars)
		{
			_buf = new byte[chars.Length];
			try {
				for (int i = 0; i < chars.Length; ++i) {
					_buf[i] = Encode(chars[i]);
				}
			} catch (ArgumentException e) {
				throw new ArgumentException(Resources.NameString_Constructor_ArgumentException, nameof(chars), e);
			}
		}

		/// <summary>
		///  型'<see cref="TakymLib.Text.NameString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="chars">保持する文字列を指定します。</param>
		/// <exception cref="System.ArgumentException"/>
		public NameString(ReadOnlyMemory<char> chars) : this(chars.Span) { }

		/// <summary>
		///  型'<see cref="TakymLib.Text.NameString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="chars">保持する文字列を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.InvalidOperationException"/>
		public NameString(IEnumerable<char> chars)
		{
			chars.EnsureNotNull();
			var buf = new List<byte>();
			try {
				foreach (char c in chars) {
					buf.Add(Encode(c));
				}
			} catch (ArgumentException e) {
				throw new ArgumentException(Resources.NameString_Constructor_ArgumentException, nameof(chars), e);
			}
			_buf = buf.ToArray();
		}

		private NameString(byte[]? buf)
		{
			_buf = buf;
		}

		private NameString(SerializationInfo info, StreamingContext context)
		{
			_buf = info.GetValue<byte[]>("_") is not null and var data ? Decompress(data) : null;
		}

		/// <summary>
		///  現在の名前文字列を直列化します。
		/// </summary>
		/// <param name="info">直列化されたデータを含むオブジェクトを指定します。</param>
		/// <param name="context">ストリームの転送先または転送元に関する文脈情報を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Runtime.Serialization.SerializationException"/>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.EnsureNotNull();
			if (_buf is not null) {
				info.AddValue("_", Compress(_buf));
			}
		}

		/// <summary>
		///  指定された名前文字列を結合し、新しい名前文字列を生成します。
		/// </summary>
		/// <param name="ns">結合する名前文字列を指定します。</param>
		/// <returns>現在の名前文字列に<paramref name="ns"/>が連結された名前文字列を返します。</returns>
		public NameString Concat(NameString ns)
		{
			if (_buf is null) {
				return ns;
			} else if (ns._buf is null) {
				return this;
			} else {
				return new(_buf.Combine(ns._buf));
			}
		}

		/// <summary>
		///  指定された名前文字列を結合し、新しい名前文字列を生成します。
		/// </summary>
		/// <remarks>
		///  この関数は<see cref="TakymLib.Text.NameString.Concat(NameString)"/>の別名です。
		/// </remarks>
		/// <param name="ns">結合する名前文字列を指定します。</param>
		/// <returns>現在の名前文字列に<paramref name="ns"/>が連結された名前文字列を返します。</returns>
		public NameString Append(NameString ns)
		{
			return this.Concat(ns);
		}

		/// <summary>
		///  現在の名前文字列から<see langword="NULL"/>文字を削除します。
		/// </summary>
		/// <returns><see langword="NULL"/>文字が削除された新しい名前文字列を返します。</returns>
		public NameString Compact()
		{
			if (_buf is null) {
				return this;
			} else {
				var result = new List<byte>(_buf.Length);
				for (int i = 0; i < _buf.Length; ++i) {
					byte b = _buf[i];
					if (b != 0) {
						result.Add(b);
					}
				}
				return new(result.ToArray());
			}
		}

		/// <summary>
		///  現在の名前文字列から部分文字列を取得します。
		/// </summary>
		/// <param name="start">開始位置を指定します。</param>
		/// <param name="length">文字列長を指定します。</param>
		/// <returns>新しい名前文字列を返します。</returns>
		public NameString Substring(int start, int length)
		{
			return this[start..(start + length)];
		}

		/// <summary>
		///  指定したオブジェクトインスタンスの値と現在のインスタンスの値が等価かどうか判定します。
		/// </summary>
		/// <param name="obj">判定対象のオブジェクトを指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public override bool Equals(object? obj)
		{
			return obj is NameString other && this.Equals(other);
		}

		/// <summary>
		///  指定した名前文字列と現在の名前文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="other">判定対象の名前文字列を指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public bool Equals(NameString other)
		{
			if (_buf == other._buf) {
				return true;
			} else if (_buf is null || other._buf is null) {
				return false;
			} else if (_buf.Length == other._buf.Length) {
				for (int i = 0; i < _buf.Length; ++i) {
					if (_buf[i] != other._buf[i]) {
						return false;
					}
				}
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		///  指定したオブジェクトインスタンスの値と現在のインスタンスの値を比較します。
		/// </summary>
		/// <param name="obj">比較対象のオブジェクトを指定します。</param>
		/// <returns>
		///  等価の場合は<code>0</code>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(object? obj)
		{
			return obj is NameString other ? this.CompareTo(other) : 1;
		}

		/// <summary>
		///  指定した名前文字列と現在の名前文字列を比較します。
		/// </summary>
		/// <param name="other">比較対象の名前文字列を指定します。</param>
		/// <returns>
		///  等価の場合は<code>0</code>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(NameString other)
		{
			if (_buf == other._buf) {
				return 0;
			} else if (_buf is null) {
				return -1;
			} else if (other._buf is null) {
				return 1;
			} else {
				int len = Math.Min(_buf.Length, other._buf.Length);
				for (int i = 0; i < len; ++i) {
					int r = _buf[i].CompareTo(other._buf[i]);
					if (r != 0) {
						return r;
					}
				}
				return _buf.Length.CompareTo(other._buf.Length);
			}
		}

		/// <summary>
		///  現在の名前文字列のハッシュ値を計算します。
		/// </summary>
		/// <returns>ハッシュ値を返します。</returns>
		public override int GetHashCode()
		{
			if (_buf is null) {
				return 0;
			} else {
				int result = 0;
				for (int i = 0; i < _buf.Length; ++i) {
					result ^= _buf[i].GetHashCode();
				}
				return result;
			}
		}

		/// <summary>
		///  現在の名前文字列と等価の文字列を返します。
		/// </summary>
		/// <returns>型'<see cref="string"/>'の値を返します。</returns>
		/// <exception cref="System.ArgumentException"/>
		public override string ToString()
		{
			if (_buf is null) {
				return string.Empty;
			} else {
				char[] result = new char[_buf.Length];
				for (int i = 0; i < _buf.Length; ++i) {
					result[i] = DecodeCore(_buf[i]);
				}
				return new string(result);
			}
		}

		/// <summary>
		///  現在の名前文字列をバイト配列に変換します。
		/// </summary>
		/// <returns>型'<see cref="byte"/>'の配列を返します。</returns>
		public byte[] ToBinary()
		{
			return Compress(_buf);
		}

		/// <summary>
		///  指定されたバイト配列を名前文字列に変換します。
		/// </summary>
		/// <param name="buf">変換するバイト配列を指定します。</param>
		/// <returns>新しい名前文字列を返します。</returns>
		public static NameString FromBinary(byte[] buf)
		{
			return new NameString(Decompress(buf));
		}

		/// <summary>
		///  指定された読み取り専用スパンを名前文字列に変換します。
		/// </summary>
		/// <param name="buf">変換する読み取り専用スパンを指定します。</param>
		/// <returns>新しい名前文字列を返します。</returns>
		public static NameString FromBinary(ReadOnlySpan<byte> buf)
		{
			return new NameString(Decompress(buf));
		}

		/// <summary>
		///  この名前文字列の文字を列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IEnumerator{T}"/>オブジェクトを返します。</returns>
		public IEnumerator<char> GetEnumerator()
		{
			if (_buf is not null) {
				for (int i = 0; i < _buf.Length; ++i) {
					yield return DecodeCore(_buf[i]);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		///  指定された文字を符号化します。
		/// </summary>
		/// <param name="c">符号化する文字を指定します。</param>
		/// <returns><paramref name="c"/>を表現する符号無し8ビット整数値を返します。</returns>
		/// <exception cref="System.ArgumentException"/>
		public static byte Encode(char c)
		{
			return c switch {
				'\0' => 0,
				'_'  => 1,
				(>= 'A' and <= 'Z') => ((byte)(c - 'A' +  2)),
				(>= 'a' and <= 'z') => ((byte)(c - 'a' + 28)),
				(>= '0' and <= '9') => ((byte)(c - '0' + 54)),
				_ => throw new ArgumentException(string.Format(Resources.NameString_Encode_ArgumentException, c, ((ushort)(c))), nameof(c))
			};
		}

		/// <summary>
		///  指定された符号無し8ビット整数値を逆符号化します。
		/// </summary>
		/// <param name="b">逆符号化する符号無し8ビット整数値を指定します。</param>
		/// <returns><paramref name="b"/>が表現している文字を返します。</returns>
		/// <exception cref="System.ArgumentException"/>
		public static char Decode(byte b)
		{
			try {
				b.EnsureWithinClosedRange(0, 0b0011_1111, nameof(b));
			} catch (ArgumentOutOfRangeException e) {
				throw new ArgumentException(
					string.Format(Resources.NameString_Decode_ArgumentException, b),
					nameof(b), e
				);
			}
			return DecodeCore(b);
		}

		private static char DecodeCore(byte b)
		{
			return b switch {
				0 => '\0',
				1 => '_',
				(>=  2 and <= 27) => ((char)(b + 'A' -  2)),
				(>= 28 and <= 53) => ((char)(b + 'a' - 28)),
				(>= 54 and <= 63) => ((char)(b + '0' - 54)),
				_ => throw new ArgumentException(string.Format(Resources.NameString_Decode_ArgumentException, b), nameof(b))
			};
		}

		private static byte[] Compress(ReadOnlySpan<byte> buf)
		{
			// 0         1         2         3         4         5         6         7         8
			// 000000|00-0000|0000-00|000000 000000|00-0000|0000-00|000000 000000|00-0000|0000-00|000000
			// 0      1       2       3      4      5       6       7      8      9       10      11
			int i = buf.Length * 3, j;
			Span<byte> data = stackalloc byte[(i >> 2) + ((i & 0b11) == 0 ? 0 : 1)];
			for (i = 0, j = 0; i < buf.Length; ++i) {
				switch (i & 0b11) {
				case 0:
					data[j] = ((byte)(buf[i] << 2));
					break;
				case 1:
					data[j] |= ((byte)((buf[i] & 0b00_110000) >> 4));
					++j;
					data[j] = ((byte)((buf[i] & 0b00_001111) << 4));
					break;
				case 2:
					data[j] |= ((byte)((buf[i] & 0b00_111100) >> 2));
					++j;
					data[j] = ((byte)((buf[i] & 0b00_000011) << 6));
					break;
				case 3:
					data[j] |= buf[i];
					++j;
					break;
				}
			}
			return data.ToArray();
		}

		private static byte[] Decompress(ReadOnlySpan<byte> data)
		{
			// 0         1         2         3         4         5         6         7         8
			// 000000|00-0000|0000-00|000000 000000|00-0000|0000-00|000000 000000|00-0000|0000-00|000000
			// 0      1       2       3      4      5       6       7      8      9       10      11
			Span<byte> buf = stackalloc byte[((data.Length / 3) << 2) + (data.Length % 3)];
			for (int i = 0, j = 0; i < buf.Length; ++i) {
				switch (i & 0b11) {
				case 0:
					buf[i] = ((byte)(data[j] >> 2));
					break;
				case 1:
					buf[i] = ((byte)((data[j] & 0b00000011) << 4));
					++j;
					buf[i] |= ((byte)((data[j] & 0b11110000) >> 4));
					break;
				case 2:
					buf[i] = ((byte)((data[j] & 0b00001111) << 2));
					++j;
					buf[i] |= ((byte)((data[j] & 0b11000000) >> 6));
					break;
				case 3:
					buf[i] = ((byte)(data[j] & 0b00111111));
					++j;
					break;
				}
			}
			return (buf[^1] == 0 ? buf[0..^1] : buf).ToArray();
		}

		/// <summary>
		///  指定された二つの名前文字列を結合します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>結合された名前文字列を返します。</returns>
		public static NameString operator +(NameString left, NameString right)
			=> left.Concat(right);

		/// <summary>
		///  指定された二つの名前文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public static bool operator ==(NameString left, NameString right)
			=> left.Equals(right);

		/// <summary>
		///  指定された二つの名前文字列が不等価かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>等しい場合は<see langword="false"/>、等しくない場合は<see langword="true"/>を返します。</returns>
		public static bool operator !=(NameString left, NameString right)
			=> !left.Equals(right);

		/// <summary>
		///  左辺が右辺未満かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より小さい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator <(NameString left, NameString right)
			=> left.CompareTo(right) < 0;

		/// <summary>
		///  左辺が右辺以下かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より小さいか等しい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator <=(NameString left, NameString right)
			=> left.CompareTo(right) <= 0;

		/// <summary>
		///  左辺が右辺超過かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より大きい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator >(NameString left, NameString right)
			=> left.CompareTo(right) > 0;

		/// <summary>
		///  左辺が右辺以上かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値を指定します。</param>
		/// <param name="right">右辺の値を指定します。</param>
		/// <returns>左辺の方が右辺より大きいか等しい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator >=(NameString left, NameString right)
			=> left.CompareTo(right) >= 0;

		/// <summary>
		///  名前文字列を通常の文字列へ暗黙的に変換します。
		/// </summary>
		/// <param name="name">通常の文字列へ変換する名前文字列を指定します。</param>
		/// <returns>変換後の文字列を返します。</returns>
		/// <exception cref="System.ArgumentException"/>
		public static implicit operator string(NameString name) => name.ToString();

		/// <summary>
		///  名前文字列をバイト配列へ暗黙的に変換します。
		/// </summary>
		/// <param name="name">バイト配列へ変換する名前文字列を指定します。</param>
		/// <returns>変換後のバイト配列を返します。</returns>
		public static implicit operator byte[](NameString name) => name.ToBinary();

		/// <summary>
		///  通常の文字列を名前文字列へ明示的に変換します。
		/// </summary>
		/// <param name="name">名前文字列へ変換する通常の文字列を指定します。</param>
		/// <returns>変換後の名前文字列を返します。</returns>
		/// <exception cref="System.ArgumentException"/>
		public static explicit operator NameString(string name) => new(name);

		/// <summary>
		///  バイト配列を名前文字列へ明示的に変換します。
		/// </summary>
		/// <param name="name">名前文字列へ変換するバイト配列を指定します。</param>
		/// <returns>変換後の名前文字列を返します。</returns>
		public static explicit operator NameString(byte[] name) => FromBinary(name);

		/// <summary>
		///  バイト配列を名前文字列へ明示的に変換します。
		/// </summary>
		/// <param name="name">名前文字列へ変換するバイト配列を指定します。</param>
		/// <returns>変換後の名前文字列を返します。</returns>
		public static explicit operator NameString(ReadOnlySpan<byte> name) => FromBinary(name);
	}
}
