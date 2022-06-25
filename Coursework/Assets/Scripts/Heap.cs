using System;

public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex { get; set; }
}

public class Heap<T> where T : IHeapItem<T>
{
    public int Count => _currentItemCount;

    private T[] _items;
    private int _currentItemCount;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        SortUp(item);
        _currentItemCount++;
    }

    public T RemoveFirst()
    {
        var firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0].HeapIndex = 0;
        SortDown(_items[0]);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item);
    }

    private void SortDown(T item)
    {
        while (true)
        {
            var leftChildIndex = item.HeapIndex * 2 + 1;
            var rightChildIndex = item.HeapIndex * 2 + 2;

            var swapIndex = 0;

            if (leftChildIndex < _currentItemCount)
            {
                swapIndex = leftChildIndex;

                if (rightChildIndex < _currentItemCount)
                {
                    if (_items[leftChildIndex].CompareTo(_items[rightChildIndex]) < 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }

                if (item.CompareTo(_items[swapIndex]) < 0)
                {
                    Swap(item, _items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void SortUp(T item)
    {
        var parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            var parentItem = _items[parentIndex];

            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T itemA, T itemB)
    {
        (_items[itemA.HeapIndex], _items[itemB.HeapIndex]) = (itemB, itemA);
        (itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
    }
}