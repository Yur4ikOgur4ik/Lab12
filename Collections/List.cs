using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicalInstruments;

namespace Collections
{
    public class DoublyLinkedList<T> where T : MusicalInstrument, IInit, ICloneable
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
        public void AddOddIndexElements()
        {
            Console.WriteLine("Enter a number of elements to add");
            int count = ValidInput.GetPositiveInt();

            // Создаем новый список для хранения элементов в нужном порядке
            DoublyLinkedList<T> newList = this.DeepClone();
            this.Clear();

            int elementIndex = 0;
            for (int i = 0; i < newList.Count + count; i++)
            {
                if (elementIndex < newList.Count)
                {
                    if (i % 2 == 0) // Нечетные позиции (1, 3, 5 и т.д.)
                    {
                        var rndPiano = new Piano();
                        rndPiano.RandomInit();
                        this.Add((T)(object)rndPiano);
                    }
                    else // Четные позиции (2, 4, 6 и т.д.)
                    {

                        this.Add(newList.GetElementAt(elementIndex));
                        elementIndex++;


                    }
                }
            }
            while (elementIndex < newList.Count)
            {
                this.Add(newList.GetElementAt(elementIndex));
                elementIndex++;

            };
            Console.WriteLine(elementIndex);
            Console.WriteLine(newList.Count);



            
        }

        private T GetElementAt(int index)
        {
            if (index < 0 || index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            Point<T>? current = Begin;
            for (int i = 0; i < index; i++)
            {
                if (current == null)
                    throw new InvalidOperationException("Unexpected end of list.");
                current = current.Next;
            }
            if (current == null)
                throw new InvalidOperationException("Unexpected end of list.");
            return current.Data!;
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

