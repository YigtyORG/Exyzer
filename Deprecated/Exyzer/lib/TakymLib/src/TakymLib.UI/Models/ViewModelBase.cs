/****
 * TakymLib
 * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2022 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using TakymLib.Threading.Tasks;

namespace TakymLib.UI.Models
{
	/// <summary>
	///  UI の内部機能を提供するクラスの基底クラスです。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		/// <summary>
		///  プロパティが変更された時に呼び出されます。
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		///  プロパティの変更を通知します。
		/// </summary>
		/// <typeparam name="T">プロパティの型を指定します。</typeparam>
		/// <param name="location">プロパティへの参照を指定します。</param>
		/// <param name="value">プロパティの新しい値を指定します。</param>
		/// <param name="name">プロパティの名前を指定します。</param>
		protected void RaisePropertyChanged<T>(ref T location, T value, string name) where T : class?
		{
			var oldValue = location;
			while (Interlocked.CompareExchange(ref location, value, oldValue) != oldValue) {
				TaskUtility.YieldAndWait();
				oldValue = location;
			}
			this.OnPropertyChanged(CachedPropertyChangedEventArgs.Get(name));
		}

		/// <summary>
		///  プロパティの変更通知を試行します。
		/// </summary>
		/// <typeparam name="T">プロパティの型を指定します。</typeparam>
		/// <param name="location">プロパティへの参照を指定します。</param>
		/// <param name="value">プロパティの新しい値を指定します。</param>
		/// <param name="oldValue">プロパティの元の値を指定します。</param>
		/// <param name="name">プロパティの名前を指定します。</param>
		/// <returns>成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		protected bool TryRaisePropertyChanged<T>(ref T location, T value, T oldValue, string name) where T : class?
		{
			if (Interlocked.CompareExchange(ref location, value, oldValue) == oldValue) {
				this.OnPropertyChanged(CachedPropertyChangedEventArgs.Get(name));
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		///  <see cref="TakymLib.UI.Models.ViewModelBase.PropertyChanged"/>を発生させます。
		/// </summary>
		/// <param name="e">変更されたプロパティの名前を含むイベントデータを格納したオブジェクトを指定します。</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			this.PropertyChanged?.Invoke(this, e);
		}

		private sealed class CachedPropertyChangedEventArgs : PropertyChangedEventArgs
		{
			private static readonly ConcurrentDictionary<string, CachedPropertyChangedEventArgs> _ea_cache = new();

			public sealed override string? PropertyName
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => base.PropertyName;
			}

			private CachedPropertyChangedEventArgs(string name) : base(name) { }

			internal static CachedPropertyChangedEventArgs Get(string name)
			{
				return _ea_cache.GetOrAdd(name, name => new(name));
			}
		}
	}
}
