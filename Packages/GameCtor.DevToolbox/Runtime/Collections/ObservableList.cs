using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GameCtor.DevToolbox
{
    public sealed class ObservableList<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly List<T> _list;

        public ObservableList()
        {
            _list = new List<T>();
        }

        public ObservableList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        public ObservableList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public T this[int index]
        {
            get => _list[index];
            set
            {
                _list[index] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, index));
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => ((IList<T>)_list).IsReadOnly;

        public void Add(T item)
        {
            _list.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void AddRange(IEnumerable<T> collection)
        {
            var startingIndex = _list.Count;
            _list.AddRange(collection);
            var changedItems = collection is IList<T> list ? list : new List<T>(collection);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                changedItems,
                startingIndex));
        }

        public void Clear()
        {
            _list.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item) => _list.IndexOf(item);

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public bool Remove(T item)
        {
            var index = _list.IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            _list.RemoveAt(index);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            return true;
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
