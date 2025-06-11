using MusicalInstruments;
using System.Collections;
using System;
using System.Dynamic;
using System.Transactions;
using System.Collections.Generic;
using System.Reflection;
using Collections;
using System.Xml.Linq;
using System.Diagnostics;
using System.ComponentModel.Design.Serialization;
using System.Collections.ObjectModel;


namespace LW11
{
    public class Program
    {
        
        static DoublyLinkedList<MusicalInstrument> instrumentList = new DoublyLinkedList<MusicalInstrument>();
        static MyOpenHs<MusicalInstrument> hashTable = new MyOpenHs<MusicalInstrument>(4, 0.72);
        static BinaryTree<MusicalInstrument> binaryTree = new BinaryTree<MusicalInstrument>();
        private static BinaryTree<MusicalInstrument> searchTree = new BinaryTree<MusicalInstrument>();
        private static MyCollection<MusicalInstrument> collection = new MyCollection<MusicalInstrument>(3);


        #region Enter MI
        static string InputInstrumentType()
        {
            string instrType;
            Console.WriteLine("Choose an instrument: \n1) - Musical instrument (name, id)\n2) - Guitar (name, id, num of strings)\n3) - Electroguitar (guitar + power source)\n4) - Piano (name, id, key layout, number of keys)");
            int typeInt;
            typeInt = ValidInput.GetPositiveInt();
            while (typeInt > 5)
            {
                Console.WriteLine("Enter a number between 1 and 4");
                typeInt = ValidInput.GetPositiveInt();
            }
            switch (typeInt)
            {
                case 1:
                    instrType = "MusicalInstrument";
                    break;
                case 2:
                    instrType = "Guitar";
                    break;
                case 3:
                    instrType = "Eguitar";
                    break;
                case 4:
                    instrType = "Piano";
                    break;
                default:
                    instrType = "Aboba";
                    break;
            }
            return instrType;
        }
        
        static MusicalInstrument GetMusicalInstrument()
        {
            string instrType;
            instrType = InputInstrumentType();
            MusicalInstrument instr = new();
            switch (instrType)
            {
                case "MusicalInstrument":
                    instr = new MusicalInstrument();
                    instr.Init();
                    break;
                case "Guitar":
                    instr = new Guitar();
                    instr.Init();
                    break;
                case "Eguitar":
                    instr = new ElectroGuitar();
                    instr.Init();
                    break;
                case "Piano":
                    instr = new Piano();
                    instr.Init();
                    break;
                default:
                    Console.WriteLine("An unexpected error ocurred");
                    break;

            }
            return instr;
        }
        #endregion
        static void Main()//ADD MENU!!!!!!!!!!!!!!!!!!!!!!
        {

            {
                while (true)
                {
                    Console.WriteLine("\n=== Main menu ===");
                    Console.WriteLine("1. List");
                    Console.WriteLine("2. Hash table");
                    Console.WriteLine("3. Binary tree");
                    Console.WriteLine("4. Collection");
                    Console.WriteLine("0. Exit");

                    int choice = ValidInput.GetInt("Enter your choice");

                    switch (choice)
                    {
                        case 1:
                            ListMenu();
                            break;
                        case 2:
                            HashTableMenu();
                            break;
                        case 3:
                            TreeMenu();
                            break;
                        case 4:
                            CollectionMenu();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Wrong choice, try again");
                            break;
                    }
                }
            }


            

        }
        #region Меню работы со списком
        static void ListMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== List menu ===");
                Console.WriteLine("1. Print list");
                Console.WriteLine("2. Add elements on odd positions");
                Console.WriteLine("3. Удалить элементы, начиная с указанного имени");
                Console.WriteLine("4. Создать глубокую копию списка");
                Console.WriteLine("5. Очистить список");
                Console.WriteLine("6. Загрузить тестовые данные");
                Console.WriteLine("0. Вернуться в главное меню");

                int choice = ValidInput.GetInt("Введите ваш выбор:");

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
                        Console.WriteLine("Старый список очищен. Тестовые данные загружены.");
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowList()
        {
            Console.WriteLine("\nТекущий список инструментов:");
            if (instrumentList.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                //instrumentList.PrintList();
            }
        }

        static void AddOddIndexElements()
        {
            int count = ValidInput.GetPositiveInt("Введите количество элементов для добавления:");
            instrumentList.AddOddIndexElements(count);
            Console.WriteLine($"Добавлено {count} элементов на нечётные позиции.");
            ShowList();
        }

        static void RemoveFromElement()
        {
            if (instrumentList.Count == 0)
            {
                Console.WriteLine("Список пуст. Нечего удалять.");
                return;
            }

            ShowList();
            Console.Write("Введите имя инструмента, с которого начать удаление: ");
            string nameToRemove = Console.ReadLine().Trim();

            instrumentList.RemoveFromElementToEnd(nameToRemove);
            Console.WriteLine($"Удалены все элементы, начиная с '{nameToRemove}'.");
            ShowList();
        }

