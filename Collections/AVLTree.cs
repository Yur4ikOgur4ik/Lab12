using MusicalInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class AVLTree<T> where T: MusicalInstrument, IInit, ICloneable
    {
        public TreeNode<T> Root { get; private set; }

        public AVLTree()
        {
            Root = null;
        }

        // 1. Построить идеально сбалансированное дерево из отсортированного списка
        public void BuildPerfectTree(List<T> elements)
        {
            Root = null;
            BuildSubtree(elements, 0, elements.Count - 1);
        }

        private void BuildSubtree(List<T> elements, int start, int end)
        {
            if (start > end) return;

            int mid = (start + end) / 2;
            Insert(elements[mid]);

            BuildSubtree(elements, start, mid - 1);
            BuildSubtree(elements, mid + 1, end);
        }

        // Вставка в AVL-дерево (с балансировкой)
        public void Insert(T value)
        {
            Root = InsertNode(Root, value);
        }

        private TreeNode<T> InsertNode(TreeNode<T> node, T value)
        {
            if (node == null)
                return new TreeNode<T>(value);

            int compare = value.CompareTo(node.Data);
            if (compare < 0)
                node.Left = InsertNode(node.Left, value);
            else if (compare > 0)
                node.Right = InsertNode(node.Right, value);
            else
                return node; // Не вставляем дубликаты

            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

            return Balance(node);
        }

        // 6. Удаление узла по ключу
        public void Delete(T key)
        {
            Root = DeleteNode(Root, key);
        }

        private TreeNode<T> DeleteNode(TreeNode<T> node, T key)
        {
            if (node == null)
                return null;

            int compare = key.CompareTo(node.Data);
            if (compare < 0)
                node.Left = DeleteNode(node.Left, key);
            else if (compare > 0)
                node.Right = DeleteNode(node.Right, key);
            else
            {
                // Узел с одним или без потомков
                if (node.Left == null || node.Right == null)
                {
                    TreeNode<T> temp = node.Left ?? node.Right;
                    if (temp == null)
                        return null;
                    else
                        node = temp;
                }
                else
                {
                    // Узел с двумя потомками: ищем Inorder Successor
                    TreeNode<T> minRight = FindMin(node.Right);
                    node.Data = minRight.Data;
                    node.Right = DeleteNode(node.Right, minRight.Data);
                }
            }

            if (node == null)
                return null;

            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            return Balance(node);
        }

        private TreeNode<T> FindMin(TreeNode<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        // Балансировка поддерева
        private TreeNode<T> Balance(TreeNode<T> node)
        {
            int balanceFactor = GetBalanceFactor(node);

            // Left Heavy
            if (balanceFactor > 1)
            {
                if (GetBalanceFactor(node.Left) >= 0)
                    return RotateRight(node); // Left-Left
                else
                {
                    node.Left = RotateLeft(node.Left); // Left-Right
                    return RotateRight(node);
                }
            }

            // Right Heavy
            if (balanceFactor < -1)
            {
                if (GetBalanceFactor(node.Right) <= 0)
                    return RotateLeft(node); // Right-Right
                else
                {
                    node.Right = RotateRight(node.Right); // Right-Left
                    return RotateLeft(node);
                }
            }

            return node;
        }

        // Получить высоту узла
        private int Height(TreeNode<T> node) => node?.Height ?? 0;

        // Получить фактор баланса
        private int GetBalanceFactor(TreeNode<T> node)
        {
            if (node == null) return 0;
            return Height(node.Left) - Height(node.Right);
        }

        // Малые повороты
        private TreeNode<T> RotateRight(TreeNode<T> y)
        {
            var x = y.Left;
            var T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = 1 + Math.Max(Height(y.Left), Height(y.Right));
            x.Height = 1 + Math.Max(Height(x.Left), Height(x.Right));

            return x;
        }

        private TreeNode<T> RotateLeft(TreeNode<T> x)
        {
            var y = x.Right;
            var T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = 1 + Math.Max(Height(x.Left), Height(x.Right));
            y.Height = 1 + Math.Max(Height(y.Left), Height(y.Right));

            return y;
        }

        // 2. Распечатать дерево по уровням (уровневый обход)
        public void PrintLevelOrder()
        {
            if (Root == null)
            {
                Console.WriteLine("Дерево пустое.");
                return;
            }

            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(Root);

            int level = 0;
            while (queue.Count > 0)
            {
                int levelCount = queue.Count;
                Console.Write($"Уровень {level}: ");

                for (int i = 0; i < levelCount; i++)
                {
                    var current = queue.Dequeue();
                    Console.Write(current.Data + " ");

                    if (current.Left != null) queue.Enqueue(current.Left);
                    if (current.Right != null) queue.Enqueue(current.Right);
                }

                Console.WriteLine();
                level++;
            }
        }

        // 3. Обработка дерева — пример: поиск элементов по условию
        public void ProcessTree(Func<T, bool> condition)
        {
            TraverseAndProcess(Root, condition);
        }

        private void TraverseAndProcess(TreeNode<T> node, Func<T, bool> condition)
        {
            if (node == null) return;

            TraverseAndProcess(node.Left, condition);
            if (condition(node.Data))
                Console.WriteLine($"Найдено значение: {node.Data}");
            TraverseAndProcess(node.Right, condition);
        }

        // 4. Преобразование в другое AVL-дерево (независимая копия)
        public AVLTree<T> ConvertToAVLTree()
        {
            List<T> list = new List<T>();
            CollectInOrder(Root, list);

            var newTree = new AVLTree<T>();
            foreach (var item in list)
                newTree.Insert(item);

            return newTree;
        }

        private void CollectInOrder(TreeNode<T> node, List<T> list)
        {
            if (node == null) return;
            CollectInOrder(node.Left, list);
            list.Add(node.Data);
            CollectInOrder(node.Right, list);
        }

        // 7. Очистка дерева
        public void Clear()
        {
            Root = null;
            Console.WriteLine("Дерево удалено из памяти.");
        }
    }
}
