using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go4work
{
    /// <summary>
    /// obserwowalna kolekcja która ma limit elementów
    /// przekroczenie limitu powoduje zignorowanie dodawania elementu
    /// </summary>
    /// <typeparam name="T">type elementów kolekcji</typeparam>
    public class LimitedObservableCollection<T>: ObservableCollection<T>
    {
        /// <summary>
        /// rozmiar kolekcji (może zostać w dowolnej chwili zmieniony)
        /// </summary>
        public int Capacity { 
            get => _capacity;
            set
            {
                // usuwanie nadmiarowych elementów
                if (value < this.Count)
                {
                    if (value < 0) {
                        this.ClearItems();
                        return;
                    }

                    while(this.Count > value)
                    {
                        this.RemoveAt(this.Count - 1);
                    }
                }
                this._capacity = value;
            }
        }
        private int _capacity;

        public LimitedObservableCollection(int capacity = 10): base()
        {
            Capacity = capacity;
        }

        public LimitedObservableCollection(IEnumerable<T> collection, int capacity = 10): base(CreateLimitedCopy(collection, capacity))
        {
            Capacity = capacity;
        }

        public LimitedObservableCollection(List<T> list, int capacity = 10): base(CreateLimitedCopy(list, capacity))
        {
            Capacity = capacity;
        }

        /// <summary>
        /// uruchamianie przy każdym wstawianiu elementu - blokuje dodawanie elementów jeśli kolekcja jest pełna
        /// </summary>
        /// <param name="index">numer indeksu w którym zostanie wstawiony nowy element</param>
        /// <param name="item">rzecz do wstawienia</param>
        protected override void InsertItem(int index, T item)
        {
            if (this.Count >= this.Capacity)
            {
                Debug.WriteLine("LimitedObservableCollection - limit reached");
                return;
            }

            base.InsertItem(index, item);
        }

        // tworzy kopię kolekcji z ograniczoną ilością elementów
        private static List<T> CreateLimitedCopy(IEnumerable<T> collection, int size)
        {
            var result = new List<T>();

            foreach (var item in collection)
            {
                if (result.Count >= size)
                    break;

                result.Add(item);
            }

            return result;
        }
    }
}