        static void CloneList()
        {
            var clonedList = instrumentList.DeepClone();
            Console.WriteLine("\nСоздана глубокая копия списка:");
            //clonedList.PrintList();

            Console.WriteLine("\nИзменяем оригинальный список...");
            if (instrumentList.Count > 0)
            {
                instrumentList.begin.Data.Name = "Модифицированное имя";
                Console.WriteLine("Оригинальный список после изменения:");
                //instrumentList.PrintList();
                Console.WriteLine("\nКлон остался без изменений:");
                //clonedList.PrintList();
            }
        }

        static void ClearList()
        {
            instrumentList.Clear();
            Console.WriteLine("Список очищен.");
        }

        static void InitializeDefaultInstruments()
        {
            instrumentList.Clear();
            instrumentList.Add(new Piano("Grand Piano", 1, "Acoustic", 88));
            instrumentList.Add(new Piano("Digital Piano", 2, "Digital", 76));
            instrumentList.Add(new Guitar("Acoustic Guitar", 3, 6));
            instrumentList.Add(new Guitar("Bass Guitar", 4, 4));
            instrumentList.Add(new MusicalInstrument("Drums", 5));
            instrumentList.Add(new MusicalInstrument("Flute", 6));
        }
        #endregion
        #region Меню работы с хэш-таблицей
        static void HashTableMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Hash table menu ===");
                Console.WriteLine("1. Fill");
                Console.WriteLine("2. Find");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. Print Hashset");
                Console.WriteLine("5. Add element");
                Console.WriteLine("0. Get back to main menu");

                int choice = ValidInput.GetInt("Enter your choice");

