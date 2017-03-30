using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Repo2.Core.ns11.ChangeNotification;

namespace Repo2.Core.ns11.DataStructures
{
    public class Observables<T> : ObservableCollection<T>
    {
        private      EventHandler _selectionChanged;
        public event EventHandler  SelectionChanged
        {
            add    { _selectionChanged -= value; _selectionChanged += value; }
            remove { _selectionChanged -= value; }
        }

        private      EventHandler _contentSwapped;
        public event EventHandler  ContentSwapped
        {
            add    { _contentSwapped -= value; _contentSwapped += value; }
            remove { _contentSwapped -= value; }
        }


        public Observables() : base()
        {
        }


        public Observables(IEnumerable<T> collection) : base(collection)
        {
        }


        public double? ManualTotal { get; set; }


        public void Swap(IEnumerable<T> newItems)
        {
            this.ClearItems();

            foreach (var item in newItems)
                this.Add(item);

            _contentSwapped.Raise();
        }


        //  crashes the app
        //
        //public async Task SwapAsync(IEnumerable<T> newItems)
        //{
        //    await Task.Run(async () =>
        //    {
        //        await Task.Delay(1);
        //        Swap(newItems);
        //    });
        //}


        public void RaiseSelectionChanged()
            => _selectionChanged.Raise();
    }
}
