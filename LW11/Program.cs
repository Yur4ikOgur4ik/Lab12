using MusicalInstruments;
using System.Collections;
using System;
using System.Dynamic;
using System.Transactions;
using System.Collections.Generic;
using System.Reflection;
using Collections;
using System.Xml.Linq;


namespace LW11
{
    public class Program
    {
        static AVLTree<MusicalInstrument> avlTree = new();
        static DoublyLinkedList<MusicalInstrument> instrumentList = new DoublyLinkedList<MusicalInstrument>();
        static MyOpenHs<MusicalInstrument> hashTable = new MyOpenHs<MusicalInstrument>(4, 0.72);
        

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
            hashTable.Add(new Guitar("Acoustic Guitar", 3, 6));
            hashTable.Add(new Guitar("Bass Guitar", 4, 4));
            hashTable.Add(new MusicalInstrument("Drums", 5));
            hashTable.Add(new MusicalInstrument("Flute", 6));


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
                Console.WriteLine("\n=== Меню работы с деревом ===");
                Console.WriteLine("1. Добавить элемент в дерево");
                Console.WriteLine("2. Вывести дерево по уровням");
                Console.WriteLine("3. Найти элемент по имени");
                Console.WriteLine("4. Удалить элемент по имени");
                Console.WriteLine("5. Загрузить тестовые данные");
                Console.WriteLine("6. Очистить дерево");
                Console.WriteLine("0. Вернуться в главное меню");

                int choice = ValidInput.GetInt("Введите ваш выбор:");

                switch (choice)
                {
                    case 1:
                        AddToTree();
                        break;
                    case 2:
                        PrintTreeLevelOrder();
                        break;
                    case 3:
                        FindInTree();
                        break;
                    case 4:
                        RemoveFromTree();
                        break;
                    case 5:
                        LoadDefaultTreeData();
                        break;
                    case 6:
                        avlTree.Clear();
                        Console.WriteLine("Дерево очищено.");
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void AddToTree()
        {
            var instrument = GetMusicalInstrument();
            avlTree.Insert(instrument);
            Console.WriteLine($"Добавлено: {instrument.Name}");
        }

        static void FindInTree()
        {
            Console.Write("Введите имя инструмента для поиска: ");
            string name = Console.ReadLine().Trim();

            List<MusicalInstrument> found = new List<MusicalInstrument>();
            TraverseAndCollect(avlTree.Root, name, found);

            if (found.Count > 0)
            {
                Console.WriteLine("Найденные элементы:");
                foreach (var item in found)
                    Console.WriteLine(item);
            }
            else
            {
                Console.WriteLine("Элемент не найден.");
            }
        }

        private static void TraverseAndCollect(TreeNode<MusicalInstrument> node, string name, List<MusicalInstrument> found)
        {
            if (node == null) return;

            TraverseAndCollect(node.Left, name, found);

            if (node.Data.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                found.Add(node.Data);

            TraverseAndCollect(node.Right, name, found);
        }

        static void RemoveFromTree()
        {
            Console.Write("Введите имя инструмента для удаления: ");
            string name = Console.ReadLine().Trim();

            bool removed = false;
            CollectAndRemove(avlTree.Root, name, ref removed);
            if (!removed)
                Console.WriteLine("Элемент не найден или дерево пустое.");
            else
                Console.WriteLine($"Удалены все элементы с именем: {name}");
        }

        private static void CollectAndRemove(TreeNode<MusicalInstrument> node, string name, ref bool removed)
        {
            if (node == null) return;

            CollectAndRemove(node.Left, name, ref removed);
            CollectAndRemove(node.Right, name, ref removed);

            if (node.Data.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                avlTree.Delete(node.Data);
                removed = true;
            }
        }

        static void LoadDefaultTreeData()
        {
            avlTree.Clear();

            List<MusicalInstrument> instruments = new()
    {
        new Piano("Grand Piano", 1, "Acoustic", 88),
        new Piano("Digital Piano", 2, "Digital", 76),
        new Guitar("Acoustic Guitar", 3, 6),
        new Guitar("Bass Guitar", 4, 4),
        new MusicalInstrument("Drums", 5),
        new MusicalInstrument("Flute", 6)
    };

            // Сортируем перед построением идеального дерева
            instruments.Sort();
            BuildPerfectSubtree(instruments, 0, instruments.Count - 1);

            Console.WriteLine("Тестовые данные загружены в дерево.");
        }

        private static void BuildPerfectSubtree(List<MusicalInstrument> elements, int start, int end)
        {
            if (start > end) return;

            int mid = (start + end) / 2;
            avlTree.Insert(elements[mid]);

            BuildPerfectSubtree(elements, start, mid - 1);
            BuildPerfectSubtree(elements, mid + 1, end);
        }

        #endregion
    }
}
