using System;

namespace Framework.Develop
{
    public interface IClassEventSystem : IService
    {
        void Subscribe(int id, Action<object,GameEventArgs>handler);
        void Unsubscribe(int id, Action<object,GameEventArgs> handler);

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        void Fire(object sender, GameEventArgs e);


        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        void FireNow(object sender, GameEventArgs e);
    }
}