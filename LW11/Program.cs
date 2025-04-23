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
        static DoublyLinkedList<MusicalInstrument> instrumentList = new DoublyLinkedList<MusicalInstrument>();
        static void Main()//ADD MENU!!!!!!!!!!!!!!!!!!!!!!
        {
            
            while (true)
            {
                Console.WriteLine("\n=== Musical Instruments List Manager ===");
                Console.WriteLine("1. Show current list");
                Console.WriteLine("2. Add random elements to odd positions");
                Console.WriteLine("3. Remove elements starting from specified name");
                Console.WriteLine("4. Create deep clone of the list");
                Console.WriteLine("5. Clear the list");
                Console.WriteLine("6. Load test data");
                Console.WriteLine("0. Exit");

                int choice = ValidInput.GetInt("Enter your choice (0-6):");
                switch (choice)
                {
                    case 1:
                        ShowList();
                        break;
                    case 2:
                        AddOddIndexElements();
                        break;
                    case 3:
                        RemoveFromElement();
                        break;
                    case 4:
                        CloneList();
                        break;
                    case 5:
                        ClearList();
                        break;
                    case 6:
                        InitializeDefaultInstruments();
                        Console.WriteLine("List cleared. Test data loaded");
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Wrong choice, try again");
                        break;
                }

            }
            

            #region ToDelete
            //// 1. Сформировать двунаправленный список
            //DoublyLinkedList<MusicalInstrument> instrumentList = new DoublyLinkedList<MusicalInstrument>();

            //// Создаем несколько музыкальных инструментов
            //Piano piano1 = new Piano("Grand Piano", 1, "Acoustic", 88);
            //Piano piano2 = new Piano("Electric Piano", 2, "Digital", 76);
            //MusicalInstrument guitar = new MusicalInstrument("Guitar", 3);
            //MusicalInstrument violin = new MusicalInstrument("Violin", 4);
            //MusicalInstrument drums = new MusicalInstrument("Drums", 5);
            //MusicalInstrument flute = new MusicalInstrument("Flute", 6);

            //// Добавляем инструменты в список
            //instrumentList.Add(piano1);
            //instrumentList.Add(piano2);
            //instrumentList.Add(guitar);
            //instrumentList.Add(violin);
            //instrumentList.Add(drums);
            //instrumentList.Add(flute);

            //// 2. Распечатать полученный список
            //Console.WriteLine("Original List:");
            //instrumentList.PrintList();
            //Console.WriteLine();

            //// 3. Выполнить обработку списка:
            //// а) Добавить элементы с номерами 1, 3, 5 и т.д.  

            //Console.WriteLine("Enter a number of elements to add");
            //int count = ValidInput.GetPositiveInt();
            //instrumentList.AddOddIndexElements(count);
            //Console.WriteLine("List after adding odd index elements:");
            //instrumentList.PrintList();
            //Console.WriteLine();

            ////б) Удалить из списка все элементы, начиная с элемента с заданным именем +  5. Выполнить глубокое клонирование списка
            //DoublyLinkedList<MusicalInstrument> clonedList = instrumentList.DeepClone();
            //Console.WriteLine("Enter instrument name to start removal from:");
            //string nameToRemove = Console.ReadLine() ?? "";
            //instrumentList.RemoveFromElementToEnd(nameToRemove);
            //instrumentList.begin.Data.Name = "Adasdasdsad";

            //Console.WriteLine($"List after removing from '{nameToRemove}' to end:");
            //instrumentList.PrintList();
            //Console.WriteLine();
            //Console.WriteLine("Cloned list:");
            //clonedList.PrintList();
            //Console.WriteLine();






            ////6.Удалить список из памяти
            //instrumentList.Clear();
            //Console.WriteLine("Original list after clearing:");
            //instrumentList.PrintList(); // Должен быть пуст
            //Console.WriteLine();

            //Console.WriteLine("Cloned list (should still exist):");
            //clonedList.PrintList();
            #endregion

        }
        static void InitializeDefaultInstruments()
        {
            instrumentList.Clear();
            instrumentList.Add(new Piano("Grand Piano", 1, "Acoustic", 88));
            instrumentList.Add(new Piano("Electric Piano", 2, "Digital", 76));
            instrumentList.Add(new MusicalInstrument("Guitar", 3));
            instrumentList.Add(new MusicalInstrument("Violin", 4));
            instrumentList.Add(new MusicalInstrument("Drums", 5));
            instrumentList.Add(new MusicalInstrument("Flute", 6));
        }

        static void ShowList()
        {
            Console.WriteLine("\nCurrent instruments list:");
            if (instrumentList.Count == 0)
            {
                Console.WriteLine("The list is empty");
            }
            else
            {
                instrumentList.PrintList();
            }
        }

        static void AddOddIndexElements()
        {
            int count = ValidInput.GetPositiveInt("Enter number of elements to add:");
            instrumentList.AddOddIndexElements(count);
            Console.WriteLine($"Added {count} elements to odd positions");
            ShowList();
        }

        static void RemoveFromElement()
        {
            if (instrumentList.Count == 0)
            {
                Console.WriteLine("The list is empty, nothing to remove");
                return;
            }

            ShowList();
            Console.Write("\nEnter instrument name to start removal from: ");
            string nameToRemove = Console.ReadLine() ?? "";
            instrumentList.RemoveFromElementToEnd(nameToRemove);
            Console.WriteLine($"Elements starting from '{nameToRemove}' have been removed");
            ShowList();
        }

        static void CloneList()
        {
            var clonedList = instrumentList.DeepClone();
            Console.WriteLine("\nCreated deep clone of the list:");
            clonedList.PrintList();

            Console.WriteLine("\nModifying original list to verify cloning:");
            if (instrumentList.Count > 0)
            {
                instrumentList.begin.Data.Name = "Modified Name";
                Console.WriteLine("Original list after modification:");
                instrumentList.PrintList();
                Console.WriteLine("\nClone remains unchanged:");
                clonedList.PrintList();
            }
        }

        static void ClearList()
        {
            instrumentList.Clear();
            Console.WriteLine("The list has been cleared");
        }
    }
}
