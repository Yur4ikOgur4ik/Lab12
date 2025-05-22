using MusicalInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class BinaryTree<T> where T: MusicalInstrument, IInit, ICloneable
    {
        public T CreateRandomInstr()
        {
            Random rnd = new Random();
            MusicalInstrument instr;
            int type = rnd.Next(3);
            switch (type)
            {
                case 0:
                    instr = new MusicalInstrument();
                    break;
                case 1:
                    instr = new Guitar();
                    break;
                case 2:
                    instr = new ElectroGuitar();
                    break;
                default:
                    instr = new Piano();
                    break;

            }
            instr.RandomInit();
            instr.Name = instr.Name + " (Rnd)";
            return (T)(object)instr;
        }
        public TreeNode<T> Root { get; private set; }

        public BinaryTree()
        {
            Root = null;
        }

        public void BuildPerfectTree(int size)
        {
            if (size <= 0)
            {
                Root = null;
                return;
            }

            Root = BuildSubtree(size);
        }

        private TreeNode<T> BuildSubtree(int size)
        {
            if (size == 0)
                return null;

            int leftSize = size / 2;
            int rightSize = size - leftSize - 1;

            // Создаём случайный инструмент для текущего узла
            T instrument = CreateRandomInstr();

            // Создаём узел
            TreeNode<T> node = new TreeNode<T>(instrument);

            // Рекурсивно строим левое и правое поддеревья
            node.Left = BuildSubtree(leftSize);
            node.Right = BuildSubtree(rightSize);

            return node;
        }

        public void ShowTree()
        {
            if (Root == null)
            {
                Console.WriteLine("Tree is empty");
                return;
            }

            ShowTree(Root, 0);
        }

        public void ShowTree(TreeNode<T> node, int level)
        {
            // Сначала выводим правое поддерево
            if (node.Right != null)
                ShowTree(node.Right, level + 1);

            // Выводим текущий узел
            Console.WriteLine($"{new string(' ', level * 4)}[{level}] {node.Data}");

            // Затем левое поддерево
            if (node.Left != null)
                ShowTree(node.Left, level + 1);
        }

        public MusicalInstrument FindMaxId()
        {
            if (Root == null)
            {
                Console.WriteLine("Дерево пусто.");
                return null;
            }

            return FindMaxIdRecursive(Root);
        }

        private MusicalInstrument FindMaxIdRecursive(TreeNode<T> node)
        {
            if (node == null)
                return null;

            // Рекурсивно ищем слева и справа
            var leftMax = FindMaxIdRecursive(node.Left);
            var rightMax = FindMaxIdRecursive(node.Right);

            // Начальный максимум — текущий узел
            MusicalInstrument current = node.Data;

            // Сравниваем с левым поддеревом
            if (leftMax != null && leftMax.ID.Id > current.ID.Id)
            {
                current = leftMax;
            }

            // Сравниваем с правым поддеревом
            if (rightMax != null && rightMax.ID.Id > current.ID.Id)
            {
                current = rightMax;
            }

            return current;
        }


        public BinaryTree<T> ConvertToSearchTree()
        {
            BinaryTree<T> searchTree = new BinaryTree<T>();

            if (Root == null)
                return searchTree;

            TreeAddDeep(Root, searchTree);

            return searchTree;
        }


        private void TreeAddDeep(TreeNode<T> node, BinaryTree<T> searchTree)
        {
            if (node == null)
                return;

            // Клонируем объект перед добавлением
            T clone = (T)node.Data.Clone();

            // Вставляем в дерево поиска по ID
            searchTree.AddToSearchTree(clone);

            // Обходим левое и правое поддеревья
            TreeAddDeep(node.Left, searchTree);
            TreeAddDeep(node.Right, searchTree);
        }


        public void AddToSearchTree(T instrument)
        {
            Root = AddToSearchTree(Root, instrument);
        }

        private TreeNode<T> AddToSearchTree(TreeNode<T> node, T instrument)
        {
            if (node == null)
            {
                return new TreeNode<T>(instrument);
            }

            // Сравниваем по ID.Id
            int compare = instrument.ID.Id.CompareTo(node.Data.ID.Id);

            if (compare < 0)
            {
                node.Left = AddToSearchTree(node.Left, instrument);
            }
            else if (compare > 0)
            {
                node.Right = AddToSearchTree(node.Right, instrument);
            }

            return node;
        }
        public void DeleteById(IdNumber id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            Root = DeleteNode(Root, id);
        }

        private TreeNode<T> DeleteNode(TreeNode<T> node, IdNumber id)
        {
            if (node == null)
                return null;

            // Сравниваем ID
            int compare = id.Id.CompareTo(node.Data.ID.Id);

            if (compare < 0)
            {
                // Идем налево
                node.Left = DeleteNode(node.Left, id);
            }
            else if (compare > 0)
            {
                // Идем направо
                node.Right = DeleteNode(node.Right, id);
            }
            else
            {
                // Нашли узел

                // 1. Узел с одним потомком или без потомков
                if (node.Left == null)
                {
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    return node.Left;
                }

                // 2. Узел с двумя потомками: ищем минимальный в правом поддереве
                T minRight = FindMin(node.Right);

                // Копируем данные минимального узла
                node.Data = minRight;

                // Удаляем минимальный узел из правого поддерева
                node.Right = DeleteNode(node.Right, minRight.ID);
            }

            return node;
        }

        private T FindMin(TreeNode<T> node)
        {
            while (node.Left != null)
                node = node.Left;

            return node.Data;
        }

        public bool Contains(IdNumber id)
        {
            return Contains(Root, id);
        }

        private bool Contains(TreeNode<T> node, IdNumber id)
        {
            if (node == null)
                return false;

            int compare = id.Id.CompareTo(node.Data.ID.Id);

            if (compare == 0)
                return true;
            else if (compare < 0)
                return Contains(node.Left, id);
            else
                return Contains(node.Right, id);
        }
    }
}
