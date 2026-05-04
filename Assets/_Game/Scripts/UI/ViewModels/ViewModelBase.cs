using System;
using System.Collections.Generic;
using UniRx;

namespace UI.ViewModels
{
    public class ViewModelBase : IDisposable
    {
        private readonly List<IDisposable> _subscriptions = new();

        protected void Subscribe<T>(IObservable<T> observable, Action<T> callback)
        {
            _subscriptions.Add(observable.Subscribe(callback));
        }

        protected virtual void OnDispose()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
        }

        public void Dispose() => OnDispose();
    }
}