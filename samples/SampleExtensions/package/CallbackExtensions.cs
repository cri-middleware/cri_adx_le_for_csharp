using CriWare.Interfaces;
using System;
using System.Threading;

namespace CriWare {
	/// <summary>
	/// CRIWAREのコールバック向けのユーティリティクラス
	/// </summary>
	public static class CallbackExtensions
	{
		class CallbackContext<TArgs>
		{
			public Action<TArgs> callback;
			public TArgs args;

			static System.Collections.Concurrent.ConcurrentStack<CallbackContext<TArgs>> pool = new System.Collections.Concurrent.ConcurrentStack<CallbackContext<TArgs>>();
			public static CallbackContext<TArgs> Get(Action<TArgs> callback, TArgs args)
			{
				pool.TryPop(out var result);
				result ??= new CallbackContext<TArgs>();
				result.callback = callback;
				result.args = args;
				return result;
			}
			public void Return()
			{
				callback = null;
				args = default;
				pool.Push(this);
			}
		}

		class AnonymousDisposable : IDisposable
		{
			Action action;
			public AnonymousDisposable(Action action) => this.action = action;
			public void Dispose() => Interlocked.Exchange(ref action, null)?.Invoke();
		}

		/// <summary>
		/// CRIWAREの各コールバックを登録元スレッドで購読可能にする拡張メソッド
		/// </summary>
		/// <typeparam name="TArgs">コールバック引数の型</typeparam>
		/// <param name="callback">CRIWAREのコールバックオブジェクト</param>
		/// <param name="action">リスナーとなるデリゲート</param>
		/// <param name="continueOnCapturedContext">コールバックを購読元スレッドへ移譲するか</param>
		/// <returns>購読解除をハンドリングするためのIDisposableオブジェクト</returns>
		public static IDisposable RegisterListener<TArgs>(this CriWare.Interfaces.ICallback<TArgs> callback, Action<TArgs> action, bool continueOnCapturedContext = true)
		{
			var syncContext = continueOnCapturedContext ? SynchronizationContext.Current : null;
			Action<TArgs> handler = syncContext == null ?
				args => action(args) :
				args => {
					syncContext.Post(static contxt => {
						var ctx = contxt as CallbackContext<TArgs>;
						ctx.callback(ctx.args);
						ctx.Return();
					}, CallbackContext<TArgs>.Get(action, args));
				};
			callback.Event += handler;
			return new AnonymousDisposable(() => callback.Event -= handler);
		}

		/// <summary>
		/// コピー済みPCMデータを渡すコールバックへの変換
		/// </summary>
		/// <typeparam name="T">PCMデータ格納オブジェクト</typeparam>
		/// <param name="callback">オリジナルのコールバックオブジェクト</param>
		/// <returns>コピー済みPCMデータをもつコールバックオブジェクト</returns>
		/// <remarks>
		/// CRIWAREのフィルターコールバックでは、引数としてその時点でのPCMデータへのポインタを渡されます。
		/// ただし、その内容を遅延して別スレッドで読み取る場合などはポインタの示す先が生存している保証はありません。
		/// 本メソッドによる変換後のイベントでは、コールバック発生時点でマネージド配列へPCMデータをコピーしているため、
		/// 遅延したアクセスを安全に行うことが可能です。
		/// </remarks>
		public static ICallback<float[][]> WithCopiedPcm<T>(this ICallback<T> callback) where T : IPcmData =>
			new CallbackWithSamples<T>(callback);

		class CallbackWithSamples<T> : ICallback<float[][]>
			where T : IPcmData
		{
			float[][] data;
			ICallback<T> callback;
			public CallbackWithSamples(ICallback<T> callback) => this.callback = callback;

			Action<float[][]> _event = null;
			public event Action<float[][]> Event
			{
				add {
					if(_event == null)
						callback.Event += Callback_Event;
					_event += value;
				}
				remove
				{
					_event -= value;
					if(_event != null)
						callback.Event -= Callback_Event;
				}
			}

			void Callback_Event(T obj)
			{
				if ((data?.Length ?? 0) != obj.numChannels)
					data = new float[obj.numChannels][];
				for (int i = 0; i < obj.numChannels; i++)
				{
					var span = obj.GetFloatPcm(i);
					if ((data[i]?.Length ?? 0) != span.Length)
						data[i] = new float[span.Length];
					span.CopyTo(data[i]);
				}
				_event?.Invoke(data);
			}
		}
	}
}
