using Ash;
using Ash.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 事件组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Event")]
    public sealed class EventComponent : ComponentBase
    {
        private IEventManager m_EventManager = null;

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_EventManager.Count;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_EventManager = AshEntry.GetModule<IEventManager>();
            if (m_EventManager == null)
            {
                Log.Error("Event manager is invalid.");
                return;
            }
        }

        private void Start()
        {

        }

        /// <summary>
        /// 检查订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理回调函数。</param>
        /// <returns>是否存在事件处理回调函数。</returns>
        public bool Check(int id, EventHandler<Ash.Event.AshEventArgs> handler)
        {
            return m_EventManager.Check(id, handler);
        }

        /// <summary>
        /// 订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理回调函数。</param>
        public void Subscribe(int id, EventHandler<Ash.Event.AshEventArgs> handler)
        {
            m_EventManager.Subscribe(id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理回调函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理回调函数。</param>
        public void Unsubscribe(int id, EventHandler<Ash.Event.AshEventArgs> handler)
        {
            m_EventManager.Unsubscribe(id, handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件内容。</param>
        public void Fire(object sender, Ash.Event.AshEventArgs e)
        {
            m_EventManager.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件内容。</param>
        public void FireNow(object sender, Ash.Event.AshEventArgs e)
        {
            m_EventManager.FireNow(sender, e);
        }


        #region Dispatcher
        /// <summary>
        /// 触发一个事件,并获取事件的返回结果
        /// <para>如果<paramref name="halt"/>为<c>true</c>那么返回的结果是事件的返回结果,没有一个事件进行处理的话返回<c>null</c>
        /// 反之返回一个事件处理结果数组(<c>object[]</c>)</para>
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="payload">载荷</param>
        /// <param name="halt">是否只触发一次就终止</param>
        /// <returns>事件结果</returns>
        public object Trigger(string eventName, object payload = null, bool halt = false)
        {
            return m_EventManager.Trigger(eventName, payload, halt);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IAshEventHandler On(string eventName, Action<object> handler, int life = 0)
        {
            return m_EventManager.On(eventName, handler, life);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public IAshEventHandler On(string eventName, Func<object, object> handler, int life = 0)
        {
            return m_EventManager.On(eventName, handler, life);
        }
        #endregion
    }
}
