using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Repo2.Core.ns11.DataStructures
{
    public class Observables<T> : ObservableCollection<T>
    {
        public void Swap(IEnumerable<T> newItems)
        {
            this.ClearItems();
            foreach (var item in newItems)
                this.Add(item);
        }
    }
}