                switch (choice)
                {
                    case 1:
                        FillHashTable();
                        break;
                    case 2:
                        SearchInstrument();
                        break;
                    case 3:
                        RemoveInstrument();
                        break;
                    case 4:
                        PrintHS();
                        break;
                    case 5:
                        AddHashTable();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Wrong choice, try again");
                        break;
                }
            }
        }

        static void FillHashTable()
        {
            hashTable.Clear();

            hashTable.Add(new Piano("Grand Piano", 1, "Octave", 88));
            hashTable.Add(new Piano("Digital Piano", 2, "Scale", 76));
            


            Console.WriteLine("Hash table filled successfully");
            Console.WriteLine(hashTable.PrintHS());
        }

        static void SearchInstrument()
        {
            if (hashTable.Count == 0)
            {
                Console.WriteLine("Hash table is empty, Fill it firts");
                return;
            }

            Console.Write("Enter MusicalInstrument to add: ");
            MusicalInstrument name = GetMusicalInstrument();
            int index;
            MusicalInstrument currentInstrumentToDelete = hashTable.Find(name, out index);
            if (currentInstrumentToDelete != null)
            {
                Console.WriteLine($"Found instrument: {currentInstrumentToDelete.ToString()}, at index: {index}");
            }
            else
                Console.WriteLine("Instrument not found");

            
        }
        static void PrintHS()
        {
            Console.WriteLine(hashTable.PrintHS());
            Console.WriteLine($"Capacity - {hashTable.Capacity}");
        }
        static void RemoveInstrument()
        {
            if (hashTable.Count == 0)
            {
                Console.WriteLine("Hash table is empty, Fill it first");
                return;
            }
            MusicalInstrument name = GetMusicalInstrument();
            

            if (hashTable.Remove(name))
            {
                Console.WriteLine("Instrument successfully deleted");
            }
            else
            {
                Console.WriteLine("Element has been deleted earlier, or it was not in the HashTable");
            }

            
        }

        static void AddHashTable()
        {
            MusicalInstrument instrToAdd = GetMusicalInstrument();
            hashTable.Add(instrToAdd);

            
        }

        #endregion

        #region Меню_работы_с_деревом

        static void TreeMenu()
        {
            while (true)
            {
                Console.WriteLine("\n===== Меню работы с деревом =====");
                Console.WriteLine("1. Создать идеально сбалансированное дерево");
                Console.WriteLine("2. Вывести дерево 'лежащим на боку'");
                Console.WriteLine("3. Преобразовать в дерево поиска");
                Console.WriteLine("4. Вывести дерево поиска");
                Console.WriteLine("5. Найти элемент с максимальным ID");
                Console.WriteLine("6. Delete by ID");
                Console.WriteLine("7. Clear Binary tree");
                Console.WriteLine("8. Clear Search tree");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                int choice = ValidInput.GetInt("Enter your choice");

                switch (choice)
                {
                    case 1:
                        CreateBalancedTree();
                        break;
                    case 2:
                        if (binaryTree != null && binaryTree.Root != null)
                            Console.WriteLine(binaryTree.ShowTree(binaryTree.Root, 0));
                        else
                            Console.WriteLine("Дерево пусто.");
                        break;
                    case 3:
                        if (binaryTree == null || binaryTree.Root == null)
                        {
                            Console.WriteLine("Дерево не создано.");
                        }
                        else
                        {
                            searchTree = binaryTree.ConvertToSearchTree();
                            Console.WriteLine("Дерево преобразовано в дерево поиска.");
                        }
                        break;
                    case 4:
                        if (searchTree == null)
                        {
                            Console.WriteLine("Дерево поиска не создано.");
                        }
                        else
                        {
                            Console.WriteLine("Дерево поиска:");
                            if (searchTree.Root != null)
                                Console.WriteLine(searchTree.ShowTree(searchTree.Root, 0));
                            else
                                Console.WriteLine("Search tree is empry");
                        }
                        break;
                    case 5:
                        if (binaryTree == null || binaryTree.Root == null)
                        {
                            Console.WriteLine("Дерево не создано.");
                        }
                        else
                        {
                            var max = binaryTree.FindMaxId();
                            Console.WriteLine("Элемент с максимальным ID:");
                            Console.WriteLine(max);
                        }
                        break;
                    case 6:
                        if (searchTree == null || searchTree.Root == null)
                        {
                            Console.WriteLine("Дерево поиска пусто.");
                        }
                        else
                        {
                            int idToDelete = ValidInput.GetInt("Введите ID для удаления");
                            if (searchTree.Contains(new IdNumber(idToDelete)))
                            {
                                searchTree.DeleteById(new IdNumber(idToDelete));
                                Console.WriteLine($"Элемент с ID {idToDelete} удален.");
                            }
                            else
                            {
                                Console.WriteLine($"Элемент с ID {idToDelete} не найден.");
                            }
                        }
                        break;
                    case 7:
                        binaryTree.Clear();
                        break;
                    case 8:
                        searchTree.Clear();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
        private static void CreateBalancedTree()
        {
            int size = ValidInput.GetPositiveInt("Введите размер дерева:");

            
            binaryTree.BuildPerfectTree(size);

            Console.WriteLine("Дерево создано.");
        }


        #endregion
        #region Collection
        static void CollectionMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Collection Menu ===");
                Console.WriteLine("1. Add element");
                Console.WriteLine("2. Remove element");
                Console.WriteLine("3. Print all elements (foreach)");
                Console.WriteLine("4. Make a copy of a collection");
                Console.WriteLine("5. Clear collection");
                Console.WriteLine("6. Show count of elements");
                Console.WriteLine("0. Back to main menu");

                int choice = ValidInput.GetInt("Enter your choice:");

                switch (choice)
                {
                    case 1:
                        AddToCollection();
                        break;
                    case 2:
                        RemoveFromCollection();
                        break;
                    
                    case 3:
                        PrintWithForeach();
                        break;
                    case 4:
                        CopyToArrayAndPrint();
                        break;
                    case 5:
                        collection.Clear();
                        Console.WriteLine("Collection cleared.");
                        break;
                    case 6:
                        Console.WriteLine($"Element count: {collection.Count}");
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
        static void AddToCollection()
        {
            Console.WriteLine("Enter instrument to add:");
            MusicalInstrument item = GetMusicalInstrument();
            if (!collection.Contains(item))
            {
                collection.Add(item);
                Console.WriteLine("Instrument added.");
            }
            else
                Console.WriteLine("There is such item in collection already");
        }

        static void RemoveFromCollection()
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("Collection is empty.");
                return;
            }

            Console.WriteLine("Enter instrument to remove:");
            MusicalInstrument item = GetMusicalInstrument();

            if (collection.Remove(item))
                Console.WriteLine("Instrument removed.");
            else
                Console.WriteLine("Instrument not found.");
        }

        

        static void PrintWithForeach()
        {
            Console.WriteLine("\nPrinting via foreach:");
            if (collection.Count == 0)
            {
                Console.WriteLine("Collection is empty.");
                return;
            }

            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }

        static void CopyToArrayAndPrint()
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("Collection is empty.");
                return;
            }

            MyCollection<MusicalInstrument> clonedCollection = new MyCollection<MusicalInstrument>(collection);

            MusicalInstrument rndInstr = new MusicalInstrument();
            rndInstr.RandomInit();
            collection.Add(rndInstr);//add to main to see the difference

            Console.WriteLine("Cloned Collection");
            foreach (var item in clonedCollection) { Console.WriteLine(item); }

            Console.WriteLine("Original collection");
            foreach (var item in collection) { Console.WriteLine(item); }

        }

        #endregion
    }
}
