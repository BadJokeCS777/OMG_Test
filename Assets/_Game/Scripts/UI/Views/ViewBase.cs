using System;
using System.Collections.Generic;
using UI.ViewModels;
using UniRx;
using UnityEngine;

namespace UI.Views
{
    public class ViewBase<Vm> : MonoBehaviour where Vm : ViewModelBase
    {
        private readonly List<IDisposable> _subscriptions = new();

        protected Vm ViewModel;

        public virtual void BindViewModel(Vm viewModel)
        {
            ViewModel = viewModel;
        }

        protected void Subscribe<T>(IObservable<T> observable, Action<T> callback)
        {
            _subscriptions.Add(observable.Subscribe(callback));
        }

        protected virtual void OnDestroy()
        {
            ViewModel.Dispose();
            ViewModel = null;

            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
        }
    }
}
