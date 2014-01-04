using System;
using System.Threading;

/*
 *  AsyncResult implementation taken from http://blogs.msdn.com/b/ploeh/archive/2007/02/09/agenericiasyncresultimplementation.aspx on 1/10/2011
 *  No licensing information found. If the author needs to contact me, please leave a message on the Quick and Dirty Feed Parser discussion boards:
 *  https://github.com/Aaronontheweb/qdfeed
 *  - Aaron Stannard
 */

namespace QDFeedParser.Silverlight.Extensions
{
    public class AsyncResult<T> : IAsyncResult, IDisposable
    {
        private readonly AsyncCallback _callback;
        private bool _completed;
        private bool _completedSynchronously;
        private readonly object _asyncState;
        private readonly ManualResetEvent _waitHandle;
        private T _result;
        private Exception _e;
        private readonly object _syncRoot;

        internal AsyncResult(AsyncCallback cb, object state) : this(cb, state, false) { }
        internal AsyncResult(AsyncCallback cb, object state, bool completed)
        {
            this._callback = cb;
            this._asyncState = state;
            this._completed = completed;
            this._completedSynchronously = completed;
            this._waitHandle = new ManualResetEvent(false);
            this._syncRoot = new object();
        }

        #region IAsyncResult Members

        public object AsyncState { get { return this._asyncState; } }
        public WaitHandle AsyncWaitHandle { get { return this._waitHandle; } }
        public bool CompletedSynchronously { get { lock (this._syncRoot) { return this._completedSynchronously; } } }
        public bool IsCompleted { get { lock (this._syncRoot) { return this._completed; } } }
        #endregion
        #region IDisposable Members
        public void Dispose() { this.Dispose(true); GC.SuppressFinalize(this); }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            lock (this._syncRoot) { if (this._waitHandle != null) { ((IDisposable)this._waitHandle).Dispose(); } }
        }

        internal Exception Exception { get { lock (this._syncRoot) { return this._e; } } }
        internal T Result { get { lock (this._syncRoot) { return this._result; } } }
        internal void Complete(T result, bool completedSynchronously) { lock (this._syncRoot) { this._completed = true; this._completedSynchronously = completedSynchronously; this._result = result; } this.SignalCompletion(); }     internal void HandleException(Exception e, bool completedSynchronously) { lock (this._syncRoot) { this._completed = true; this._completedSynchronously = completedSynchronously; this._e = e; } this.SignalCompletion(); }     private void SignalCompletion() { this._waitHandle.Set(); ThreadPool.QueueUserWorkItem(new WaitCallback(this.InvokeCallback)); }     private void InvokeCallback(object state) { if (this._callback != null) { this._callback(this); } }
    }
}
