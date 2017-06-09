using Repo2.Core.ns11.ChangeNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        private      EventHandler<T> _selectedItemChanged;
        public event EventHandler<T>  SelectedItemChanged
        {
            add    { _selectedItemChanged -= value; _selectedItemChanged += value; }
            remove { _selectedItemChanged -= value; }
        }

        private      EventHandler _contentSwapped;
        public event EventHandler  ContentSwapped
        {
            add    { _contentSwapped -= value; _contentSwapped += value; }
            remove { _contentSwapped -= value; }
        }

        private T _selectedItem;


        public Observables() : base()
        {
            //this.PropertyChanged += Observables_PropertyChanged;
        }


        public Observables(IEnumerable<T> collection) : base(collection)
        {
            //this.PropertyChanged += Observables_PropertyChanged;
        }



        public double?   ManualTotal    { get; set; }

        public T         SelectedItem
        {
            get => _selectedItem;
            set =>_selectedItemChanged.Raise(_selectedItem = value);
        }


        public void Swap(IEnumerable<T> newItems)
        {
            this.ClearItems();

            if (newItems != null)
            {
                foreach (var item in newItems)
                    this.Add(item);
            }

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


        //private void Observables_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == nameof(SelectedItem))
        //        _selectedItemChanged.Raise(SelectedItem);
        //}


        public void RaiseSelectionChanged()
            => _selectionChanged.Raise();
    }
}
