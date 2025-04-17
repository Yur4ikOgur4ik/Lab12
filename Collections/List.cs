using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicalInstruments;

namespace Collections
{
    public class DoublyLinkedList<T>
    {
        public Point<T>? Begin { get; private set; }
        public Point<T>? End { get; private set; } // Добавляем указатель на конец списка

        public int Count
        {
            get
            {
                int count = 0;
                Point<T>? current = Begin;
                while (current != null)
                {
                    count++;
                    current = current.Next;
                }
                return count;
            }
        }

        public DoublyLinkedList()
        {
            Begin = null;
            End = null;
        }

        public void Add(T item)
        {
            Point<T> newPoint = new Point<T>(item);
            if (Begin == null)
            {
                Begin = newPoint;
                End = newPoint;
            }
            else
            {
                End!.Next = newPoint;
                newPoint.Prev = End;
                End = newPoint;
            }
        }

        public void AddToBegin(T item)
        {
            Point<T> newPoint = new Point<T>(item);
            if (Begin == null)
            {
                Begin = newPoint;
                End = newPoint;
            }
            else
            {
                newPoint.Next = Begin;
                Begin.Prev = newPoint;
                Begin = newPoint;
            }
        }

        public void PrintList()
        {
            Point<T>? current = Begin;
            int count = 1;
            while (current != null)
            {
                Console.WriteLine($"{count}: {current.Data}");
                current = current.Next;
                count++;
            }
        }

        // 1. Добавление элементов с номерами 1, 3, 5 и т.д.
        public void AddOddIndexElements(List<T> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (i % 2 == 0) // Индексация с 0, поэтому четные индексы - это 1, 3, 5 позиции
                {
                    this.Add(elements[i]);
                }
            }
        }

        // 2. Удаление всех элементов, начиная с элемента с заданным именем, и до конца списка
        public void RemoveFromElementToEnd(string name)
        {
            Point<T>? current = Begin;
            bool found = false;

            while (current != null)
            {
                if (current.Data is MusicalInstrument instrument && instrument.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
                current = current.Next;
            }

            if (found)
            {
                if (current!.Prev != null)
                {
                    current.Prev.Next = null;
                    End = current.Prev;
                }
                else
                {
                    Begin = null;
                    End = null;
                }
            }
        }

        // 3. Глубокое клонирование списка
        public DoublyLinkedList<T> DeepClone()
        {
            DoublyLinkedList<T> newList = new DoublyLinkedList<T>();
            Point<T>? current = Begin;

            while (current != null)
            {
                if (current.Data is ICloneable cloneable)
                {
                    T clonedItem = (T)cloneable.Clone();
                    newList.Add(clonedItem);
                }
                else
                {
                    // Если элемент не поддерживает клонирование, просто добавляем ссылку
                    newList.Add(current.Data);
                }
                current = current.Next;
            }

            return newList;
        }

        // 4. Удаление списка из памяти
        public void Clear()
        {
            
            Begin = null;
            End = null;
        }
    }
}

