using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Repo2.Core.ns11.DataStructures
{
    public class Observables<T> : ObservableCollection<T>
    {
        public Observables() : base()
        {
        }


        public Observables(IEnumerable<T> collection) : base(collection)
        {
        }


        public void Swap(IEnumerable<T> newItems)
        {
            this.ClearItems();
            foreach (var item in newItems)
                this.Add(item);
        }
    }
}
