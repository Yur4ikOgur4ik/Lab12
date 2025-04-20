using MusicalInstruments;
using System.Collections;
using System;
using System.Dynamic;
using System.Transactions;
using System.Collections.Generic;
using System.Reflection;
using Collections;


namespace LW11
{
    public class Program
    {
        static void Main()
        {
            // 1. Сформировать двунаправленный список
            DoublyLinkedList<MusicalInstrument> instrumentList = new DoublyLinkedList<MusicalInstrument>();

            // Создаем несколько музыкальных инструментов
            Piano piano1 = new Piano("Grand Piano", 1, "Acoustic", 88);
            Piano piano2 = new Piano("Electric Piano", 2, "Digital", 76);
            MusicalInstrument guitar = new MusicalInstrument("Guitar", 3);
            MusicalInstrument violin = new MusicalInstrument("Violin", 4);
            MusicalInstrument drums = new MusicalInstrument("Drums", 5);
            MusicalInstrument flute = new MusicalInstrument("Flute", 6);

            // Добавляем инструменты в список
            instrumentList.Add(piano1);
            instrumentList.Add(piano2);
            instrumentList.Add(guitar);
            instrumentList.Add(violin);
            instrumentList.Add(drums);
            instrumentList.Add(flute);

            // 2. Распечатать полученный список
            Console.WriteLine("Original List:");
            instrumentList.PrintList();
            Console.WriteLine();

            // 3. Выполнить обработку списка:
            // а) Добавить элементы с номерами 1, 3, 5 и т.д.
            List<MusicalInstrument> newInstruments = new List<MusicalInstrument>
        {
            new MusicalInstrument("Harp", 7),
            new MusicalInstrument("Trumpet", 8),
            new MusicalInstrument("Saxophone", 9),
            new MusicalInstrument("Cello", 10),
            new MusicalInstrument("Clarinet", 11)
        };

            instrumentList.AddOddIndexElements();
            Console.WriteLine("List after adding odd index elements:");
            instrumentList.PrintList();
            Console.WriteLine();

            // б) Удалить из списка все элементы, начиная с элемента с заданным именем
            Console.WriteLine("Enter instrument name to start removal from:");
            string nameToRemove = Console.ReadLine() ?? "";
            instrumentList.RemoveFromElementToEnd(nameToRemove);
            Console.WriteLine($"List after removing from '{nameToRemove}' to end:");
            instrumentList.PrintList();
            Console.WriteLine();

            // 5. Выполнить глубокое клонирование списка
            DoublyLinkedList<MusicalInstrument> clonedList = instrumentList.DeepClone();
            Console.WriteLine("Cloned list:");
            clonedList.PrintList();
            Console.WriteLine();

            // 6. Удалить список из памяти
            instrumentList.Clear();
            Console.WriteLine("Original list after clearing:");
            instrumentList.PrintList(); // Должен быть пуст
            Console.WriteLine();

            Console.WriteLine("Cloned list (should still exist):");
            clonedList.PrintList();
        }

    }
}
