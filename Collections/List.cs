using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    internal class List<T> : ICollection<T>
    {

        public Point<T> begin;
        public int Count
        {
            get
            {
                int count = 0;
                if (begin == null)
                    return 0;
                Point<T> current = begin;
                while (current != null)
                {
                    count++;
                    current = current.Next;
                }
                return count;
            }
        }

        public List()
        {
            begin = null;
        }


        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            Point<T> newPoint = new Point<T>(item);
            if (begin == null)
                begin = newPoint;
            else
                AddToEnd(newPoint);
        }

        public void Add(int number, T item)
        {
            Point<T> newPoint = new Point<T>(item);
            if (number > Count + 1)
                throw new Exception("number is bigger than number of elements in list");
            if (number == 1)
            {
                AddToBegin(newPoint);
                return;
            }

            int count = 1;
            Point<T> current = begin;
            while (current.Next != null)
            {
                if (count + 1 == number)
                    break;
                count++;
                current = current.Next;
            }

            if (current.Next == null)
                AddToEnd(newPoint);
            else
            {
                newPoint.Next = current.Next;
                current.Next = newPoint;
            }
        }



        public void Clear()
        {
            begin = null;
        }

        public bool Contains(T item)
        {
            Point<T> current = begin;
            while (current != null && !current.Data.Equals(item))
                current = current.Next;
            return current != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
