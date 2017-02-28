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
        }


        public void RaiseSelectionChanged()
            => _selectionChanged.Raise();
    }
}
